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
        private Cluster CurrentBase { get; set; }
        private Dictionary<PixelPoint, Color> CurrentImage { get; set; }
    private System.Windows.Forms.Timer RotationTimer { get; set; }
        private Button CurrentlyPressedRotateButton { get; set; }
        private ScatterChart ScatterPlotChart { get; set; }

        private IClusterReader ClusterReader { get; }
        public ClusterUI()
        {
            InitializeComponent();
            ClusterHistogram.Visible = false;
            ClusterPixHistogram.Visible = false;
            ClusterReader = new MMClusterReader();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

        }
        #region Event Handlers
        public void NextButtonClicked(object sender, EventArgs e)
        {
            clusterNumber++;
            Cluster cluster = ClusterReader.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), clusterNumber);
            if (cluster != null)
            {
                CurrentBase = cluster;
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
                CurrentBase = cluster;
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
                    CurrentBase = cluster;
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
            const string NNFragHeFe = "../../../ClassifierForClusters/trained_models/trainFragHeFeNew.json_trained 0.961.txt";
            const string NNPrLe = "../../../ClassifierForClusters/trained_models/trainPrLe_ElMuPi.json_trained 0.966.txt";
            const string NNLead = "../../../ClassifierForClusters/trained_models/trainLeadMulti.json_trained 0.909.txt";
            const string NNElMuPi = "../../../ClassifierForClusters/trained_models/trainElMuPi.json_trained 0.801.txt";
            


            NNInputProcessor preprocessor = new NNInputProcessor();
            var attributePairs = new Dictionary<ClusterAttribute, object>();
            IClassifier classifier = new MultiLayeredClassifier();
            classifier.Load(new string[] { NNLead, NNFragHeFe, NNPrLe, NNElMuPi });
            IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
            foreach (var checkedAttribute in classifier.ValidFields)
            {
                var attributeName = checkedAttribute;
                attributePairs.Add(attributeName, null);
                attributesToGet.Add(attributeName);
            }
            DefaultAttributeCalculator attributeCalculator = new DefaultAttributeCalculator();
            attributeCalculator.Calculate(CurrentBase, attributesToGet, ref attributePairs);
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

        public void LoadClustersClicked(object sender, EventArgs e)
        {
            try
            {

                ClusterReader.GetTextFileNames(new StreamReader(InViewFilePathBox.Text), InViewFilePathBox.Text, out pxFile, out clFile);
                Cluster cluster = ClusterReader.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), clusterNumber);

                if (cluster != null)
                {
                    HistogramPixPoints = new Histogram(cluster, pixel => pixel.Energy).Points;
                    PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    PictureBox.Image = GetClusterImage(point => point.Energy, cluster);
                }
                CurrentBase = cluster;
                ClusterIndexValueLabel.Text = clusterNumber.ToString();
                NowViewingLabel.Text = "Now Viewing: \n" + InViewFilePathBox.Text.Substring(InViewFilePathBox.Text.LastIndexOf('\\') + 1);
                EnableButtons();
            }
            catch 
            {
                MessageBox.Show("File was not load because of an error. Please check format of the file");
            }
        }
        public void View3DClicked(object sender, EventArgs e)
        {
            //AnalysisPCA anal = new AnalysisPCA();
            if (CurrentBase == null)
                return;
            IZCalculator zCalculator = new ZCalculator();
           /*Point3D[] points3D = ToPoints3D(anal.Transform(Current.Points))*/ 
            var points3D = ToPoints3D(zCalculator.TransformPoints(CurrentBase));
            ScatterChart chart = new ScatterChart(winChartViewer, points3D);
            ScatterPlotChart = chart;
            EnableRotateButtons();
        }
        public void HideHistogramClicked(object sender, EventArgs e)
        {
            ClusterHistogram.Visible = false;
        }
        public void HidePixHistogramClicked(object sender, EventArgs e)
        {
            ClusterPixHistogram.Visible = false;
        }
        public async void ShowHistogramClicked(object sender, EventArgs e)
        {
            //if (HistogramPoints == null)
            // return;
            HistogramPoint[] HistogramPoints = null;
            Thread histogramCalculationThread = new Thread(() => { HistogramPoints = new Histogram(new StreamReader(clFile), cl => ((double)cl.PixCount)).Points; });

            await Task.Factory.StartNew(() => { HistogramPoints = new Histogram(new StreamReader(clFile), cl => ((double)cl.PixCount)).Points; },
                                        TaskCreationOptions.LongRunning);

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
                ClusterPixHistogram.Titles.Add("Pixel Histogram - Current Cluster");
            }
            var chartArea = new ChartArea();
            ClusterPixHistogram.ChartAreas.Clear();
            ClusterPixHistogram.ChartAreas.Add(new ChartArea());
            ClusterPixHistogram.ChartAreas[0].AxisX.Title = "Energy in keV";
            ClusterPixHistogram.ChartAreas[0].AxisY.Title = "Pixel Count";



            Series series = ClusterPixHistogram.Series.Add("Number of pixels with given Energy");
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
                ScatterPlotChart.angleVert += 5;
            else if (button == RotateDownButton)
                ScatterPlotChart.angleVert -= 5;
            else if (button == RotateLeftButton)
                ScatterPlotChart.angleHoriz += 5;
            else if (button == RotateRightButton)
                ScatterPlotChart.angleHoriz -= 5;
            ScatterPlotChart.angleVert %= 360;
            ScatterPlotChart.angleHoriz %= 360;
            winChartViewer.Chart = ScatterPlotChart.RotateChart(ScatterPlotChart.angleVert, ScatterPlotChart.angleHoriz);
                

        }
        public void Rotate3DPlotHoldDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var button = sender as Button;
            if(RotationTimer == null)
            {
                RotationTimer = new System.Windows.Forms.Timer();
                RotationTimer.Interval = 100;
                RotationTimer.Tick += Rotate3DPlotTimerTick;
            }
            CurrentlyPressedRotateButton = button;
            RotationTimer.Start();
        }
        public void Rotate3DPlotTimerTick(object sender, EventArgs e)
        {
            Rotate3DPlot(CurrentlyPressedRotateButton, e);
        }
        public void Rotate3DPlotHoldUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
                RotationTimer.Stop();             
        }
        double ZoomFactor = -10;
        double CenterImageX = 0.5;
        double CenterImageY = 0.5;
        int CurrentZoom = 1;
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (e.Delta <= 0)
                {
                    //set minimum size to zoom
                    if (PictureBox.Width < 50)
                        // lbl_Zoom.Text = pictureBox1.Image.Size; 
                        return;
                }
                else
                {
                    //set maximum size to zoom
                    if (PictureBox.Width > 1000)
                        return;
                }
                ZoomFactor += e.Delta / (double)100;
                ZoomFactor = Math.Min(Math.Max(-10, ZoomFactor), 10);
                int realZoom = (int)Math.Round(0.5 * (ZoomFactor + 10) + 1);
                if (realZoom == 1)
                {
                    CenterImageY = 0.5;
                    CenterImageX = 0.5;
                }
                else
                {
                    CenterImageX = (((1 / (double)CurrentZoom) * (e.X - PictureBox.Location.X) / (double)PictureBox.Width) + CenterImageX - (1 / (double)(CurrentZoom * 2)));
                    CenterImageY = (((1 / (double)CurrentZoom) * (e.Y - PictureBox.Location.Y) / (double)PictureBox.Height) + CenterImageY - (1 / (double)(CurrentZoom * 2)));
                }
                Bitmap zoomedImage = (Bitmap)PictureBoxZoom(PictureBox.Image, realZoom, CenterImageX ,CenterImageY);
                PictureBox.Image = zoomedImage;
                PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                CurrentZoom = realZoom;
                ZoomLabel.Text = $"Zoom: {CurrentZoom}x";
            }
        }
        public Image PictureBoxZoom(Image img, int size, double centerX, double centerY)
        {
            return GetClusterImage(point => point.Energy, CurrentBase, false,size) ;

            //grap.InterpolationMode = InterpolationMode.HighQualityBicubic;

        }
        public void PictureBoxHover(MouseEventArgs e)
        {
            PictureBox.Focus();
        }
        public void SkeletonizeButtonClicked(object sender, EventArgs e)
        {
            if (CurrentBase == null)
                return;
            PixelFilter haloFilter = new EnergyHaloFilter();
            ISkeletonizer skeletonizer = new ThinSkeletonizer();
            Cluster skeleton = skeletonizer.SkeletonizeCluster(CurrentBase);
            PictureBox.Image = GetClusterImage(point => point.Energy, skeleton);

        }
   
        public void ViewBranchButtonClicked(object sender, EventArgs e)
        {
            CurrentImage = new Dictionary<PixelPoint, Color>();
            var skeletonizer = new ThinSkeletonizer();
            var skeletCluster = new Cluster(CurrentBase.FirstToA, CurrentBase.PixelCount, CurrentBase.ByteStart);
            skeletCluster.Points = skeletonizer.SkeletonizePoints(CurrentBase.Points);
            //Current.Points = skeletonizer.SkeletonizePoints(Current.Points);
            var NeighCountFilter = new NeighbourCountFilter(nCount => nCount >= 3, NeighbourCountOption.WithYpsilonNeighbours);
            var centerCalc = new EnergyCenterFinder();
            var center = centerCalc.CalcCenterPoint(skeletCluster, CurrentBase.Points);
            var branchAnalyzer = new BranchAnalyzer(centerCalc);
            var analyzedCluster = branchAnalyzer.Analyze(skeletCluster, CurrentBase);
            var mainBranches = analyzedCluster.MainBranches;
            Color[] branchColors = { Color.Blue, Color.Green, Color.Red, Color.Yellow };
            foreach (var pixel in skeletCluster.Points)
            {
                ((Bitmap)PictureBox.Image).SetPixel(pixel.xCoord, pixel.yCoord, Color.Black);                
            }
            for (int i = 0; i < mainBranches.Count; i++)
                PictureBox.Image = DrawBranches(mainBranches[i], branchColors[i % branchColors.Length]);
            ((Bitmap)PictureBox.Image).SetPixel(analyzedCluster.Center.xCoord, analyzedCluster.Center.yCoord, Color.White);
            CurrentImage.InsertOrAssign(analyzedCluster.Center, Color.White);
            foreach (var pix in NeighCountFilter.Process(skeletCluster.Points))
            {
               // ((Bitmap)PictureBox.Image).SetPixel(analyzedCluster.Center.xCoord, analyzedCluster.Center.yCoord, Color.Pink);
                //CurrentImage.InsertOrAssign(pix, Color.Pink);
            }
            //PictureBox.Image = GetClusterImage(pix => pix.ToT, Current);
        }
        public void ShowAttributesClicked(object sender, EventArgs e)
        {
            IClassifier classifier = new MultiLayeredClassifier();
            var attributePairs = new Dictionary<ClusterAttribute, object>();
            IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
            foreach (var checkedAttribute in classifier.ValidFields)
            {
                var attributeName = checkedAttribute;
                attributePairs.Add(attributeName, null);
                attributesToGet.Add(attributeName);
            }
            attributePairs.Add(ClusterAttribute.Branches, null);
            attributesToGet.Add(ClusterAttribute.Branches);
            DefaultAttributeCalculator attributeCalculator = new DefaultAttributeCalculator();
            attributeCalculator.Calculate(CurrentBase, attributesToGet, ref attributePairs);
            Form attributeForm = new Form()
            {
                Width = 300,
                Height = 500,


            };
            System.Windows.Forms.TextBox attributeTextBox = new System.Windows.Forms.TextBox()
            {
                Parent = attributeForm,
                Width = 300,
                Height = 500,
                Multiline = true,
                Text = JsonConvert.SerializeObject(attributePairs, Formatting.Indented)

            }; 
            attributeForm.Controls.Add(attributeTextBox);
            attributeForm.Show();
        }
        #endregion
        public Image GetClusterImage(Func<PixelPoint, double> attributeGetter, Cluster cluster,bool storePixels = true, int zoom = 1)
        {
            const int bitmapSize = 256;
            var centerCalc = new EnergyCenterFinder();
            var vertexFinder = new VertexFinder();
            //var center = centerCalc.CalcCenterPoint(cluster.Points);
            if (storePixels)
            {
                CurrentImage = new Dictionary<PixelPoint, Color>();

                foreach (var pixel in cluster.Points)
                {
                    CurrentImage.Add(pixel, cluster.ToColor(Math.Max(Math.Min(6 * Math.Log(attributeGetter(pixel), 1.16) / 256, 1), 0)));
                }
            }
            var pixels = new Bitmap(bitmapSize, bitmapSize);
            for (int i = 0; i < 255; i++)
                for (int j = 0; j < 255; j++)
                    pixels.SetPixel(i, j, Color.Black);
            var upperLeftX = (int)(CenterImageX* bitmapSize * zoom) - 128;
            var upperLeftY = (int)(CenterImageY * bitmapSize * zoom) - 128;
            foreach (var pixel in CurrentImage.Keys)
            {
                for (int zoomI = 0; zoomI < zoom; zoomI++)
                    for (int zoomJ = 0; zoomJ < zoom; zoomJ++)
                    {
                        var scaledX = pixel.xCoord * zoom + zoomI;
                        var scaledY = pixel.yCoord * zoom + zoomJ;
                        if (scaledX >= upperLeftX && scaledX < upperLeftX + 256 && scaledY >= upperLeftY && scaledY < upperLeftY + 256)
                            pixels.SetPixel(scaledX - upperLeftX, scaledY - upperLeftY, CurrentImage[pixel]);
                    }
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
                CurrentImage.InsertOrAssign(new PixelPoint(branchListPoints[i].xCoord, branchListPoints[i].yCoord), color);
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
        private void EnableButtons()
        {
            foreach (Control control in Controls)
            {
                Button button = control as Button;
                if (button != null)
                {
                    button.Enabled = true;
                }
                GroupBox groupBox = control as GroupBox;
                if(groupBox != null)
                    foreach(Control controlInGroupBox in groupBox.Controls)
                    {
                        Button buttonInGroupBox = controlInGroupBox as Button;
                        if (buttonInGroupBox != null)
                        {
                        buttonInGroupBox.Enabled = true;
                        }
                    }
            }
            RotateLeftButton.Enabled = false;
            RotateRightButton.Enabled = false;
            RotateUpButton.Enabled = false;
            RotateDownButton.Enabled = false;
                
        }

        private void EnableRotateButtons()
        {
            RotateLeftButton.Enabled = true;
            RotateRightButton.Enabled = true;
            RotateUpButton.Enabled = true;
            RotateDownButton.Enabled = true;
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
    
}public static class DictionaryExtensions
{
    public static void InsertOrAssign<Tkey, TVal>(this Dictionary<Tkey, TVal> dict, Tkey key, TVal value)
    {
        if (dict.ContainsKey(key))
            dict[key] = value;
        else
            dict.Add(key, value);
    }
}
