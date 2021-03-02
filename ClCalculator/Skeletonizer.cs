using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ClusterCalculator
{
    public interface ISkeletonizer
    {
        PixelPoint[] SkeletonizePoints(IList<PixelPoint> points);
        Cluster SkeletonizeCluster(Cluster cluster);

    }
    public class ThinSkeletonizer : ISkeletonizer
    {
        HashSet<PixelPoint> pointsHash { get; set; }
        List<PixelPoint> neighboursTemp { get; set; }
        EnergyCalculator EnergyCalculator { get; }
        EnergyHaloFilter HaloFilter { get; }

        readonly (int x,  int y)[] neighbourDiff = { (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1) };
        public ThinSkeletonizer(EnergyCalculator energyCalc)
        {
            EnergyCalculator = energyCalc;
            HaloFilter = new EnergyHaloFilter(EnergyCalculator);
        }
        private PixelPoint[] SkeletonizeSelected(IList<PixelPoint> points, Predicate<PixelPoint> condition, bool preserveEnergy = true)
        {
            //prepare collections of points
            pointsHash = points.ToHashSetPixPoints();
            neighboursTemp = new List<PixelPoint>();


            var toDelete = new List<PixelPoint>();
            bool isFirstIteration = true;
            int deletedCount = 0;
            while ((deletedCount > 0) || isFirstIteration)
            {
                isFirstIteration = false;
                deletedCount = 0;
                //first subiteration
                foreach (var current in pointsHash)
                {

                    GetNeighbours(current); //returns value to neighboursTemp (to reuse the same list)
                    if (neighboursTemp.Count >= 2 && neighboursTemp.Count <= 6 && (GetZeroOneCount(current) == 1)
                        && (!NeighbourExists(current, 2) || !NeighbourExists(current, 4) || (!NeighbourExists(current, 0) && !NeighbourExists(current, 6))) && condition(current))
                    {
                        toDelete.Add(current);
                    }

                }
                for (int i = 0; i < toDelete.Count; i++)
                {

                    pointsHash.TryGetValue(toDelete[i], out PixelPoint actualVal);
                    GetNeighbours(toDelete[i]);
                    if (preserveEnergy)
                    {
                        var currentEnergy = EnergyCalculator.ToElectronVolts(actualVal.ToT, actualVal.xCoord, actualVal.yCoord);
                    for (int j = 0; j < neighboursTemp.Count; j++)
                    {
                        pointsHash.TryGetValue(neighboursTemp[j], out PixelPoint actualNeighbour);
                        var neighbourEnergy = EnergyCalculator.ToElectronVolts(actualNeighbour.ToT, actualNeighbour.xCoord, actualNeighbour.yCoord);
                        actualNeighbour.SetToT(EnergyCalculator.ToTimeOverThreshold(neighbourEnergy
                            + (currentEnergy / neighboursTemp.Count), actualNeighbour.xCoord, actualNeighbour.yCoord));
                    }
                    }

                    pointsHash.Remove(toDelete[i]);
                }
                deletedCount += toDelete.Count;
                toDelete.Clear();

                //second subiteration
                foreach (var current in pointsHash)
                {
                    GetNeighbours(current); //returns value to neighboursTemp (to reuse the same list)
                    if (neighboursTemp.Count >= 2 && neighboursTemp.Count <= 6 && (GetZeroOneCount(current) == 1)
                        && (!NeighbourExists(current, 0) || !NeighbourExists(current, 6) || (!NeighbourExists(current, 2) && !NeighbourExists(current, 4))) && condition(current))

                    {
                        toDelete.Add(current);
                    }

                }
                for (int i = 0; i < toDelete.Count; i++)
                {

                    pointsHash.TryGetValue(toDelete[i], out PixelPoint actualVal);
                    GetNeighbours(toDelete[i]);
                    if (preserveEnergy)
                    {
                        var currentEnergy = EnergyCalculator.ToElectronVolts(actualVal.ToT, actualVal.xCoord, actualVal.yCoord);
                    for (int j = 0; j < neighboursTemp.Count; j++)
                    {
                        
                            pointsHash.TryGetValue(neighboursTemp[j], out PixelPoint actualNeighbour);
                        var neighbourEnergy = EnergyCalculator.ToElectronVolts(actualNeighbour.ToT, actualNeighbour.xCoord, actualNeighbour.yCoord);
                        actualNeighbour.SetToT(EnergyCalculator.ToTimeOverThreshold(neighbourEnergy
                            + (currentEnergy / neighboursTemp.Count), actualNeighbour.xCoord, actualNeighbour.yCoord));
                        
                    }
                    }
                    //----
                    pointsHash.Remove(toDelete[i]);
                }
                deletedCount += toDelete.Count;
                toDelete.Clear();
            }
            return pointsHash.ToArray();
        }
        public PixelPoint[] SkeletonizePoints(IList<PixelPoint> points)
        {
            var removedHalo = SkeletonizeSelected(points, point => !HaloFilter.MatchesFilter(point), preserveEnergy:true);
            var allSkeletonized = SkeletonizeSelected(removedHalo, condition: point => true, preserveEnergy:true);
            return allSkeletonized;
        }
        public Cluster SkeletonizeCluster(Cluster cluster)
        {
            var skeletPoints = SkeletonizePoints(cluster.Points);
            var skeletCluster = new Cluster(cluster.FirstToA, cluster.PixelCount, cluster.ByteStart);
            skeletCluster.Points = skeletPoints;
            return skeletCluster;
        }
        private void GetNeighbours(PixelPoint point)
        {
            neighboursTemp.Clear();
            for (int i = 0; i < neighbourDiff.Length; i++)
            {
                var neighbour = new PixelPoint((ushort)(point.xCoord + neighbourDiff[i].x), (ushort)(point.yCoord + neighbourDiff[i].y));
                if (pointsHash.Contains(neighbour))
                    neighboursTemp.Add(neighbour);
            }
        }
        private int GetZeroOneCount(PixelPoint point)
        {
            var zeroOneCount = 0;
            for (int i = 0; i < neighbourDiff.Length; i++)
            {
                if (!NeighbourExists(point, i) && NeighbourExists(point, (i + 1) % neighbourDiff.Length))
                    zeroOneCount++;
                if (zeroOneCount > 1)
                    break; //we dont need exact zero-one count, done to speed up the calculation

            }
            return zeroOneCount;
        }
        private bool NeighbourExists(PixelPoint point, int neighIndex)
        {
            return neighboursTemp.Contains(new PixelPoint((ushort)(point.xCoord + neighbourDiff[neighIndex].x), (ushort)(point.yCoord + neighbourDiff[neighIndex].y)));
        }

    }
}
