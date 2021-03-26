using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using ClusterFilter;
using ClusterCalculator;
using System.Threading;
using System.Globalization;
namespace ClusterDescriptionGen
{
    public partial class Form1 : Form
    {
        private const string configPath = "../../../config/calib_files_fe/";
        private IDescriptionWriter ClDescriptionWriter { get; set; }
        private EnergyCalculator[] EnergyCalculators { get; set; }
        private IClusterReader ClusterReader { get; } = new MMClusterReader();
        public Form1()
        {
            InitializeComponent();
            SelectedInputListView.View = View.Details;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
        }
        public void BrowseProcessButtonClicked(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                
                //InputTextbox.Text += string.Join(";",fileDialog.FileNames);
                    const string noClass = "<none>";
                    foreach (var fileName in fileDialog.FileNames)
                    {
                        var selectedFileClassPair = new ListViewItem(noClass);
                        selectedFileClassPair.SubItems.Add(fileName);
                        SelectedInputListView.Items.Add(selectedFileClassPair);
                    }
                    
            }
        }
         public void BrowseConfigButtonClicked(object sender, EventArgs e)
         {
             using (var dialog = new FolderBrowserDialog())
             {
                 if (dialog.ShowDialog() == DialogResult.OK)
                 {
                    for(int i = 0; i < SelectedInputListView.Items.Count; ++i)
                    {
                        if(SelectedInputListView.Items[i].SubItems.Count < 3)
                        {
                            //TODO check if folder contains a.txt ...
                            SelectedInputListView.Items[i].SubItems.Add(dialog.SelectedPath + '\\');
                            break;
                        }

                    }
                }
             }
         }
        struct ClusterCollectionAndEnum
        {
            public string Class { get; }
            public string ConfigPath { get; }
            public ClusterInfoCollection Collection { get; }
            public IEnumerator<ClusterInfo> Enumerator { get; }

