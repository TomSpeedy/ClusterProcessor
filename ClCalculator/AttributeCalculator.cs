using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    public interface IAttributeCalculator
    {
        void Calculate(ClusterClassCollection collection, IList<ClusterAttribute> attributesToGet, ref IClusterReader reader, ref Dictionary<ClusterAttribute, object> attributePair);
    }
    public class DefaultAttributeCalculator : IAttributeCalculator
    {
        NeighbourCountFilter NeighbourCountFilter = new NeighbourCountFilter(nCount => nCount >= 3, NeighbourCountOption.WithYpsilonNeighbours);
        public void Calculate(ClusterClassCollection collection, IList<ClusterAttribute> attributesToGet, ref IClusterReader reader, ref Dictionary<ClusterAttribute, object> attributePairs)
        {
            var partition = collection.Partitions[collection.PartitionIndex];
            var centerFinder = partition.CenterFinder;
            var branchAnalyzer = partition.BranchAnalyzer;
            var EnergyCalculator = partition.EnergyCalc;
            var vertexFinder = partition.VertexFinder;
            var skeletonizer = partition.Skeletonizer;

            var clInfo = partition.Enumerator.Current;

            var current = reader.LoadByClInfo(partition.Collection.PxFile, clInfo);
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
                        var crossPoints = NeighbourCountFilter.Process(skeletonizedCluster.Points);
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
                        double maxEnergy = current.Points.Max(point => point.Energy);
                        attributePairs[attribute] = maxEnergy;
                        break;
                    case ClusterAttribute.Class:
                        attributePairs[attribute] = collection.Class;
                        break;
                    default: break;

                }
            }
        }

    }
    public struct ClusterClassPartition
    {
        public string ConfigPath { get; }
        public ClusterInfoCollection Collection { get; }
        public IEnumerator<ClusterInfo> Enumerator { get; }
        public EnergyCenterFinder CenterFinder { get; }
        public EnergyCalculator EnergyCalc { get; }
        public BranchAnalyzer BranchAnalyzer { get; }
        public VertexFinder VertexFinder { get; }
        public ISkeletonizer Skeletonizer { get; }
        public ClusterClassPartition(ClusterInfoCollection collection, string configPath)
        {
            ConfigPath = configPath;
            Collection = collection;
            Enumerator = collection.GetEnumerator();
            CenterFinder = new EnergyCenterFinder();
            BranchAnalyzer = new BranchAnalyzer(CenterFinder);
            EnergyCalc = new EnergyCalculator();
            VertexFinder = new VertexFinder();
            Skeletonizer = new ThinSkeletonizer();
        }
    }
    public struct ClusterClassCollection
    {
        public string Class;
        public List<ClusterClassPartition> Partitions;
        public int PartitionIndex { get; private set; }
        public IEnumerator<ClusterInfo> CurrentEnumerator { get; set; }

        public ClusterClassCollection(ClusterClassPartition startPartition, string clusterClass)
        {
            Class = clusterClass;
            Partitions = new List<ClusterClassPartition>();
            Partitions.Add(startPartition);
            PartitionIndex = 0;
            CurrentEnumerator = Partitions[PartitionIndex].Enumerator;
        }
        public void SetNewCurrentEnumerator()
        {
            if (Partitions.Count > 1)
            {
                PartitionIndex = (PartitionIndex + 1) % Partitions.Count;
                CurrentEnumerator = Partitions[PartitionIndex].Enumerator;
            }
        }
        public bool RemovePartition()
        {
            if (Partitions.Count > 1)
            {
                Partitions[PartitionIndex].Enumerator.Dispose();
                Partitions.RemoveAt(PartitionIndex);
                SetNewCurrentEnumerator();
                return true;
            }
            if (Partitions.Count == 1)
            {
                Partitions[0].Enumerator.Dispose();
                Partitions.RemoveAt(0);
            }
            return false;
        }
    }

}
