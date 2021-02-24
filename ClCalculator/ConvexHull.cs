using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    public class ConvexHull
    {
        public PixelPoint MinPoint { get; }
        public List<PixelPoint> HullPoints = new List<PixelPoint>();
        public double CalculateArea()
        {
            if (HullPoints.Count == 1)
                return HullPoints.Count;
            if (HullPoints.Count == 2)
                return HullPoints[0].GetDistance(HullPoints[1]) + 1;
            double area = 0;
            int j = HullPoints.Count - 1;
            for (int i = 0; i < HullPoints.Count; i++)
            {
                area += (HullPoints[j].xCoord + HullPoints[i].xCoord) * (HullPoints[j].yCoord - HullPoints[i].yCoord);
                j = i;
            }
            return Math.Abs(area / 2);
        }
        public ConvexHull(IList<PixelPoint> Points)
        {

            if (Points.Count <= 3)
            {
                HullPoints = Points.ToList();
                return;
            }

            MinPoint = GetMinPoint(Points);
            var pixComp = new PixelPointComparer(MinPoint);
            var sortedPoints = Points.ToList();
            sortedPoints.Remove(MinPoint);
            sortedPoints.Sort(pixComp);
            sortedPoints.Add(MinPoint);
            pixComp.Compare(sortedPoints[0], sortedPoints[1]);
            HullPoints.Add(MinPoint);
            HullPoints.Add(sortedPoints[0]); 
            for (int i = 1; i < sortedPoints.Count; i++)
            {
                if ((HullPoints.Count <= 2) && (i == sortedPoints.Count - 1)) 
                {
                    //cluster is a line
                    if (HullPoints.Count == 2)
                        HullPoints.Remove(HullPoints[HullPoints.Count - 1]);
                    HullPoints.Add(sortedPoints.Find(distPoint =>
                        MinPoint.GetDistance(distPoint) == sortedPoints.Max(point => MinPoint.GetDistance(point))));
                    break;
                }
                CheckHull(sortedPoints[i]);
            }

        }
        public double CalculateWidth()
        {
            double max = 0;
            for (int i = 0; i < HullPoints.Count; i++)
            {
                for (int j = i + 1; j < HullPoints.Count; j++)
                {
                    var dist = GetDistance(HullPoints[i], HullPoints[j]);
                    if (dist > max)
                    {
                        max = dist;
                    }
                }
            }
            return max;
        }
        private double GetDistance(PixelPoint first, PixelPoint second)
        {
            return Math.Sqrt((first.xCoord - second.xCoord).Sqr() + (first.yCoord - second.yCoord).Sqr());
        }
        private enum Direction
        {
            left,
            right,
            straight
        }

        private void CheckHull(PixelPoint current)
        {
            Direction dir = GetTurn(current);
            while ((dir != Direction.left) && (HullPoints.Count > 1))
            {
                if (dir == Direction.straight && 
                    current.GetDistance(HullPoints[HullPoints.Count - 2]) < 
                    HullPoints[HullPoints.Count - 1].GetDistance(HullPoints[HullPoints.Count - 2]))
                    return;
                HullPoints.RemoveAt(HullPoints.Count - 1);
                if (HullPoints.Count > 1)
                    dir = GetTurn(current);
                else
                    break;
            }
            if ((current.xCoord != MinPoint.xCoord) || (current.yCoord != MinPoint.yCoord))
                HullPoints.Add(current);
        }
        private Direction GetTurn(PixelPoint current)
        {
            PixelPoint lastHullPoint = HullPoints[HullPoints.Count - 1];
            PixelPoint secLastHullPoint = HullPoints[HullPoints.Count - 2];
            int crossProduct = (lastHullPoint.xCoord - secLastHullPoint.xCoord) * (current.yCoord - secLastHullPoint.yCoord) -
                (lastHullPoint.yCoord - secLastHullPoint.yCoord) * (current.xCoord - secLastHullPoint.xCoord);
            if (crossProduct < 0)
                return Direction.left;
            else if
                (crossProduct == 0)
                return Direction.straight;
            else
                return Direction.right;

        }
        private PixelPoint GetMinPoint(IList<PixelPoint> points)
        {
            PixelPoint min = points[0];
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].yCoord < min.yCoord)
                {
                    min = points[i];
                }
                if ((points[i].yCoord == min.yCoord) && (points[i].xCoord <= min.xCoord))
                {
                    min = points[i];
                }
            }
            return min;
        }
    }
    class PixelPointComparer : IComparer<PixelPoint>
    {
        public PixelPointComparer(PixelPoint minPoint)
        {
            this.MinPoint = minPoint;
        }
        PixelPoint MinPoint { get; }
        public int Compare(PixelPoint pixelPoint1, PixelPoint pixelPoint2)
        {
            var diffSize1 = Math.Sqrt(((pixelPoint1.xCoord - MinPoint.xCoord) * (pixelPoint1.xCoord - MinPoint.xCoord))
                                    + ((pixelPoint1.yCoord - MinPoint.yCoord) * (pixelPoint1.yCoord - MinPoint.yCoord)));
            var diffSize2 = Math.Sqrt((pixelPoint2.xCoord - MinPoint.xCoord) * (pixelPoint2.xCoord - MinPoint.xCoord)
                                    + ((pixelPoint2.yCoord - MinPoint.yCoord) * (pixelPoint2.yCoord - MinPoint.yCoord)));
            double Angle1 = Math.Acos((pixelPoint1.xCoord - MinPoint.xCoord) / diffSize1);
            double Angle2 = Math.Acos((pixelPoint2.xCoord - MinPoint.xCoord) / diffSize2);
            //if (cosineOfAngle1)
            if (Angle1 > Angle2)
                return -1;
            else if (Angle1 == Angle2)
                return 0;
            else
                return 1;
        }
    }
}
