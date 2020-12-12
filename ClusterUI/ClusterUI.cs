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

namespace ClusterUI 

    // TODO : gather more interesting clusters


{
    public partial class ClusterUI : Form
    {
        const string configPath = "../../../config/";
        private int clusterNumber = 1;
        private string pxFile;
        private string clFile;    
        private HistogramPoint[] HistogramPoints { get; set; }
        private HistogramPoint[] HistogramPixPoints { get; set; }
        private Cluster Current { get; set; }
        private ScatterChart ScatterChart { get; set; }
        private IClusterReader ClusterReader { get; set; }
        public ClusterUI()
        {
            InitializeComponent();
            ClusterHistogram.Visible = false;
            ClusterPixHistogram.Visible = false;
            ClusterReader = new MMClusterReader();

        }
        #region Event Handlers
        public void NextButtonClicked(object sender, EventArgs e)
        {
                clusterNumber++;
            Cluster cluster = ClusterReader.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), clusterNumber);
            Current = cluster;
            if (cluster != null)
            {
                HistogramPixPoints = new Histogram(cluster, pixel => pixel.ToT).Points;
                PictureBox.Image = GetClusterImage(point => point.ToT, cluster);
            }
            else
                clusterNumber--;

        }
        public void PrevButtonClicked(object sender, EventArgs e)
        {
            if (clusterNumber >= 2)
                clusterNumber--;
            Cluster cluster = ClusterReader.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), clusterNumber);
            HistogramPixPoints = new Histogram(cluster, pixel => pixel.ToT).Points;
            if (cluster != null)
            {
                HistogramPixPoints = new Histogram(cluster, pixel => pixel.ToT).Points;
                PictureBox.Image = GetClusterImage(point => point.ToT, cluster);
            }
            Current = cluster;
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
                    HistogramPixPoints = new Histogram(cluster, pixel => pixel.ToT).Points;
                    PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    PictureBox.Image = GetClusterImage(point => point.ToT, cluster);
                }
                Current = cluster;

                NowViewingLabel.Text = "Now Viewing: \n" + InViewFilePathBox.Text.Substring(InViewFilePathBox.Text.LastIndexOf('\\') + 1);
                //add info about clfile and px file and clusterIndex
            }
            catch 
            {
                return;
            }
        }
        public void View3DClicked(object sender, EventArgs e)
        {
            AnalysisPCA anal = new AnalysisPCA();
            if (Current == null)
                return;
            IZCalculator zCalculator = new ZCalculator();
            Point3D[] points3D = anal.To3DPoints(anal.Transform(Current.Points));//zCalculator.TransformPoints(Current);
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
        public void ProcessFilterClicked(object sender, EventArgs e)
        {
            Thread filteringThread = new Thread(() => ProcessFilter());
            filteringThread.Start();
            

        }
        public void BrowseFilterFileButtonClicked(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                InFilePathBox.Text = fileDialog.FileName;
            }
        }
        public void SkeletonizeButtonClicked(object sender, EventArgs e)
        {
            EnergyCalculator energyCalculator = new EnergyCalculator(new StreamReader(configPath + "a.txt"), new StreamReader(configPath + "b.txt"), new StreamReader(configPath + "c.txt"), new StreamReader(configPath + "t.txt"));
            PixelFilter haloFilter = new EnergyHaloFilter(energyCalculator);
            ISkeletonizer skeletonizer = new ThinSkeletonizer();
            Current.Points = haloFilter.Process(skeletonizer.Skeletonize(Current.Points)).ToArray();

            PictureBox.Image = GetClusterImage(point => point.ToT, Current);

        }

        #endregion
        
        public Image GetClusterImage(Func<PixelPoint, double> attributeGetter, Cluster cluster)
        {
            const int bitmapSize = 256;
            var centerCalc = new EnergyCenterFinder(new StreamReader(configPath + "a.txt"), new StreamReader(configPath + "b.txt"), new StreamReader(configPath + "c.txt"), new StreamReader(configPath + "t.txt"));
            var center = centerCalc.CalcCenterPoint(cluster.Points);
            var pixels = new Bitmap(bitmapSize, bitmapSize);
            for (int i = 0; i < bitmapSize; i++)
                for (int j = 0; j < bitmapSize; j++)
                    pixels.SetPixel(i, j, Color.Black);
            foreach (var pixel in cluster.Points)
            {
                pixels.SetPixel(pixel.xCoord, pixel.yCoord, cluster.ToColor(Math.Max(Math.Min(6 * Math.Log(attributeGetter(pixel), 1.16) / 256, 1), 0)));
                if (pixel.GetDistance(center) <= 2)
                    pixels.SetPixel(pixel.xCoord, pixel.yCoord, Color.Blue);

            }
            return pixels;
        }
        private void CreateNewIniFile(StreamReader example, StreamWriter output, string oldClFileName, string newClFileName)
        {
            while (example.Peek() != -1)
            {
                output.WriteLine(example.ReadLine().Replace(oldClFileName.Substring(PathParser.GetPrefixPath(oldClFileName).Length),
                    newClFileName));
            }
            output.Close();
        }

        private void ProcessFilter()
        { 
            try
            {
                const int minVertexCount = 3;

                var workingDirName = PathParser.GetPrefixPath(InFilePathBox.Text);
                ClusterReader.GetTextFileNames(new StreamReader(InFilePathBox.Text), InFilePathBox.Text, out string pxFile, out string clFile);
                var outClPath = clFile + "_filtered_" + DateTime.Now.ToString().Replace(':', '-') + ".cl";
                var filteredOut = new StreamWriter(outClPath);
                CreateNewIniFile(new StreamReader(InFilePathBox.Text), new StreamWriter(workingDirName + OutFileNameIniBox.Text + ".ini"), clFile, PathParser.GetSuffixPath(outClPath));

                var pixelCountFilter = new PixelCountFilter(new StreamReader(pxFile),
                    int.TryParse(FromPixCountFilterBox.Text, out int resultLowerP) ? resultLowerP : 0,
                    int.TryParse(ToPixCountFilterBox.Text, out int resultUpperP) ? resultUpperP : 100000);

                var energyFilter = new EnergyFilter(new StreamReader(pxFile), new StreamReader(configPath + "a.txt"),
                    new StreamReader(configPath + "b.txt"), new StreamReader(configPath + "c.txt"), new StreamReader(configPath + "t.txt"),
                    double.TryParse(FromEnergyFilterBox.Text, out double resultLowerE) ? resultLowerE : 0,
                    double.TryParse(ToEnergyFilterBox.Text, out double resultUpperE) ? resultUpperE : 1000000);

                var convexityFilter = new ConvexityFilter(new StreamReader(pxFile),
                     int.TryParse(FromLinearityTextBox.Text, out int resultLowerL) ? resultLowerL : 0,
                     int.TryParse(ToLinearityTextBox.Text, out int resultUpperL) ? resultUpperL : 100, ConvexitySkeletFilterCheckBox.Checked);

                var vertexCountFilter = new VertexCountFilter(new StreamReader(pxFile), minVertexCount);

                List<ClusterFilter> usedFiletrs = new List<ClusterFilter>();
                if (int.TryParse(FromPixCountFilterBox.Text, out int valInt) || int.TryParse(ToPixCountFilterBox.Text, out valInt))
                {
                    usedFiletrs.Add(pixelCountFilter);
                }
                if (double.TryParse(FromEnergyFilterBox.Text, out double valDouble) || double.TryParse(ToEnergyFilterBox.Text, out valDouble))
                {
                    usedFiletrs.Add(energyFilter);
                }
                usedFiletrs.Add(vertexCountFilter);
                if (double.TryParse(FromLinearityTextBox.Text, out  valDouble) || double.TryParse(ToLinearityTextBox.Text, out valDouble))
                {
                    usedFiletrs.Add(convexityFilter);
                }
                              
                usedFiletrs.Add(new SuccessFilter());
                var multiFilter = new MultiFilter(usedFiletrs);
                multiFilter.Process(new StreamReader(clFile), filteredOut);

                filteredOut.Close();
                MessageBox.Show("Done");
            }
            catch (IOException)
            {
                MessageBox.Show("Processing error. Input or output file is not accessible or is in incorrect format.");
                return;
            }
            catch
            {
                MessageBox.Show("Processing error. Input is in incorrect format.");
                return;
            }
        }
        public void DrawLineInt(Bitmap bmp, ConvexHull hull)
        {

            bmp.SetPixel(hull.MinPoint.xCoord, hull.MinPoint.yCoord, Color.Blue);
            for (int i = 0; i < hull.HullPoints.Count; i++)
            {
                //bmp.SetPixel(hull.HullPoints[i].xCoord, hull.HullPoints[i].yCoord, Color.Green);


                Pen blackPen = new Pen(Color.Green, 3);
                int x1 = hull.HullPoints[i % hull.HullPoints.Count].xCoord;
                int y1 = hull.HullPoints[i % hull.HullPoints.Count].yCoord;
                int x2 = hull.HullPoints[(i + 1) % hull.HullPoints.Count].xCoord;
                int y2 = hull.HullPoints[(i + 1) % hull.HullPoints.Count].yCoord;
                using (var graphics = Graphics.FromImage(bmp))
                {
                    graphics.DrawLine(blackPen, x1, y1, x2, y2);
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

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

    
}
