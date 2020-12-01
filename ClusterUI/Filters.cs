using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ClusterUI
{
    public static class Extensions
    {
        public static double Sqr(this double value) => value * value;
        public static int Sqr(this int value) => value * value;

    }

    public abstract class PixelFilter //works with loaded data, suitable only for filtering currently viewed files
    {
        public int ProcessedCount { get; protected set; } = 0;
        public int FilterSuccessCount { get; protected set; } = 0;
        public abstract bool MatchesFilter(PixelPoint pixel);
        public List<PixelPoint> Process(IList<PixelPoint> pixels)
        {
            List<PixelPoint> result = new List<PixelPoint>();
            foreach (PixelPoint pixel in pixels)
            {
                if (MatchesFilter(pixel))
                {
                    result.Add(pixel);
                    FilterSuccessCount++;
                }
                ProcessedCount++;
                    
            }
            return result;
        }
    }

    public class EnergyHaloFilter : PixelFilter
    {
        readonly EnergyCalculator EnergyCalculator;
        const double HaloLimit = 8;
        public EnergyHaloFilter(StreamReader aFile, StreamReader bFile, StreamReader cFile, StreamReader tFile)
        {
            EnergyCalculator = new EnergyCalculator(aFile, bFile, cFile, tFile);

        }
        public override bool MatchesFilter(PixelPoint pixel)
        {
            return (EnergyCalculator.ToElectronVolts(pixel.ToT, pixel.xCoord, pixel.yCoord) >= HaloLimit);
        }
    }

    public abstract class ClusterFilter //works with text files
    {
        public int ProcessedCount { get; protected set; } = 0;
        public int FilterSuccessCount { get; protected set; } = 0;
        public abstract bool MatchesFilter(ClusterInfo clInfo);
        public void Process(StreamReader inputCLFile, StreamWriter outputCLFile)
        {
            IClusterWriter clusterWriter = new MMClusterWriter(outputCLFile);
            foreach (var clInfo in new ClusterInfoCollection(inputCLFile))
            { 
                if (MatchesFilter(clInfo))
                {
                    FilterSuccessCount++;
                    clusterWriter.WriteClusterInfo(clInfo);

                }
                ProcessedCount++;
            }
        }

    }
    public class MultiFilter : ClusterFilter
    {
        public IList<ClusterFilter> Filters { get; }
        public MultiFilter(IList<ClusterFilter> filters)
        {
            this.Filters = filters;
        }
        public override bool MatchesFilter(ClusterInfo clInfo)
        {
            return Filters.All(filter => filter.MatchesFilter(clInfo));
        }
    }
    public class EnergyFilter : ClusterFilter
    {
        
        private StreamReader PixelFile { get; set; }
        private EnergyCalculator EnergyCalculator { get; set; }
        

        private double LowerBound { get; }
        private double UpperBound { get; }
        public EnergyFilter(StreamReader pixelFile, StreamReader aFile, StreamReader bFile, StreamReader cFile, StreamReader tFile, double lowerBound = 0D, double upperBound = double.MaxValue)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            this.EnergyCalculator = new EnergyCalculator(aFile, bFile, cFile, tFile);

        }
        public override bool MatchesFilter(ClusterInfo clInfo)
        {
            PixelFile.BaseStream.Seek((long)clInfo.ByteStart, SeekOrigin.Begin);
            double totalEnergy = 0D;
            for (int i = 0; i < clInfo.PixCount; i++)
            {
                var tokens = PixelFile.ReadLine().Split();
                if (tokens[0] == "#")
                    tokens = PixelFile.ReadLine().Split();


                if (tokens.Length == 5)
                {
                    
                    ushort.TryParse(tokens[0], out ushort x);
                    ushort.TryParse(tokens[1], out ushort y);
                    double.TryParse(tokens[2].Replace('.', ','), out double ToA);
                    double.TryParse(tokens[3].Replace('.', ','), out double ToT);
                    double Energy = 0;
                    if ((x <= 255) && (x >= 0) && (y >= 0) &&(y <= 255))
                        Energy = EnergyCalculator.ToElectronVolts(ToT, x, y);
                    totalEnergy += Energy;
                   
                }
            }
            if ((totalEnergy >= LowerBound) && (totalEnergy <= UpperBound))
            {
                return true;
            }
            return false;

        }
    }
    public class PixelCountFilter : ClusterFilter
    {
        private StreamReader PixelFile { get; }
        private double LowerBound { get; }
        private double UpperBound { get; }

        public PixelCountFilter(StreamReader pixelFile, int lowerBound, int upperBound)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }
        public override bool MatchesFilter(ClusterInfo clInfo)
        {
            
            if ((clInfo.PixCount >= this.LowerBound) && (clInfo.PixCount <= this.UpperBound))
            {
                return true;
            }
            return false;
        }
        



    }
    public class ConvexityFilter:ClusterFilter
    {
        private StreamReader PixelFile { get; }
        private double LowerBound { get; }
        private double UpperBound { get; }
        bool useSkelet { get; }

        public ConvexityFilter(StreamReader pixelFile, int lowerBound, int upperBound, bool useSkelet = false)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }
       
        public override bool MatchesFilter(ClusterInfo clInfo)
        {
            PixelFile.DiscardBufferedData();
            PixelFile.BaseStream.Seek((long)clInfo.ByteStart, SeekOrigin.Begin);
            var points = new List<PixelPoint>();
            for (int i = 0; i < clInfo.PixCount; i++)
            {
                var tokens = PixelFile.ReadLine().Split();
                if (tokens[0] == "#")
                {
                    if (i == 0)
                        tokens = PixelFile.ReadLine().Split();
                    else
                        throw new InvalidOperationException();

                }
                    
                ushort.TryParse(tokens[0], out ushort x);
                ushort.TryParse(tokens[1], out ushort y);
                double.TryParse(tokens[2].Replace('.', ','), out double ToA);
                double.TryParse(tokens[3].Replace('.', ','), out double ToT);
                if (points.FindIndex(point => point.xCoord == x && point.yCoord == y) == -1) //we didnt read the same pixel twice, If we did, then just ignore it
                {

                    points.Add(new PixelPoint(x, y, ToA, ToT));

                }
            }
            double area = 0;
            if (points.Count <= 1)
                area = points.Count;
            else
            {
                if(useSkelet)
                {
                    ISkeletonizer skeletonizer = new ThinSkeletonizer(points);
                    var hull = new ConvexHull(skeletonizer.Skeletonize());
                    area = hull.CalculateArea();
                }
                else
                {
                    var hull = new ConvexHull(points);
                    area = hull.CalculateArea();
                }
                //CalculateWidth(hull);
            }
            
            double convexityPercentage = 100 * clInfo.PixCount / (double)area;
            if (convexityPercentage >= LowerBound && convexityPercentage <= UpperBound)
                return true;
            return false;
        }
        private double CalculateWidth(ConvexHull hull)
        {
            double max = 0;
            for (int i = 0; i < hull.HullPoints.Count; i++)
            {
                for(int j = i + 1; j < hull.HullPoints.Count; j++)
                {
                    var dist = GetDistance(hull.HullPoints[i], hull.HullPoints[j]);
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
            

    }
    public class SuccessFilter : ClusterFilter
    {
        public override bool MatchesFilter(ClusterInfo clInfo) => true;
    }
    public class ConvexHull
    {
        public PixelPoint MinPoint { get; }
        public List<PixelPoint> HullPoints = new List<PixelPoint>();
        public double CalculateArea()
        {
            if (HullPoints.Count <= 2)
                return HullPoints.Count;
            double area = 0;
            int j = HullPoints.Count - 1;
            for (int i = 0; i < HullPoints.Count; i++)
            {
                area += (HullPoints[j].xCoord + HullPoints[i].xCoord) * (HullPoints[j].yCoord - HullPoints[i].yCoord);
                j = i;
            }
            return Math.Abs(area / 2);
        }
        public ConvexHull(IList <PixelPoint> Points)
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
                if (HullPoints.Count == 1) { }
                CheckHull(sortedPoints[i]);
            }

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
                HullPoints.RemoveAt(HullPoints.Count - 1);
                if (HullPoints.Count > 1)
                    dir = GetTurn(current);
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
