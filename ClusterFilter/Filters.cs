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
using ClusterCalculator;
namespace ClusterFilter
{
    


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
        public EnergyFilter(StreamReader pixelFile, Calibration calib, double lowerBound = 0D, double upperBound = double.MaxValue)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            this.EnergyCalculator = new EnergyCalculator(calib);

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
    public class ConvexityFilter : ClusterFilter
    {
        private StreamReader PixelFile { get; }
        private double LowerBound { get; }
        private double UpperBound { get; }
        private EnergyCalculator EnergyCalculator {get;}
        bool useSkelet { get; }

        public ConvexityFilter(StreamReader pixelFile, Calibration calib, int lowerBound, int upperBound, bool useSkelet = false)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            EnergyCalculator = new EnergyCalculator(calib);
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
                    ISkeletonizer skeletonizer = new ThinSkeletonizer(EnergyCalculator);
                    var hull = new ConvexHull(skeletonizer.SkeletonizePoints(points));
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
                 

    }
    public class SuccessFilter : ClusterFilter
    {
        public override bool MatchesFilter(ClusterInfo clInfo) => true;
    }
    public class VertexCountFilter : ClusterFilter
    {
        private VertexFinder VertexFinder { get; }
        private StreamReader PixelFile { get; set; }
        private int MinVertexCount { get; }
        public VertexCountFilter(StreamReader pixelFile, int minVertexCount, Calibration calib)
        {
            VertexFinder = new VertexFinder(calib);
            PixelFile = pixelFile;
            MinVertexCount = minVertexCount;
        }
        public override bool MatchesFilter(ClusterInfo clusterInfo)
        {
            PixelFile.DiscardBufferedData();
            PixelFile.BaseStream.Seek((long)clusterInfo.ByteStart, SeekOrigin.Begin);
            var points = new List<PixelPoint>();
            for (int i = 0; i < clusterInfo.PixCount; i++)
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
                points.Add(new PixelPoint(x, y, ToA, ToT));
            }
            return VertexFinder.FindVertices(points).Count >= MinVertexCount;
        }
    }
    
    

}