            public ClusterCollectionAndEnum(ClusterInfoCollection collection, IEnumerator<ClusterInfo> enumerator, string clusterClass, string configPath)
            {
                Class = clusterClass;
                Collection = collection;
                Enumerator = enumerator;
                ConfigPath = configPath;
            }
        }
        public void ProcessButtonClicked(object sender, EventArgs e)
        {
            

            ClDescriptionWriter = new JSONDecriptionWriter(new StreamWriter(OutputTextbox.Text));

            string[] iniFiles = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[1].Text).ToArray();
            string[] classes = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToArray();
            string[] configDirs = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[2].Text).ToArray();
            //string[] pxFiles = new string[iniFiles.Length];
            List <ClusterCollectionAndEnum> clusterEnumCollections = new List<ClusterCollectionAndEnum>();

            NeighbourCountFilter neighbourCountFilter = new NeighbourCountFilter(nCount => nCount >= 3, NeighbourCountOption.WithYpsilonNeighbours);
            EnergyCalculators = new EnergyCalculator[configDirs.Length];
            EnergyCenterFinder[] centerFinders = new EnergyCenterFinder[configDirs.Length];
            BranchAnalyzer[] branchAnalyzers = new BranchAnalyzer[configDirs.Length];
            
            VertexFinder[] vertexFinders = new VertexFinder[configDirs.Length];
            ISkeletonizer[] skeletonizers = new ThinSkeletonizer[configDirs.Length];

            for (int i = 0; i < iniFiles.Length; i++ )
            {
                ClusterReader.GetTextFileNames(new StreamReader(iniFiles[i]), iniFiles[i], out string pxFile, out string clFile);
                var clCollection = new ClusterInfoCollection(new StreamReader(clFile), new StreamReader(pxFile));
                clusterEnumCollections.Add(new ClusterCollectionAndEnum(clCollection, clCollection.GetEnumerator(), classes[i], configDirs[i]));
                centerFinders[i] = new EnergyCenterFinder(new Calibration(clusterEnumCollections.Last().ConfigPath));
                branchAnalyzers[i] = new BranchAnalyzer(centerFinders[i]);
                EnergyCalculators[i] = new EnergyCalculator(new Calibration(clusterEnumCollections.Last().ConfigPath));
                vertexFinders[i] = new VertexFinder(new Calibration(clusterEnumCollections.Last().ConfigPath));
                skeletonizers[i] = new ThinSkeletonizer(EnergyCalculators[i]);
            }

            


            var attributePairs = new Dictionary<ClusterAttribute, object>();
            IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
            foreach (var checkedAttribute in AttributeCheckedList.CheckedItems)
            {
                var attributeName = ((string)checkedAttribute).ToAttribute();
                attributePairs.Add(attributeName, null);
                attributesToGet.Add(attributeName);
            }
            Cluster current;
            int clustersProcessedCount = 0; //remove
            int maxClusterCount = 1000000;
            Random random = new Random();

            bool done = false;
            while (clustersProcessedCount < maxClusterCount && clusterEnumCollections.Count > 0)
            {
                var currentIndex = random.Next(0, clusterEnumCollections.Count);
                var clusterEnumCollection = clusterEnumCollections[currentIndex];
                
                while (!clusterEnumCollection.Enumerator.MoveNext())
                {                  
                    clusterEnumCollections.Remove(clusterEnumCollection);
                    clusterEnumCollection.Enumerator.Dispose();
                    if (clusterEnumCollections.Count == 0)
                    {
                        done = true;
                        break;
                    }
                    currentIndex = random.Next(0, clusterEnumCollections.Count);
                    clusterEnumCollection = clusterEnumCollections[currentIndex];
                }
                if (done)
                    break;

                var centerFinder = centerFinders[currentIndex];
                var branchAnalyzer = branchAnalyzers[currentIndex];
                var EnergyCalculator = EnergyCalculators[currentIndex];
                var vertexFinder = vertexFinders[currentIndex];
                var skeletonizer = skeletonizers[currentIndex];


                var clInfo = clusterEnumCollection.Enumerator.Current;
            //foreach (var clInfo in clusterCollection)
            //{
                
                current = ClusterReader.LoadByClInfo(clusterEnumCollection.Collection.PxFile, clInfo);
                clustersProcessedCount++;
                ConvexHull hull = null;
                Cluster skeletonizedCluster = null;
                BranchedCluster branchedCluster = null;
                foreach (var attribute in attributesToGet)
                {

                    switch (attribute)
                    {
                        case ClusterAttribute.PixelCount:
                            attributePairs[attribute] = clInfo.PixCount;
                            break;
                        case ClusterAttribute.TotalEnergy:
                            attributePairs[attribute] = EnergyCalculator.CalcTotalEnergy(current.Points);
                            break;
                        case ClusterAttribute.AverageEnergy:
                            attributePairs[attribute] = EnergyCalculator.CalcTotalEnergy(current.Points) / (double)clInfo.PixCount;
                            break;
                        case ClusterAttribute.Width:
                        case ClusterAttribute.Convexity:
                            if (hull == null)
                            {
                                var newPoints = current.Points.ToHashSet();
                                hull = new ConvexHull(newPoints.ToList());
                            }
                            if (attribute == ClusterAttribute.Convexity)
                                attributePairs[attribute] = Math.Min(clInfo.PixCount / (double)hull.CalculateArea(), 1);
                            else
                                attributePairs[attribute] = hull.CalculateWidth();
                            break;
                        case ClusterAttribute.Branches:
                            if (skeletonizedCluster == null)
                            {
                                skeletonizedCluster = skeletonizer.SkeletonizeCluster(current);
                            }
                            if (branchedCluster == null)
                            {
                                branchedCluster = branchAnalyzer.Analyze(skeletonizedCluster, current);
                            }
                            attributePairs[attribute] = branchedCluster.ToDictionaries(EnergyCalculator);
                            break;
                        case ClusterAttribute.CrosspointCount:
                            if (skeletonizedCluster == null)
                            {
                                skeletonizedCluster = skeletonizer.SkeletonizeCluster(current);
                            }
                            var crossPoints = neighbourCountFilter.Process(skeletonizedCluster.Points);
                            attributePairs[attribute] = crossPoints.Count;
                            break;
                        case ClusterAttribute.RelativeHaloSize:
                            if (skeletonizedCluster == null)
                            {
                                skeletonizedCluster = skeletonizer.SkeletonizeCluster(current);
                            }
                            attributePairs[attribute] = skeletonizedCluster.Points.Length / (double)current.Points.Length;
                            break;
                        case ClusterAttribute.VertexCount:
                            if (skeletonizedCluster == null)
                            {
                                skeletonizedCluster = skeletonizer.SkeletonizeCluster(current);
                            }
                            attributePairs[attribute] = vertexFinder.FindVertices(skeletonizedCluster.Points).Count;
                            break;
                        case ClusterAttribute.BranchCount:
                            if (skeletonizedCluster == null)
                            {
                                skeletonizedCluster = skeletonizer.SkeletonizeCluster(current);
                            }
                            if (branchedCluster == null)
                            {
                                branchedCluster = branchAnalyzer.Analyze(skeletonizedCluster, current);
                            }
                            int branchesCount = 0;
                            foreach (var branch in branchedCluster.MainBranches)
                                branchesCount += (branch.GetTotalSubBranchCount() + 1);
                            attributePairs[attribute] = branchesCount;
                            break;
                        case ClusterAttribute.MaxEnergy:
                            double maxEnergy = current.Points.Max(point => EnergyCalculator.ToElectronVolts(point.ToT, point.xCoord, point.yCoord));
                            attributePairs[attribute] = maxEnergy;
                            break;
                        case ClusterAttribute.Class:
                            attributePairs[attribute] = clusterEnumCollection.Class;
                            break;
                        default: break;

                    }
                 }
                    ClDescriptionWriter.WriteDescription(attributePairs);
                    //if (clustersProcessedCount > 1000) break;
                //}
            }
            ClDescriptionWriter.Close();
        }

    }
    

}
