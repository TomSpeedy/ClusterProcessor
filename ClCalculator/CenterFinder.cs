using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{
   /// <summary>
   /// Class for finding the center of a cluster based on the energy of the pixels
   /// </summary>
    public class EnergyCenterFinder
    {
        private NeighbourCountFilter NeighbourFilter;
        private IZCalculator ZCalculator;
        const int MinNeighbourCount = 3;
        const int Epsilon = 3;
        const double skeletWeight = 1.5;
        const double noSkeletWeight = 1;
        const double coreWeight = 3;
        public EnergyCenterFinder() 
        {
            ZCalculator = new ZCalculator();
            NeighbourFilter = new NeighbourCountFilter(neighbourCount => neighbourCount >= MinNeighbourCount, NeighbourCountOption.WithYpsilonNeighbours); //we only count non diagonals neighbours
        }
        private double CalcSurroundEnergy(PixelPoint point, HashSet<PixelPoint> allPoints, HashSet<PixelPoint> skeletPoints)
        {
            double surroundEnergy = 0;
            for (int i = point.xCoord - Epsilon; i <= point.xCoord + Epsilon; ++i)
            {
                for (int j = point.yCoord - Epsilon; j <= point.yCoord + Epsilon; ++j)
                {                 
                    var neighbour = new PixelPoint((ushort)i, (ushort)j);
                    var weight = skeletPoints.Contains(neighbour) ? skeletWeight : noSkeletWeight;
                    if (i < ushort.MaxValue && j < ushort.MaxValue && j >= 0 && i >= 0 && allPoints.TryGetValue(neighbour, out PixelPoint actualNeighbour))
                    {
                        surroundEnergy += weight* actualNeighbour.Energy;
                    }
                    
                }
            }
            surroundEnergy += (coreWeight - skeletWeight) * point.Energy;
            return surroundEnergy;
        }
        /// <summary>
        ///  calculates the center For the given cluster 
        /// </summary>
        /// <param name="skeletCluster"> already skeletonized cluster</param>
        /// <param name="allPoints"> points of the former cluster</param>
        /// <returns></returns>
        public PixelPoint CalcCenterPoint(Cluster skeletCluster, IList<PixelPoint> allPoints)
        {
            var hashedPoints = allPoints.ToHashSetPixPoints();
            var hashedSkeletPoints = skeletCluster.Points.ToHashSetPixPoints();
            double maxEnergy = 0;
            var skeletPoints = skeletCluster.Points.ToList();
            PixelPoint maxEnergyPoint = new PixelPoint();
            List<PixelPoint> possibleMidPoints = NeighbourFilter.Process(skeletPoints);
            if (possibleMidPoints.Count == 0)
                possibleMidPoints = skeletPoints;
            //AddPointsByZCoord(possibleMidPoints, skeletCluster);
            possibleMidPoints.Sort((left, right) =>
            {
                var leftE = CalcSurroundEnergy(left, hashedPoints, hashedSkeletPoints);
                var rightE = CalcSurroundEnergy(right, hashedPoints, hashedSkeletPoints);
                if (leftE < rightE)
                    return 1;
                if (leftE > rightE)
                    return -1;
                return 0;
            });
            
            for (int i = 0; i < possibleMidPoints.Count; i++) {
                var currentPointSurrEnergy = CalcSurroundEnergy(possibleMidPoints[i], hashedPoints, hashedSkeletPoints);
                if (currentPointSurrEnergy > maxEnergy)
                {
                    maxEnergy = currentPointSurrEnergy;
                    maxEnergyPoint = possibleMidPoints[i];
                }
            }
            return maxEnergyPoint;
        }
        private void AddPointsByZCoord(IList<PixelPoint> possibleMidPoints, Cluster skeletCluster)
        {
            //hash set used to quickly determine if point is already considered to be possible mid points
            var hashMidPoints = possibleMidPoints.ToHashSet();
            var skeletPoints = skeletCluster.Points.ToList();
            var firstToA = skeletPoints.Min(point => point.ToA);
            skeletPoints.Sort((left, right) =>
            {
                if (ZCalculator.CalculateZ(left, firstToA) < ZCalculator.CalculateZ(right, firstToA))
                    return 1;
                if (ZCalculator.CalculateZ(left, firstToA) > ZCalculator.CalculateZ(right, firstToA))
                    return -1;
                return 0;
            });
            //get bottom 10% of pixels by Z coord
            const double percentageTaken = 5;
            for (int i = 0; i < skeletPoints.Count * percentageTaken / 100d ; ++i)
            {
                if (!hashMidPoints.Contains(skeletPoints[i]))
                {
                    possibleMidPoints.Add(skeletPoints[i]);
                    hashMidPoints.Add(skeletPoints[i]);
                }
            }
            //get top 10% of pixels by Z coord
            for (int i = skeletPoints.Count - 1; i > (100-percentageTaken)* skeletPoints.Count / 100d; --i)
            {
                if (!hashMidPoints.Contains(skeletPoints[i]))
                {
                    possibleMidPoints.Add(skeletPoints[i]);
                    hashMidPoints.Add(skeletPoints[i]);
                }
            }
        }
    }
    public static class HashSetExtensions
    {
        public static HashSet<PixelPoint> ToHashSetPixPoints(this IEnumerable<PixelPoint> points)
        {
            var hashSet = new HashSet<PixelPoint>();
            foreach (var point in points)
            {
                if (!hashSet.Contains(point))
                    hashSet.Add(point);
                else
                {
                    hashSet.TryGetValue(point, out PixelPoint oldPoint);
                    if (oldPoint.Energy < point.Energy)
                    {
                        hashSet.Remove(oldPoint);
                        hashSet.Add(point);
                    }
                }
            }
            return hashSet;
        }
    }

}
