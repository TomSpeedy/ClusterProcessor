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
        }
        public void BrowseProcessButtonClicked(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                InputTextbox.Text = fileDialog.FileName;
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
        public void ProcessButtonClicked(object sender, EventArgs e)
        {
            

            ClDescriptionWriter = new JSONDecriptionWriter(new StreamWriter(OutputTextbox.Text));
            EnergyCalculator = new EnergyCalculator(new Calibration(configPath));
            ClusterReader.GetTextFileNames(new StreamReader(InputTextbox.Text), InputTextbox.Text, out string pxFile, out string clFile);
            var clusterCollection = new ClusterInfoCollection(new StreamReader(clFile));
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

            Cluster current;
            var pxFileReader = new StreamReader(pxFile);
            int index = 0; //remove
            foreach (var clInfo in clusterCollection)
            {
                current = ClusterReader.LoadByClInfo(pxFileReader, clInfo);
                index++;
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
                        default: break;

                    }
                }
                ClDescriptionWriter.WriteDescription(attributePairs);
                if (index > 50) break;
            
            }
            ClDescriptionWriter.Close();
        }

    }
    

}
