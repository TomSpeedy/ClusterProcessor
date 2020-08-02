using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClusterCore;
using System.IO;
using Filters;
using System.Diagnostics;

namespace Filters
{
    public abstract class ClusterFilter
    {
        public int ProcessedCount { get; protected set; } = 0;
        public int FilterSuccessCount { get; protected set; } = 0;
        public abstract bool MatchesFilter(double FirstToA, uint pixelCount, long? lineOfStart = null, ulong? byteOfStart = null );
        public void Process(StreamReader inputCLFile, StreamWriter outputCLFile)
        {
            while (inputCLFile.BaseStream.Position < inputCLFile.BaseStream.Length)
            {
                string[] tokens = inputCLFile.ReadLine().Split();
                double FirstToA = double.Parse(tokens[0].Replace('.', ','));
                uint pixCount = uint.Parse(tokens[1]);
                ulong lineOfStart = ulong.Parse(tokens[2]);
                ulong byteStart = ulong.Parse(tokens[3]);
                ProcessedCount++;
                if (MatchesFilter(FirstToA, pixCount, null, byteStart))
                {
                    FilterSuccessCount++;
                    outputCLFile.WriteLine($"{FirstToA} {pixCount} {lineOfStart} {byteStart}");

                }
            }
        }

    }
    
    public class EnergyFilter : ClusterFilter
    {

        private StreamReader PixelFile { get; set; }
        double[][] aConf = new double[256][];
        double[][] bConf = new double[256][];
        double[][] cConf = new double[256][];
        double[][] tConf = new double[256][];

        private double LowerBound { get; }
        private double UpperBound { get; }
        public EnergyFilter(StreamReader pixelFile, StreamReader aFile, StreamReader bFile, StreamReader cFile, StreamReader tFile, double lowerBound = 0D, double upperBound = double.MaxValue)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            LoadConfigFile(aFile, this.aConf);
            LoadConfigFile(bFile, this.bConf);
            LoadConfigFile(cFile, this.cConf);
            LoadConfigFile(tFile, this.tConf);
        }
        private void LoadConfigFile(StreamReader configFile, double[][] configArray)
        {
            char[] delimiters = { ' ', '\t' };
            string[] stringValues = configFile.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            configArray[0] = new double[256];
            for (int j = 0; j < configArray[0].Length - 1; j++)
            {
                configArray[0][j] = double.Parse(stringValues[j].Replace('.', ','));
            }
            configArray[0][255] = configArray[0][254];
            for (int i = 1; i < configArray.Length; i++)
            {
                stringValues = configFile.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                configArray[i] = new double[256];
                for (int j = 0; j < configArray[i].Length; j++)
                {
                    configArray[i][j] = double.Parse(stringValues[j].Replace('.',','));
                }

            }
        }
        public double ToElectronVolts(double ToT, ushort x, ushort y)
        {

            double a = aConf[x][y]; 
            double b = bConf[x][y];
            double c = cConf[x][y];
            double t = tConf[x][y];
            double D = Math.Pow((-a * t - ToT - b), 2) - 4 * a * (-b * t - c + ToT * t);
            double energy = (a * t + ToT + b + Math.Sqrt(D)) / (2 * a);
            return energy;
        }
        public override bool MatchesFilter(double FirstToA, uint pixelCount, long? lineOfStart = null, ulong? byteOfStart = null)
        {
            PixelFile.BaseStream.Seek((long)byteOfStart, SeekOrigin.Begin);
            double totalEnergy = 0D;
            for (int i = 0; i < pixelCount; i++)
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
                        Energy = ToElectronVolts(ToT, x, y);
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
        public override bool MatchesFilter(double FirstToA, uint pixelCount, long? lineOfStart = null, ulong? byteOfStart = null)
        {
            
            if ((pixelCount >= this.LowerBound) && (pixelCount <= this.UpperBound))
            {
                return true;
            }
            return false;
        }
        



    }
    public class LinearityFilter:ClusterFilter
    {
        private StreamReader PixelFile { get; }
        private double LowerBound { get; }
        private double UpperBound { get; }

        public LinearityFilter(StreamReader pixelFile, int lowerBound, int upperBound)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }
       
        public override bool MatchesFilter(double FirstToA, uint pixelCount, long? lineOfStart = null, ulong? byteOfStart = null)
        {
            PixelFile.BaseStream.Seek((long)byteOfStart, SeekOrigin.Begin);
            PixelPoint[] points = new PixelPoint[pixelCount];
            for (int i = 0; i < pixelCount; i++)
            {
                var tokens = PixelFile.ReadLine().Split();
                if (tokens[0] == "#")
                    tokens = PixelFile.ReadLine().Split();
                ushort.TryParse(tokens[0], out ushort x);
                ushort.TryParse(tokens[1], out ushort y);
                double.TryParse(tokens[2].Replace('.', ','), out double ToA);
                double.TryParse(tokens[3].Replace('.', ','), out double ToT);
                points[i] = new PixelPoint(x, y, ToA, ToT);
            }
            double area = 0;
            if (points.Length <= 1)
                area = points.Length;
            else
            {
                var hull = new ConvexHull(points);
                area = hull.CalculateArea();
            }
            double percentage = 100 * pixelCount / area;
            if (percentage >= LowerBound && percentage <= UpperBound)
                return true;
            return false;
        }

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
            for (int i = 0; i < HullPoints.Count; i++)
            {

                area += (HullPoints[i % HullPoints.Count].xCoord * HullPoints[(i + 1) % HullPoints.Count].yCoord -
                    HullPoints[i % HullPoints.Count].yCoord * HullPoints[(i + 1) % HullPoints.Count].xCoord);
            }
            return Math.Abs(area / 2);
        }
        public ConvexHull(PixelPoint[] Points)
        {

            if (Points.Length <= 3)
                HullPoints = Points.ToList();
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
        private PixelPoint GetMinPoint(PixelPoint[] points)
        {
            PixelPoint min = points[0];
            for (int i = 0; i < points.Length; i++)
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
