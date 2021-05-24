using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ClusterCalculator
{
    public interface IAttributeCalculator
    {
        void Calculate(ClusterClassCollection collection, IList<ClusterAttribute> attributesToGet, ref IClusterReader reader, ref Dictionary<ClusterAttribute, object> attributePair);
    }
    /// <summary>
    /// Class containing calculation of all features of type ClusterAttribute
    /// </summary>
    public class DefaultAttributeCalculator : IAttributeCalculator
    {
        EnergyCalculator EnergyCalculator { get; set; }
        ISkeletonizer Skeletonizer { get; set; }
        EnergyCenterFinder CenterFinder { get; set; }
        VertexFinder VertexFinder { get; set; }
        BranchAnalyzer BranchAnalyzer { get; set; }
        ClusterClassCollection ClassCollection { get; set; }
        const int minNeighborCount = 3;

        NeighbourCountFilter NeighbourCountFilter = new NeighbourCountFilter(nCount => nCount >= minNeighborCount, NeighbourCountOption.WithYpsilonNeighbours);
        /// <summary>
        /// Performs the calculation of all attributes
        /// </summary>
        /// <param name="attributePairs">result key value pairs</param>
        /// <param name="attributesToGet"> attributes to calculate (for iteration)</param>
        /// <param name="current">current instance of cluster </param>
        public void CalcAttributes(ref Dictionary<ClusterAttribute, object> attributePairs, IList<ClusterAttribute> attributesToGet, Cluster current)
        {
            ConvexHull hull = null;
            Cluster skeletonizedCluster = null;
            BranchedCluster branchedCluster = null;
            EnergyHaloFilter energyFilter = new EnergyHaloFilter(5);
            foreach (var attribute in attributesToGet)
            {
                //specific calculation of each attribute, stored in dictionary
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
                        attributePairs[attribute] = (current.Points.Length - skeletonizedCluster.Points.Length) / (double)current.Points.Length;
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
                            throw new InvalidOperationException("Cannot calculate the class attrribute directly via attribute calculator. Please Use Classifier to predict the class");
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
                    case ClusterAttribute.ClFile:                       
                        string absPath =  
                            ((FileStream)ClassCollection.Partitions[ClassCollection.PartitionIndex].Collection.ClFile.BaseStream)
                            .Name.Replace('\\', '/');
                        Uri absPathUri = new Uri(absPath);
                        Uri relativeUri = new Uri(Directory.GetCurrentDirectory().Replace('\\', '/') + "/file").MakeRelativeUri(absPathUri);
                        attributePairs[attribute] = "\"" +  relativeUri.ToString() + "\"";
                        break;
                    case ClusterAttribute.PxFile:
                        absPath =
                            ((FileStream)ClassCollection.Partitions[ClassCollection.PartitionIndex].Collection.PxFile.BaseStream)
                            .Name.Replace('\\','/');
                        absPathUri = new Uri(absPath);
                        relativeUri = new Uri(Directory.GetCurrentDirectory() + "\\file").MakeRelativeUri(absPathUri);
                        attributePairs[attribute] = "\"" + relativeUri.ToString() + "\"";
                        break;
                    case ClusterAttribute.ClIndex:
                        attributePairs[attribute] = ClassCollection.Partitions[ClassCollection.PartitionIndex].Collection.CurrentIndex;
                        break;
                    default: throw new ArgumentException("Invalid cluster attribute class - attribute calculation is not implemented");

                }
            }
        }
        /// <summary>
        /// first variant of using the calculator - via the descr generator
        /// </summary>
        /// <param name="collection">data with information about currently processed class</param>
        /// <param name="attributesToGet"> attributes to calculate</param>
        /// <param name="reader">object for reading clusters from the collection</param>
        /// <param name="attributePairs">key value attribute pairs</param>
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
        /// <summary>
        /// second variant of the API for calculation, used when the cluster object is already created
        /// </summary>
        /// <param name="current"> cluster to process </param>
        /// <param name="attributesToGet"> attributes for calculation </param>
        /// <param name="attributePairs"> ke value pairs of attributes</param>
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
   

}
