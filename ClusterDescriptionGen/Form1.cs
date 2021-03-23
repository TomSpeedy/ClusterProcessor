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

namespace ClusterDescriptionGen
{
    public partial class Form1 : Form
    {
        private const string configPath = "../../../config/";
        private IDescriptionWriter ClDescriptionWriter { get; set; }
        private EnergyCalculator EnergyCalculator { get; set; }
        private IClusterReader ClusterReader { get; } = new MMClusterReader();
        public Form1()
        {
            InitializeComponent();
            SelectedInputListView.View = View.Details;
        }
        private void AddItemToListView()
        {
            // Add a new item to the ListView, with an empty label
            // (you can set any default properties that you want to here)
            ListViewItem selectedItem = SelectedInputListView.SelectedItems[0];
            //selectedItem.SubItems[1].

                        
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
        /* public void BrowseConfigButtonClicked(object sender, EventArgs e)
         {
             using (var dialog = new FolderBrowserDialog())
             {
                 if (dialog.ShowDialog() == DialogResult.OK)
                 {
                     ConfigDirTextBox.Text = dialog.SelectedPath;
                 }
             }
         }*/
        struct ClusterCollectionAndEnum
        {
            public string Class { get; }
            public ClusterInfoCollection Collection { get; }
            public IEnumerator<ClusterInfo> Enumerator { get; }
            public ClusterCollectionAndEnum(ClusterInfoCollection collection, IEnumerator<ClusterInfo> enumerator, string clusterClass)
            {
                Class = clusterClass;
                Collection = collection;
                Enumerator = enumerator;
            }
        }
        public void ProcessButtonClicked(object sender, EventArgs e)
        {
            

            ClDescriptionWriter = new JSONDecriptionWriter(new StreamWriter(OutputTextbox.Text));
            EnergyCalculator = new EnergyCalculator(new Calibration(configPath));

            string[] iniFiles = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[1].Text).ToArray();
            string[] classes = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToArray();
            //string[] pxFiles = new string[iniFiles.Length];
            List <ClusterCollectionAndEnum> clusterEnumCollections = new List<ClusterCollectionAndEnum>();
            for (int i = 0; i < iniFiles.Length; i++ )
            {
                ClusterReader.GetTextFileNames(new StreamReader(iniFiles[i]), iniFiles[i], out string pxFile, out string clFile);
                var clCollection = new ClusterInfoCollection(new StreamReader(clFile), new StreamReader(pxFile));
                clusterEnumCollections.Add(new ClusterCollectionAndEnum(clCollection, clCollection.GetEnumerator(), classes[i]));
            }

            EnergyCenterFinder centerFinder = new EnergyCenterFinder(new Calibration(configPath));
            BranchAnalyzer branchAnalyzer = new BranchAnalyzer(centerFinder);
            NeighbourCountFilter neighbourCountFilter = new NeighbourCountFilter(nCount => nCount >= 3, NeighbourCountOption.WithYpsilonNeighbours);
            VertexFinder vertexFinder = new VertexFinder(new Calibration(configPath));
            ISkeletonizer skeletonizer = new ThinSkeletonizer(EnergyCalculator);
            var attributePairs = new Dictionary<ClusterAttribute, object>();
            IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
            foreach (var checkedAttribute in AttributeCheckedList.CheckedItems)
            {
                var attributeName = ((string)checkedAttribute).ToAttribute();
                attributePairs.Add(attributeName, null);
                attributesToGet.Add(attributeName);
            }
           // var clusterCollection = clusterCollections[0];
            Cluster current;
            //var pxFileReader = new StreamReader(pxFile);
            int clustersProcessedCount = 0; //remove
            int maxClusterCount = 10000;
            Random random = new Random();

            bool done = false;
            while (clustersProcessedCount < maxClusterCount && clusterEnumCollections.Count > 0)
            {
                var clusterEnumCollection = clusterEnumCollections[random.Next(0, clusterEnumCollections.Count - 1)];
                
                while (!clusterEnumCollection.Enumerator.MoveNext())
                {                  
                    clusterEnumCollections.Remove(clusterEnumCollection);
                    clusterEnumCollection.Enumerator.Dispose();
                    if (clusterEnumCollections.Count == 0)
                    {
                        done = true;
                        break;
                    }
                    clusterEnumCollection = clusterEnumCollections[random.Next(0, clusterEnumCollections.Count - 1)];
                }
                if (done)
                    break;
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
