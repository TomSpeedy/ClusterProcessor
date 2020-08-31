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
        PixelPoint[] Skeletonize();

    }
    class ThinSkeletonizer : ISkeletonizer
    {
        HashSet<PixelPoint> pointsHash { get; set; }

       // HashSet<PixelPoint> pointsHashMod { get; set; }

        List<PixelPoint> neighboursTemp { get; set; }

       

        readonly (int x,  int y)[] neighbourDiff = { (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1) };
        public ThinSkeletonizer(IList<PixelPoint> points)
        {
            pointsHash = new HashSet<PixelPoint>();
            neighboursTemp = new List<PixelPoint>();
            for (int i = 0; i < points.Count; i++)
            {
                pointsHash.Add(points[i]);
            }
        }
        public PixelPoint[] Skeletonize()
        {
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
                        neighboursTemp.Clear();

                }
                for (int i = 0; i < toDelete.Count; i++)
                {
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
                        neighboursTemp.Clear();

                }
                for (int i = 0; i < toDelete.Count; i++)
                {
                    pointsHash.Remove(toDelete[i]);
                }
                deletedCount += toDelete.Count;
                toDelete.Clear();
            }
            return pointsHash.ToArray();
        }
        private void GetNeighbours(PixelPoint point)
        {
           
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
