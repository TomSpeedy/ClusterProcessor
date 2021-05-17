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
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing.Text;
using ClusterCalculator;
using System.Globalization;
namespace ClusterFilter

{
    public partial class FilterUI : Form
    {
        private IClusterReader ClusterReader { get; set; } = new MMClusterReader();
        public FilterUI()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
        }
        #region Event Handlers
        
       
       
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

        #endregion


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
            const string doneMessage = "Filtering successfully completed";
            try
            {
                const int minVertexCount = 3;

                var workingDirName = PathParser.GetPrefixPath(InFilePathBox.Text);
                ClusterReader.GetTextFileNames(new StreamReader(InFilePathBox.Text), InFilePathBox.Text, out string pxFile, out string clFile);
                var outClPath = clFile.Replace('.','_') + "_filtered_" + DateTime.Now.ToString().Replace('/', '-').Replace(':','-') + ".cl";
                var filteredOut = new StreamWriter(outClPath);
                CreateNewIniFile(new StreamReader(InFilePathBox.Text), new StreamWriter(workingDirName + OutFileNameIniBox.Text + ".ini"), clFile, PathParser.GetSuffixPath(outClPath));

                var pixelCountFilter = new PixelCountFilter(new StreamReader(pxFile),
                    int.TryParse(FromPixCountFilterBox.Text, out int resultLowerP) ? resultLowerP : 0,
                    int.TryParse(ToPixCountFilterBox.Text, out int resultUpperP) ? resultUpperP : 100000);

                var energyFilter = new EnergyFilter(new StreamReader(pxFile), 
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
                MessageBox.Show(doneMessage);
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
