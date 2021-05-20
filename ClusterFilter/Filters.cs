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
        protected StreamReader PixelFile { get; set; }
        public int ProcessedCount { get; protected set; } = 0;
        public int FilterSuccessCount { get; protected set; } = 0;
        public abstract bool MatchesFilter(ClusterInfo clInfo);
        public List<PixelPoint> GetPixels(ClusterInfo clInfo)
        {
            PixelFile.DiscardBufferedData();
            PixelFile.BaseStream.Seek((long)clInfo.ByteStart, SeekOrigin.Begin);
            List<PixelPoint> points = new List<PixelPoint>();
            for (int i = 0; i < clInfo.PixCount; i++)
            {
                var tokens = PixelFile.ReadLine().Split();
                if (tokens[0] == "#")
                    tokens = PixelFile.ReadLine().Split();


                if (tokens.Length == 5)
                {

                    ushort.TryParse(tokens[0], out ushort x);
                    ushort.TryParse(tokens[1], out ushort y);
                    double.TryParse(tokens[2], out double ToA);
                    double.TryParse(tokens[3], out double Energy);

                    if (points.FindIndex(point => point.xCoord == x && point.yCoord == y) == -1)
                    {

                        points.Add(new PixelPoint(x, y, ToA, Energy));

                    }

                }
            }
            return points;

        }
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

        private EnergyCalculator EnergyCalculator { get; set; }

        private bool NeedsCalibration = false;
        private double LowerBound { get; }
        private double UpperBound { get; }
        public EnergyFilter(StreamReader pixelFile, Calibration calib, double lowerBound = 0D, double upperBound = double.MaxValue)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            this.EnergyCalculator = new EnergyCalculator(calib);
            NeedsCalibration = true;
        }
        public EnergyFilter(StreamReader pixelFile, double lowerBound = 0D, double upperBound = double.MaxValue)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            this.EnergyCalculator = new EnergyCalculator();

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
                    double.TryParse(tokens[2], out double ToA);
                    double.TryParse(tokens[3], out double ToT);
                    double Energy = 0;
                    if ((x <= 255) && (x >= 0) && (y >= 0) && (y <= 255))
                        Energy = NeedsCalibration ? EnergyCalculator.ToElectronVolts(ToT, x, y) : ToT;
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
        private double LowerBound { get; }
        private double UpperBound { get; }
        private EnergyCalculator EnergyCalculator { get; }
        bool UseSkelet { get; }
        bool NeedsCalibration = false;

        public ConvexityFilter(StreamReader pixelFile, Calibration calib, int lowerBound, int upperBound, bool useSkelet = false)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            EnergyCalculator = new EnergyCalculator(calib);
            UseSkelet = useSkelet;
        }
        public ConvexityFilter(StreamReader pixelFile, int lowerBound, int upperBound, bool useSkelet = false)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            EnergyCalculator = new EnergyCalculator();
            UseSkelet = useSkelet;
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
                double.TryParse(tokens[2], out double ToA);
                double.TryParse(tokens[3], out double ToT);
                if (points.FindIndex(point => point.xCoord == x && point.yCoord == y) == -1)
                {

                    points.Add(new PixelPoint(x, y, ToA, NeedsCalibration ? EnergyCalculator.ToElectronVolts(ToT, x, y) : ToT));

                }
            }
            double area = 0;
            if (points.Count <= 1)
                area = points.Count;
            else
            {
                if (UseSkelet)
                {
                    ISkeletonizer skeletonizer = new ThinSkeletonizer();
                    var hull = new ConvexHull(skeletonizer.SkeletonizePoints(points));
                    area = hull.CalculateArea();
                }
                else
                {
                    var hull = new ConvexHull(points);
                    area = hull.CalculateArea();
                }
            }

            double convexity = clInfo.PixCount / (double)area;
            return convexity >= LowerBound && convexity <= UpperBound;

        }


    }
    public class SuccessFilter : ClusterFilter
    {
        public override bool MatchesFilter(ClusterInfo clInfo) => true;
    }
    public class VertexCountFilter : ClusterFilter
    {
        private VertexFinder VertexFinder { get; }
        private int MinVertexCount { get; }
        private int MaxVertexCount { get; }
        public VertexCountFilter(StreamReader pixelFile, int minVertexCount, int maxVertexCount)
        {
            VertexFinder = new VertexFinder();
            PixelFile = pixelFile;
            MinVertexCount = minVertexCount;
            MaxVertexCount = maxVertexCount;
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
                double.TryParse(tokens[2], out double ToA);
                double.TryParse(tokens[3], out double ToT);
                points.Add(new PixelPoint(x, y, ToA, ToT));
            }
            var vertices = VertexFinder.FindVertices(points);
            return vertices.Count >= MinVertexCount && vertices.Count <= MaxVertexCount;
        }
    }
    public class WidthFilter : ClusterFilter
    {
        private double LowerBound { get; }
        private double UpperrBound { get; }
        public WidthFilter(StreamReader pixelFile, double minWidth, double maxWidth)
        {
            PixelFile = pixelFile;
            LowerBound = minWidth;
            UpperrBound = maxWidth;
        }
        public override bool MatchesFilter(ClusterInfo clusterInfo)
        {
            var points = GetPixels(clusterInfo);
            var hull = new ConvexHull(points);
            var width = hull.CalculateWidth();
            return width >= LowerBound && width <= UpperrBound;
        }
    }
    public class BranchCountFilter : ClusterFilter
    {
        private double LowerBound { get; }
        private double UpperrBound { get; }
        private ISkeletonizer Skeletonizer { get; } = new ThinSkeletonizer();
        private BranchAnalyzer BranchAnalyzer { get; }
        public BranchCountFilter(StreamReader pixelFile, double minBranchCount, double maxBranchCount)
        {
            PixelFile = pixelFile;
            LowerBound = minBranchCount;
            UpperrBound = maxBranchCount;
            var centerFinder = new EnergyCenterFinder();
            BranchAnalyzer = new BranchAnalyzer(centerFinder);
        }
        public override bool MatchesFilter(ClusterInfo clusterInfo)
        {
            var points = GetPixels(clusterInfo).ToArray();
            var baseCluster = new Cluster(clusterInfo.FirstToA, clusterInfo.PixCount, clusterInfo.ByteStart);
            baseCluster.Points = points;
            var skeletonizedPoints = Skeletonizer.SkeletonizePoints(points);
            var skeletCluster = new Cluster(clusterInfo.FirstToA, clusterInfo.PixCount, clusterInfo.ByteStart);
            skeletCluster.Points = skeletonizedPoints;
            var branchedCluster = BranchAnalyzer.Analyze(baseCluster, skeletCluster);

            int branchesCount = 0;
            foreach (var branch in branchedCluster.MainBranches)
                branchesCount += (branch.GetTotalSubBranchCount() + 1);
            return branchesCount >= LowerBound && branchesCount <= UpperrBound;
        }
    }
}
