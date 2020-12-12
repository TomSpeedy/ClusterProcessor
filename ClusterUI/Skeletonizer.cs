using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace ClusterUI
{
    interface ISkeletonizer
    {
        PixelPoint[] Skeletonize(IList<PixelPoint> points);

    }
    class ThinSkeletonizer : ISkeletonizer
    {
        HashSet<PixelPoint> pointsHash { get; set; }
        List<PixelPoint> neighboursTemp { get; set; }
        //EnergyCalculator EnergyCalculator { get; }

        readonly (int x,  int y)[] neighbourDiff = { (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1) };

        public PixelPoint[] Skeletonize(IList<PixelPoint> points)
        {
            //prepare collections of points
            pointsHash = points.ToHashSet();
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
                            && (!NeighbourExists(current, 2) || !NeighbourExists(current, 4) || (!NeighbourExists(current, 0) && !NeighbourExists(current, 6))))
                        {
                            toDelete.Add(current);
                        }
                  
                }
                for (int i = 0; i < toDelete.Count; i++)
                {
                    // TODO correction needed, split ToA of deleted pixel among its neighbours

                    pointsHash.TryGetValue(toDelete[i], out PixelPoint actualVal);
                    GetNeighbours(toDelete[i]);
                    for (int j = 0; j < neighboursTemp.Count; j++)
                    {
                        pointsHash.TryGetValue(neighboursTemp[j], out PixelPoint actualNeighbour);
                        actualNeighbour.SetToT(actualNeighbour.ToT + (actualVal.ToT / neighboursTemp.Count));
                    }
                    //----

                    pointsHash.Remove(toDelete[i]);
                }
                deletedCount += toDelete.Count;
                toDelete.Clear();

                //second subiteration
                foreach (var current in pointsHash) 
                {
                        GetNeighbours(current); //returns value to neighboursTemp (to reuse the same list)
                        if (neighboursTemp.Count >= 2 && neighboursTemp.Count <= 6 && (GetZeroOneCount(current) == 1)
                            && (!NeighbourExists(current, 0) || !NeighbourExists(current, 6) || (!NeighbourExists(current, 2) && !NeighbourExists(current, 4))))

                        {
                            toDelete.Add(current);
                        }                       

                }
                for (int i = 0; i < toDelete.Count; i++)
                {
                    // TODO correction needed

                    pointsHash.TryGetValue(toDelete[i], out PixelPoint actualVal);
                    GetNeighbours(toDelete[i]);
                    for (int j = 0; j < neighboursTemp.Count; j++)
                    {
                        pointsHash.TryGetValue(neighboursTemp[j], out PixelPoint actualNeighbour);
                        actualNeighbour.SetToT(actualNeighbour.ToT + (actualVal.ToT / neighboursTemp.Count));
                    }
                    //----
                    pointsHash.Remove(toDelete[i]);
                }
                deletedCount += toDelete.Count;
                toDelete.Clear();
            }
            return pointsHash.ToArray();
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
