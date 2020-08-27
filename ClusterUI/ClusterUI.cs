using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing.Text;

namespace ClusterUI //TODO : Hull for less points than 3
{
    public partial class ClusterUI : Form
    {
        //const string confFileName = "info.ini";
        int clusterNumber = 1;
        //const string filteredFile = "filtered.txt";
        //public PictureBox PictureBox = new PictureBox();
        string pxFile;
        string clFile;
        HistogramPoint[] HistogramPoints;
        public void NextButtonClicked(object sender, EventArgs e)
        {
            if (clusterNumber < HistogramPoints.Sum(point => point.Y))
                clusterNumber++;
            Cluster cluster = Cluster.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), true, clusterNumber);
            if (cluster != null)
            { 
                PictureBox.Image = GetClusterImage(point => point.ToT, cluster);
                var hull = new ConvexHull(cluster.Points);
                DrawLineInt((Bitmap)this.PictureBox.Image, hull);
            }
        }
        public void PrevButtonClicked(object sender, EventArgs e)
        {
            if (clusterNumber >= 1)
                clusterNumber--; 
            Cluster cluster = Cluster.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), true, clusterNumber);
            if (cluster != null)
            {
                PictureBox.Image = GetClusterImage(point => point.ToT, cluster);
                var hull = new ConvexHull(cluster.Points);
                DrawLineInt((Bitmap)this.PictureBox.Image, hull);
            }
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
           Cluster.GetTextFileNames(new StreamReader(InViewFilePathBox.Text), InViewFilePathBox.Text, out pxFile, out clFile);
           Cluster cluster = Cluster.LoadFromText(new StreamReader(pxFile), new StreamReader(clFile), true, clusterNumber);

           HistogramPoints = (new Histogram(new StreamReader(clFile), new StreamReader(pxFile), cl => (double)cl.PixelCount)).Points;
           PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

           if (cluster != null)
           {
               PictureBox.Image = GetClusterImage(point => point.ToT, cluster);
               //var hull = new ConvexHull(cluster.Points);
           }

       }
       public void ShowHistogramClicked(object sender, EventArgs e)
       {
           if (HistogramPoints == null)
               return;
           ClusterHistogram.Visible = true;
           ClusterHistogram.Series.Clear();
           // Set palette
           ClusterHistogram.Palette = ChartColorPalette.BrightPastel;
           // Set title
           if (ClusterHistogram.Titles.Count == 0)
           {
               ClusterHistogram.Titles.Add("Cluster Histogram");
               var chartArea = new ChartArea();
               ClusterHistogram.ChartAreas.Add(new ChartArea());
               ClusterHistogram.ChartAreas[0].AxisX.Title = "Pixel Count";
               ClusterHistogram.ChartAreas[0].AxisY.Title = "Cluster Count";
           }
               //ClusterHistogram.ChartAreas[0].Position.Height = 50;



           Series series = ClusterHistogram.Series.Add("Number of clusters with given pixel count");
           for (int i = 0; i < HistogramPoints.Length; i++)
           {
               series.Points.AddXY(HistogramPoints[i].X, HistogramPoints[i].Y);

           }
           ClusterHistogram.ChartAreas[0].AxisX.RoundAxisValues();
       }
       public void ProcessFilterClicked(object sender, EventArgs e)
       {

           string tempFileName = "temporaryFilteredOut"+ String.Format("{0:y/M/d/h/m/s/fff}", DateTime.Now);
           var workingDirName = Cluster.GetPrefixPath(InFilePathBox.Text);
           Cluster.GetTextFileNames(new StreamReader(InFilePathBox.Text), InFilePathBox.Text, out string pxFile, out string clFile);
           var filteredOut = new StreamWriter(workingDirName + OutFileNameClBox.Text);
           var tempFile = new StreamWriter(tempFileName);
           CreateNewIniFile(new StreamReader(InFilePathBox.Text), new StreamWriter(workingDirName + OutFileNameIniBox.Text), clFile, OutFileNameClBox.Text);
            
           var pixelCountFilter = new PixelCountFilter(new StreamReader(pxFile), 
               int.TryParse(FromPixCountFilterBox.Text, out int resultLowerP) ? resultLowerP : 0,
               int.TryParse(ToPixCountFilterBox.Text, out int resultUpperP) ? resultUpperP : 100000); 
           pixelCountFilter.Process(new StreamReader(clFile), tempFile);
           tempFile.Close();
           tempFile.Dispose();

           var energyFilter = new EnergyFilter(new StreamReader(pxFile), new StreamReader(workingDirName + "a.txt"),
               new StreamReader(workingDirName + "b.txt"), new StreamReader(workingDirName + "c.txt"), new StreamReader(workingDirName + "t.txt"),
               double.TryParse(FromEnergyFilterBox.Text, out double resultLowerE) ? resultLowerE : 0,
               double.TryParse(ToEnergyFilterBox.Text, out double resultUpperE) ? resultUpperE : 1000000);
           var tempFileR = new StreamReader(tempFileName);
           energyFilter.Process(tempFileR, filteredOut);
           tempFileR.Close();
           tempFileR.Dispose();
           File.Delete(tempFileName);
           filteredOut.Close();

       }
       public void BrowseFilterFileButtonClicked(object sender, EventArgs e)
       {
           var fileDialog = new OpenFileDialog();
           if (fileDialog.ShowDialog() == DialogResult.OK)
           {
               InFilePathBox.Text = fileDialog.FileName;
           }
       }
       public Image GetClusterImage(Func<PixelPoint, double> attributeGetter, Cluster cluster)
       {
           var pixels = new Bitmap(256, 256);
           for (int i = 0; i < 256; i++)
               for (int j = 0; j < 256; j++)
                   pixels.SetPixel(i, j, Color.FromArgb(255, 0, 0, 0));
           foreach (var pixel in cluster.Points)
           {
               pixels.SetPixel(pixel.xCoord, pixel.yCoord, cluster.ToColor(Math.Max(Math.Min(4 * Math.Log(attributeGetter(pixel), 1.13) / 256, 1), 0)));
           }
           return pixels;
       }
       private void CreateNewIniFile(StreamReader example, StreamWriter output, string oldClFileName, string newClFileName)
       {
           while (example.Peek() != -1)
           {
               output.WriteLine(example.ReadLine().Replace(oldClFileName.Substring(Cluster.GetPrefixPath(oldClFileName).Length), 
                   newClFileName));
           }
           output.Close();
       }
       public ClusterUI()
       {
           InitializeComponent();
           ClusterHistogram.Visible = false;
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
    public delegate double ClusterAttribute<Cluster>(Cluster cluster);
    class Histogram
    {
        const int bucketCount = 100;
        double MinValue { get; set; }
        double MaxValue { get; set; }
        public HistogramPoint[] Points = new HistogramPoint[bucketCount];
        public Histogram (StreamReader clFile, StreamReader pxFile, ClusterAttribute<Cluster> attributeGetter)
        {
            double[] buckets = new double[bucketCount];
            (MinValue, MaxValue) = FindRange(clFile, pxFile, attributeGetter);
            clFile.BaseStream.Position = 0;
            clFile.DiscardBufferedData();
            InitPoints();
            while (clFile.Peek() != -1)
            {
                var cluster = Cluster.LoadFromText(pxFile, clFile, false); 
                double attribute =  attributeGetter(cluster);
                if ((attribute == MaxValue) && (Math.Floor(attribute) == attribute))
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
        private (double min,double max) FindRange(StreamReader clFile, StreamReader pxFile, ClusterAttribute<Cluster> attributeGetter)
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            while (clFile.Peek() != -1)
            {
                var cluster = Cluster.LoadFromText(pxFile, clFile, false ); 
                var attribute = attributeGetter(cluster);
                if (attribute > max)
                    max = attribute;
                if (attribute < min)
                    min = attribute;               
            }
            return (min, max);
            
        }


    }

    
}
