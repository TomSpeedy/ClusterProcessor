using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing.Text;
using ChartDirector;
using ClusterCalculator;
using System.Globalization;
using ClassifierForClusters;
using Newtonsoft.Json;

namespace ClusterUI 

{
    public partial class ClusterUI : Form
    {
        private string configPath = "../../../config/";
        private int clusterNumber = 1;
        private string pxFile;
        private string clFile;
        private HistogramPoint[] HistogramPoints { get; set; }
        private HistogramPoint[] HistogramPixPoints { get; set; }
        private Cluster Current { get; set; }
        private ScatterChart ScatterChart { get; set; }

        private IClusterReader ClusterReader { get; }
        public ClusterUI()
        {
            InitializeComponent();
            ClusterHistogram.Visible = false;
            ClusterPixHistogram.Visible = false;
            ClusterReader = new MMClusterReader();
            ConfigDirTextBox.Text = configPath;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

        }
        #region Event Handlers
        public void NextButtonClicked(object sender, EventArgs e)
        {
            clusterNumber++;
            Cluster cluster = ClusterReader.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), clusterNumber);
            if (cluster != null)
            {
                Current = cluster;
                HistogramPixPoints = new Histogram(cluster, pixel => pixel.Energy).Points;
                PictureBox.Image = GetClusterImage(point => point.Energy, cluster);
            }
            else
            {
                clusterNumber--;
            }
            ClusterIndexValueLabel.Text = clusterNumber.ToString();

        }
        public void PrevButtonClicked(object sender, EventArgs e)
        {
            if (clusterNumber >= 2)
                clusterNumber--;
            Cluster cluster = ClusterReader.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), clusterNumber);
            if (cluster != null)
            {
                Current = cluster;
                HistogramPixPoints = new Histogram(cluster, pixel => pixel.Energy).Points;
                PictureBox.Image = GetClusterImage(point => point.Energy, cluster);
            }
            ClusterIndexValueLabel.Text = clusterNumber.ToString();

        }
        public void FindClusterByIndexClicked(object sender, EventArgs e)
        {
            if (int.TryParse(FindByIndexTextBox.Text, out int result))
            {
                Cluster cluster = ClusterReader.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), result);
                if (cluster != null)
                {
                    Current = cluster;
                    HistogramPixPoints = new Histogram(cluster, pixel => pixel.Energy).Points;
                    PictureBox.Image = GetClusterImage(point => point.Energy, cluster);
                    clusterNumber = result;
                }
                else
                {
                    MessageBox.Show("Error, cluster with given index was not found.");
                }
            }
        }     
       
        public void ClassifyButtonClicked(object sender, EventArgs e)
        {
            NNInputProcessor preprocessor = new NNInputProcessor();
            var attributePairs = new Dictionary<ClusterAttribute, object>();
            IClassifier classifier = new MultiLayeredClassifier();
            classifier.Load();
            IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
            foreach (var checkedAttribute in classifier.ValidFields)
            {
                var attributeName = checkedAttribute;
                attributePairs.Add(attributeName, null);
                attributesToGet.Add(attributeName);
            }
            DefaultAttributeCalculator attributeCalculator = new DefaultAttributeCalculator();
            attributeCalculator.Calculate(Current, attributesToGet, ref attributePairs);
            string jsonString = JsonConvert.SerializeObject(attributePairs, Formatting.Indented);
            StringReader sReader = new StringReader(jsonString);
            JsonTextReader jReader = new JsonTextReader(sReader);
            var attrVector = preprocessor.ReadJsonToVector(jReader, classifier.ValidFields);
            var classInfo = classifier.Classify(attrVector);
            ClusterClassLabel.Text = $"Calculated Class: {classInfo.MostProbableClassName}";
        }
        public void BrowseViewButtonClicked(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                InViewFilePathBox.Text = fileDialog.FileName;
            }
        }
        public void BrowseConfigButtonClicked(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ConfigDirTextBox.Text = dialog.SelectedPath;
                }
            }
        }
        public void LoadClustersClicked(object sender, EventArgs e)
        {
            try
            {
                configPath = ConfigDirTextBox.Text + "\\";
                ClusterReader.GetTextFileNames(new StreamReader(InViewFilePathBox.Text), InViewFilePathBox.Text, out pxFile, out clFile);
                Cluster cluster = ClusterReader.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), clusterNumber);

                if (cluster != null)
                {
                    HistogramPixPoints = new Histogram(cluster, pixel => pixel.Energy).Points;
                    PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    PictureBox.Image = GetClusterImage(point => point.Energy, cluster);
                }

                Current = cluster;
                ClusterIndexValueLabel.Text = clusterNumber.ToString();
                NowViewingLabel.Text = "Now Viewing: \n" + InViewFilePathBox.Text.Substring(InViewFilePathBox.Text.LastIndexOf('\\') + 1);
            }
            catch 
            {
                MessageBox.Show("File was not load because of an error. Please check format of the file");
            }
        }
        public void View3DClicked(object sender, EventArgs e)
        {
            //AnalysisPCA anal = new AnalysisPCA();
            if (Current == null)
                return;
            IZCalculator zCalculator = new ZCalculator();
           /*Point3D[] points3D = ToPoints3D(anal.Transform(Current.Points))*/ var points3D = ToPoints3D(zCalculator.TransformPoints(Current));
            ScatterChart chart = new ScatterChart(winChartViewer, points3D);
            ScatterChart = chart;
        }
        public void HideHistogramClicked(object sender, EventArgs e)
        {
            ClusterHistogram.Visible = false;
        }
        public void HidePixHistogramClicked(object sender, EventArgs e)
        {
            ClusterPixHistogram.Visible = false;
        }
        public void ShowHistogramClicked(object sender, EventArgs e)
        {
            //if (HistogramPoints == null)
               // return;
            HistogramPoints = new Histogram(new StreamReader(clFile), cl => ((double)cl.PixCount)).Points;
            ClusterHistogram.Visible = true;
            ClusterHistogram.Series.Clear();
            ClusterHistogram.Palette = ChartColorPalette.BrightPastel;
            if (ClusterHistogram.Titles.Count == 0)
            {
                ClusterHistogram.Titles.Add("Cluster Collection Histogram");
            }
            var chartArea = new ChartArea();
            ClusterHistogram.ChartAreas.Clear();
            ClusterHistogram.ChartAreas.Add(new ChartArea());
            ClusterHistogram.ChartAreas[0].AxisX.Title = "Pixel Count";
            ClusterHistogram.ChartAreas[0].AxisY.Title = "Cluster Count";
            Series series = ClusterHistogram.Series.Add("Number of clusters with given pixel count");
            for (int i = 0; i < HistogramPoints.Length; i++)
            {
                series.Points.AddXY(HistogramPoints[i].X, HistogramPoints[i].Y);

            }
            ClusterHistogram.ChartAreas[0].AxisX.RoundAxisValues();
        }
        public void ShowPixHistogramClicked(object sender, EventArgs e)
        {
            if (HistogramPixPoints == null)
                return;
            ClusterPixHistogram.Visible = true;
            ClusterPixHistogram.Series.Clear();
            ClusterPixHistogram.Palette = ChartColorPalette.BrightPastel;
            if (ClusterPixHistogram.Titles.Count == 0)
            {
                ClusterPixHistogram.Titles.Add("Cluster Collection Histogram");
            }
            var chartArea = new ChartArea();
            ClusterPixHistogram.ChartAreas.Clear();
            ClusterPixHistogram.ChartAreas.Add(new ChartArea());
            ClusterPixHistogram.ChartAreas[0].AxisX.Title = "ToT";
            ClusterPixHistogram.ChartAreas[0].AxisY.Title = "Pixel Count";



            Series series = ClusterPixHistogram.Series.Add("Number of pixels with given ToT");
            for (int i = 0; i < HistogramPixPoints.Length; i++)
            {
                series.Points.AddXY(HistogramPixPoints[i].X, HistogramPixPoints[i].Y);

            }
            ClusterPixHistogram.ChartAreas[0].AxisX.RoundAxisValues();
        }
        public void Rotate3DPlot(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == RotateUpButton)
                ScatterChart.angleVert += 10;
            else if (button == RotateDownButton)
                ScatterChart.angleVert -= 10;
            else if (button == RotateLeftButton)
                ScatterChart.angleHoriz += 10;
            else if (button == RotateRightButton)
                ScatterChart.angleHoriz -= 10;
            ScatterChart.angleVert %= 360;
            ScatterChart.angleHoriz %= 360;
            winChartViewer.Chart = ScatterChart.RotateChart(ScatterChart.angleVert, ScatterChart.angleHoriz);
                

        }
        public void SkeletonizeButtonClicked(object sender, EventArgs e)
        {
            if (Current == null)
                return;
            PixelFilter haloFilter = new EnergyHaloFilter();
            ISkeletonizer skeletonizer = new ThinSkeletonizer();
            //opt 1 Current.Points = skeletonizer.SkeletonizePoints(Current.Points).ToArray();
            //opt2
            var currentSkelet = new Cluster(Current.FirstToA, Current.PixelCount, Current.ByteStart);
            currentSkelet.Points = skeletonizer.SkeletonizePoints(Current.Points).ToArray();
            PictureBox.Image = GetClusterImage(point => point.Energy, currentSkelet);

        }
   
        public void ViewBranchButtonClicked(object sender, EventArgs e)
        {
            var skeletonizer = new ThinSkeletonizer();
            var skeletCluster = new Cluster(Current.FirstToA, Current.PixelCount, Current.ByteStart);
            skeletCluster.Points = skeletonizer.SkeletonizePoints(Current.Points);
            //Current.Points = skeletonizer.SkeletonizePoints(Current.Points);
            var centerCalc = new EnergyCenterFinder();
            var center = centerCalc.CalcCenterPoint(skeletCluster, Current.Points);
            var branchAnalyzer = new BranchAnalyzer(centerCalc);
            var analyzedCluster = branchAnalyzer.Analyze(skeletCluster, Current);
            var mainBranches = analyzedCluster.MainBranches;
            Color[] branchColors = { Color.Blue, Color.Green, Color.Red, Color.Yellow };
            foreach (var pixel in skeletCluster.Points)
                ((Bitmap)PictureBox.Image).SetPixel(pixel.xCoord, pixel.yCoord, Color.Black);
            for (int i = 0; i < mainBranches.Count; i++)
                PictureBox.Image = DrawBranches(mainBranches[i], branchColors[i % branchColors.Length]);
            ((Bitmap)PictureBox.Image).SetPixel(analyzedCluster.Center.xCoord, analyzedCluster.Center.yCoord, Color.White);
            //PictureBox.Image = GetClusterImage(pix => pix.ToT, Current);
        }
        #endregion
        public Image GetClusterImage(Func<PixelPoint, double> attributeGetter, Cluster cluster)
        {
            const int bitmapSize = 256;
            var centerCalc = new EnergyCenterFinder();
            //var center = centerCalc.CalcCenterPoint(cluster.Points);

            var vertexFinder = new VertexFinder();

            //var possibleCentersFinder = new NeighbourCountFilter(neighBourCount => neighBourCount >= 3, NeighbourCountOption.WithYpsilonNeighbours);
            //var vertices = vertexFinder.FindVertices(cluster.Points);
            //var centers = possibleCentersFinder.Process(cluster.Points);

            var pixels = new Bitmap(bitmapSize, bitmapSize);
            for (int i = 0; i < bitmapSize; i++)
                for (int j = 0; j < bitmapSize; j++)
                    pixels.SetPixel(i, j, Color.Black);
            foreach (var pixel in cluster.Points)
            {
                pixels.SetPixel(pixel.xCoord, pixel.yCoord, cluster.ToColor(Math.Max(Math.Min(6 * Math.Log(attributeGetter(pixel), 1.16) / 256, 1), 0)));

                /*if (pixel.GetDistance(center) <= 1)
                    pixels.SetPixel(pixel.xCoord, pixel.yCoord, Color.Blue);
                else if (vertices.Contains(pixel))
                    pixels.SetPixel(pixel.xCoord, pixel.yCoord, Color.Purple);
                else if (centers.Contains(pixel))
                    pixels.SetPixel(pixel.xCoord, pixel.yCoord, Color.Green);

                */
            }
            return pixels;
        }

        public Image DrawBranches(Branch branch, Color color)
        {
            Bitmap bitmap = (Bitmap)PictureBox.Image;
            var branchListPoints = branch.Points.ToList();

            for (int i = 0; i < branch.Points.Count; ++i)
            {               
                bitmap.SetPixel(branchListPoints[i].xCoord, branchListPoints[i].yCoord, color);
            }
            foreach (var subBranch in branch.SubBranches)
            {
                DrawBranches(subBranch, ControlPaint.Light(color));
            }
            return bitmap;
        }

        private Point3D[] ToPoints3D(PointD3[] points)
        {
            var newPoints = new Point3D[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                newPoints[i] = new Point3D(points[i].X, points[i].Y, points[i].Z);
            }
            return newPoints;
        }
    }

    struct HistogramPoint
    {
        public double X { get; set; }
        public int Y { get; set; }
    }

    public delegate double Attribute<T>(T attributeOwner);
   
    class Histogram
    {
        const int bucketCount = 100;
        double MinValue { get; set; }
        double MaxValue { get; set; }
        public HistogramPoint[] Points = new HistogramPoint[bucketCount];
        public Histogram (StreamReader clFile, Attribute<ClusterInfo> attributeGetter)
        {
            double[] buckets = new double[bucketCount];
            (MinValue, MaxValue) = FindCollectionRange(clFile, attributeGetter);
            clFile.BaseStream.Position = 0;
            clFile.DiscardBufferedData();
            InitPoints();
            foreach (var clInfo in new ClusterInfoCollection(clFile))
            {
                double attribute = attributeGetter(clInfo);
                if ((attribute == MaxValue) && (Math.Floor(attribute) == attribute))
                    Points[bucketCount - 1].Y++;
                else
                    Points[(int)Math.Floor(bucketCount * ((attribute - MinValue) / (MaxValue - MinValue)))].Y++;
            }

                    
            
            
        }
        public Histogram(Cluster cluster, Attribute<PixelPoint> attributeGetter)
        {
            double[] buckets = new double[bucketCount];
            (MinValue, MaxValue) = FindPixelRange(cluster, attributeGetter);
            InitPoints();
            for (int i = 0; i < cluster.Points.Length; i++)
            {
                
                double attribute = attributeGetter(cluster.Points[i]);
                if (attribute == MaxValue)
                    Points[bucketCount - 1].Y++;
                else
                    Points[(int)Math.Floor(bucketCount * ((attribute - MinValue) / (MaxValue - MinValue)))].Y++;
                
            }

        }
        private void InitPoints()
        {
            for (int i = 0; i < bucketCount; i++)
            {
                Points[i].X = (((MaxValue - MinValue) / bucketCount) * (i + 0.5)) + MinValue; //sets middle points for buckets
                Points[i].Y = 0;
            }
        }
        /// <summary>
        /// Determines range of x axis in collection histogram
        /// </summary>
        /// <param name="clFile"></param>
        /// <param name="pxFile"></param>
        /// <param name="attributeGetter"></param>
        /// <returns></returns>
        private (double min,double max) FindCollectionRange(StreamReader clFile, Attribute<ClusterInfo> attributeGetter)
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            foreach (var clInfo in new ClusterInfoCollection(clFile))
            { 
                var attribute = attributeGetter(clInfo);
                if (attribute > max)
                    max = attribute;
                if (attribute < min)
                    min = attribute;               
            }
            return (min, max);
            
        }
        private (double min, double max) FindPixelRange(Cluster cluster, Attribute<PixelPoint> attributeGetter)
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            for (int i = 0; i < cluster.Points.Length; i++)
            {
                var attributeValue = attributeGetter(cluster.Points[i]);
                if (attributeValue < min)
                    min = attributeValue;
                if (attributeValue > max)
                    max = attributeValue;
            }
            return (min, max);
        }


    }
    public class ScatterChart
    {
        private readonly ThreeDScatterChart chart;
        private readonly double[] xData;
        private readonly double[] zData;
        private readonly double[] yData;
        public int angleVert { get; set; }
        public int angleHoriz { get; set; }
        const string Title = "3D Trajectory";
        //Name of demo module
        public string getName() { return Title; }
        public Point3D ToPoint3D(PointD3 point)
        {
            return new Point3D(point.X, point.Y, point.Z);
        }
        //Number of charts produced in this demo module
        public int getNoOfCharts() { return 1; }

        //Main code for creating chart.
        //Note: the argument chartIndex is unused because this demo only has 1 chart.
        public ScatterChart(WinChartViewer viewer, Point3D[] points3D)
        {
            xData = points3D.Select(point => (double)point.X).ToArray();
            yData = points3D.Select(point => (double)point.Y).ToArray();
            zData = points3D.Select(point => (double)point.Z / 10D).ToArray();

            // Create a ThreeDScatterChart object of size 720 x 600 pixels
            var chart = new ThreeDScatterChart(viewer.Width, viewer.Height);
            chart.setPlotRegion((viewer.Width / 2), (viewer.Height / 2), 200, 200, 200);
            chart.setColorAxis(370, 190, ChartDirector.Chart.Left, 300, ChartDirector.Chart.Right);

            Initialize(chart);
            // Output the chart
            viewer.Chart = chart;
            //Save chart
            this.chart = chart;
        }
        public ThreeDScatterChart RotateChart(int angleVert, int angleHoriz)
        {
            var chart = new ThreeDScatterChart(this.chart.getWidth(), this.chart.getHeight());
            chart.setPlotRegion((this.chart.getWidth() / 2), (this.chart.getHeight() / 2), 200, 200, 200);


            Initialize(chart);
            chart.setViewAngle(angleVert, angleHoriz);
            //viewer.ImageMap = chart.getHTMLImageMap("clickable", "",
            return chart;
        }
        private void Initialize(ThreeDScatterChart chart)
        {
            const string TitleFont = "Times New Roman Italic";
            const string AxisLabelFont = "Arial Bold";
            const int AxisFontSize = 10;
            chart.addTitle(Title, TitleFont, 18);
            // Add a scatter group to the chart using 11 pixels glass sphere symbols, in which the
            // color depends on the z value of the symbol
            chart.addScatterGroup(this.xData, this.yData, this.zData, "Trajectory", ChartDirector.Chart.GlassSphere2Shape, 11,
                ChartDirector.Chart.SameAsMainColor);
            // Set the x, y and z axis titles using 10 points Arial Bold font
            chart.xAxis().setTitle("X-Axis", AxisLabelFont, AxisFontSize);
            chart.yAxis().setTitle("Y-Axis", AxisLabelFont, AxisFontSize);
            chart.zAxis().setTitle("Z-Axis", AxisLabelFont, AxisFontSize);
        }
    }

}
