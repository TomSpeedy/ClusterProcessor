using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
        EnergyCalculator EnergyCalculator { get; set; }
        ISkeletonizer Skeletonizer { get; set; }
        EnergyCenterFinder CenterFinder { get; set; }
        VertexFinder VertexFinder { get; set; }
        BranchAnalyzer BranchAnalyzer { get; set; }
        ClusterClassCollection ClassCollection { get; set; }

        NeighbourCountFilter NeighbourCountFilter = new NeighbourCountFilter(nCount => nCount >= 3, NeighbourCountOption.WithYpsilonNeighbours);
        private void CalcAttributes(ref Dictionary<ClusterAttribute, object> attributePairs, IList<ClusterAttribute> attributesToGet, Cluster current)
        {


            //var current = reader.LoadByClInfo(partition.Collection.PxFile, clInfo);
            ConvexHull hull = null;
            Cluster skeletonizedCluster = null;
            BranchedCluster branchedCluster = null;
            EnergyHaloFilter energyFilter = new EnergyHaloFilter(5);
            foreach (var attribute in attributesToGet)
            {

                switch (attribute)
                {
                    case ClusterAttribute.PixelCount:
                        attributePairs[attribute] = current.PixelCount;
                        break;
                    case ClusterAttribute.TotalEnergy:
                        attributePairs[attribute] = EnergyCalculator.CalcTotalEnergy(current.Points);
                        break;
                    case ClusterAttribute.AverageEnergy:
                        attributePairs[attribute] = EnergyCalculator.CalcTotalEnergy(current.Points) / (double)current.PixelCount;
                        break;
                    case ClusterAttribute.Width:
                    case ClusterAttribute.Convexity:
                        if (hull == null)
                        {
                            var newPoints = current.Points.ToHashSet();
                            hull = new ConvexHull(newPoints.ToList());
                        }
                        if (attribute == ClusterAttribute.Convexity)
                            attributePairs[attribute] = Math.Min(current.PixelCount / (double)hull.CalculateArea(), 1);
                        else
                            attributePairs[attribute] = hull.CalculateWidth();
                        break;
                    case ClusterAttribute.Branches:
                        if (skeletonizedCluster == null)
                        {
                            skeletonizedCluster = Skeletonizer.SkeletonizeCluster(current);
                        }
                        if (branchedCluster == null)
                        {
                            branchedCluster = BranchAnalyzer.Analyze(skeletonizedCluster, current);
                        }
                        attributePairs[attribute] = branchedCluster.ToDictionaries(EnergyCalculator);
                        break;
                    case ClusterAttribute.CrosspointCount:
                        if (skeletonizedCluster == null)
                        {
                            skeletonizedCluster = Skeletonizer.SkeletonizeCluster(current);
                        }
                        var crossPoints = NeighbourCountFilter.Process(skeletonizedCluster.Points);
                        attributePairs[attribute] = crossPoints.Count;
                        break;
                    case ClusterAttribute.RelativeHaloSize:
                        if (skeletonizedCluster == null)
                        {
                            skeletonizedCluster = Skeletonizer.SkeletonizeCluster(current);
                        }
                        attributePairs[attribute] = skeletonizedCluster.Points.Length / (double)current.Points.Length;
                        break;
                    case ClusterAttribute.VertexCount:
                        if (skeletonizedCluster == null)
                        {
                            skeletonizedCluster = Skeletonizer.SkeletonizeCluster(current);
                        }
                        attributePairs[attribute] = VertexFinder.FindVertices(skeletonizedCluster.Points).Count;
                        break;
                    case ClusterAttribute.BranchCount:
                        if (skeletonizedCluster == null)
                        {
                            skeletonizedCluster = Skeletonizer.SkeletonizeCluster(current);
                        }
                        if (branchedCluster == null)
                        {
                            branchedCluster = BranchAnalyzer.Analyze(skeletonizedCluster, current);
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
                        if (ClassCollection == null)
                            throw new InvalidOperationException("Cannot calculate the class attrribute directly. Please Use Classifier to get the class");
                        attributePairs[attribute] = ClassCollection.Class;
                        break;
                    case ClusterAttribute.StdOfEnergy:
                        double stdDevOfEnergy = 0;
                        if (current.Points.Length == 1)
                            stdDevOfEnergy = 0;
                        else
                        {
                            IEnumerable<double> pixelsEnergy = current.Points.Select(point => point.Energy);
                            double avgEnergy = pixelsEnergy.Average();
                            double sumEnergy = pixelsEnergy.Sum(energy => Math.Pow(energy - avgEnergy, 2));
                            stdDevOfEnergy = Math.Sqrt(sumEnergy / (current.PixelCount - 1));
                          
                        }
                        attributePairs[attribute] = stdDevOfEnergy;
                        break;
                    case ClusterAttribute.StdOfArrival:
                        double stdDevOfArrival = 0;
                        if (current.Points.Length == 1)
                            stdDevOfArrival = 0;
                        else
                        {
                            IEnumerable<double> pixelsTime = current.Points.Select(point => point.ToA);
                            double avgArrival = pixelsTime.Average();
                            double sumArrival = pixelsTime.Sum(energy => Math.Pow(energy - avgArrival, 2));
                            stdDevOfArrival = Math.Sqrt(sumArrival / (current.PixelCount - 1));
                        }
                        attributePairs[attribute] = stdDevOfArrival;
                        break;
                    case ClusterAttribute.RelLowEnergyPixels:
                        attributePairs[attribute] = (current.Points.Length - energyFilter.Process(current.Points).Count) / (double)current.Points.Length;
                        break;
                    default: throw new ArgumentException("Invalid cluster attribute class - attribute calculation is not implemented");

                }
            }
        }
        public void Calculate(ClusterClassCollection collection, IList<ClusterAttribute> attributesToGet, ref IClusterReader reader, ref Dictionary<ClusterAttribute, object> attributePairs)
        { 

            
            var partition = collection.Partitions[collection.PartitionIndex];
            ClassCollection = collection;
            CenterFinder = partition.CenterFinder;
            BranchAnalyzer = partition.BranchAnalyzer;
            EnergyCalculator = partition.EnergyCalc;
            VertexFinder = partition.VertexFinder;
            Skeletonizer = partition.Skeletonizer;

            var clInfo = partition.Enumerator.Current;

            var current = reader.LoadByClInfo(partition.Collection.PxFile, clInfo);

            CalcAttributes(ref attributePairs, attributesToGet, current);
            
        }
        public void Calculate(Cluster current,  IList<ClusterAttribute> attributesToGet, ref Dictionary<ClusterAttribute, object> attributePairs)
        {
            
            CenterFinder = new EnergyCenterFinder();
            BranchAnalyzer = new BranchAnalyzer(CenterFinder);
            EnergyCalculator = new EnergyCalculator();
            VertexFinder = new VertexFinder();
            Skeletonizer = new ThinSkeletonizer();
            CalcAttributes(ref attributePairs, attributesToGet, current);
        }

    }
    public class ClusterClassPartition
    {
        public string ConfigPath { get; }
        public ClusterInfoCollection Collection { get; }
        public IEnumerator<ClusterInfo> Enumerator { get; private set; }
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
        public void ResetEtor()
        {
            Enumerator = Collection.GetEnumerator();
        }
        public bool CheckPosition()
        {
            const double maxRead = 0.9;
            return Collection.ClFile.BaseStream.Position < Collection.ClFile.BaseStream.Length * maxRead;
        }
    }
    public class ClusterClassCollection
    {
        public string Class;
        public List<ClusterClassPartition> Partitions;
        public int PartitionIndex { get; private set; }
        private Random random { get; set; }
        public IEnumerator<ClusterInfo> CurrentEnumerator { get; set; }
        public long CalcLength() {return Partitions.Select(partition => partition.Collection.ClFile.BaseStream.Length).Sum();}
        

        public ClusterClassCollection(ClusterClassPartition startPartition, string clusterClass)
        {
            Class = clusterClass;
            Partitions = new List<ClusterClassPartition>();
            Partitions.Add(startPartition);
            PartitionIndex = 0;
            CurrentEnumerator = Partitions[PartitionIndex].Enumerator;
            random = new Random();
        }

        public void SetNewCurrentEnumerator(bool chooseRandomly = true)
        {
            if (Partitions.Count > 0)
            {
                if (chooseRandomly)
                    PartitionIndex = random.Next(0, Partitions.Count);
                else
                    PartitionIndex = (PartitionIndex + 1) % Partitions.Count;
                CurrentEnumerator = Partitions[PartitionIndex].Enumerator;               
            }
        }

        public bool RemovePartition(bool chooseRandomly = true)
        {
            if (Partitions.Count > 1)
            {
                Partitions[PartitionIndex].Enumerator.Dispose();
                Partitions.RemoveAt(PartitionIndex);
                SetNewCurrentEnumerator(chooseRandomly);
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
