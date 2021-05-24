using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    /// <summary>
    /// abstract filter, handling the iteration over pixels
    /// </summary>
    public abstract class PixelFilter 
    {
        public int ProcessedCount { get; protected set; } = 0;
        public int FilterSuccessCount { get; protected set; } = 0;
        public abstract bool MatchesFilter(PixelPoint pixel);
        protected HashSet<PixelPoint> HashedPoints { get; private set; }
        public List<PixelPoint> Process(IList<PixelPoint> pixels)
        {
            HashedPoints = pixels.ToHashSet();
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
    /// <summary>
    /// Checks if pixel has given energy
    /// </summary>
    public class EnergyHaloFilter : PixelFilter
    {
        double HaloLimit { get; }
        public EnergyHaloFilter(double haloLimit = 12)
        {
            HaloLimit = haloLimit;
        }
        public override bool MatchesFilter(PixelPoint pixel)
        {
            return pixel.Energy >= HaloLimit;
        }
    }
    public enum NeighbourCountOption
    {
        Base,
        WithYpsilonNeighbours,
        WithAllDiagonalNeighbours
    }
    /// <summary>
    /// class contains data for checkiong and handling Y types of neighbours
    /// </summary>
    class YpsilonConfig
    {
        public int YpsilonVersionCount { get; } = 8;
        private readonly (int x, int y)[] NeighboursVert = { (-1, -1), (1, -1), (0, 1) };
        private readonly (int x, int y)[] NeighboursHoriz = { (1, -1), (1, 1), (-1, 0) };
        private readonly (int x, int y)[] NeighboursDiaNW = { (0, -1), (1, 0), (-1, 1) };
        private readonly (int x, int y)[] NeighboursDiaNE = { (-1, 0), (0, -1), (1, 1) };
        private readonly (int x, int y) ForbiddenVert = (0, -1);
        private readonly (int x, int y) ForbiddenHoriz = (1, 0);
        private readonly (int x, int y) ForbiddenDiaNW = (1, -1);
        private readonly (int x, int y) ForbiddenDiaNE = (-1, -1);

        private (int x, int y)[] GetInverse((int x, int y)[] neighboursDiff)
        {
            var inverseNeighDiff = new (int, int)[neighboursDiff.Length];
            for (int i = 0; i < inverseNeighDiff.Length; i++)
            {
                inverseNeighDiff[i] = GetInverse(neighboursDiff[i]);
            }
            return inverseNeighDiff;
        }
        private (int x, int y) GetInverse((int x, int y) neighbour)
        {
            return (-neighbour.x, -neighbour.y);
        }
        public ((int x, int y)[], (int forbidX, int forbidY)) GetNeighboursDiff(int diffVersion)
        {
            switch (diffVersion)
            {
                case 0:
                    return (NeighboursVert, ForbiddenVert);
                case 1:
                    return (GetInverse(NeighboursVert), GetInverse(ForbiddenVert));
                case 2:
                    return (NeighboursHoriz, ForbiddenHoriz);
                case 3:
                    return (GetInverse(NeighboursHoriz), GetInverse(ForbiddenHoriz));
                case 4:
                    return (NeighboursDiaNW, ForbiddenDiaNW);
                case 5:
                    return (GetInverse(NeighboursDiaNW), GetInverse(ForbiddenDiaNW));
                case 6:
                    return (NeighboursDiaNE, ForbiddenDiaNE);
                default:
                    return (GetInverse(NeighboursDiaNE), GetInverse(ForbiddenDiaNE));
            }
        }
    }
    /// <summary>
    /// filters pixels based on the neighor count based on the neighbor type
    /// </summary>
    public class NeighbourCountFilter : PixelFilter
    {
        readonly (int x, int y)[] neighbourDiagDiff = { (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1) };
        readonly (int x, int y)[] neighbourNonDiagDiff = { (0, -1), (1, 0), (0, 1), (-1, 0) };
        private NeighbourCountOption CountOption { get; }
        private YpsilonConfig YpsilonConfig { get; }
        Predicate<int> Condition { get; }
        public NeighbourCountFilter(Predicate<int> condition, NeighbourCountOption countOption)
        {
            Condition = condition;
            CountOption = countOption;
            YpsilonConfig = new YpsilonConfig();

        }

        public override bool MatchesFilter(PixelPoint point)
        {
            if (CountOption == NeighbourCountOption.WithYpsilonNeighbours)
                return Condition(GetNeighbourCount(point)) || IsInYpsilon(point);
            else
                return Condition(GetNeighbourCount(point));
        }
        private int GetNeighbourCount(PixelPoint point)
        {
            var neighbourCount = 0;
            var neighbourDiff = neighbourDiagDiff;
            if (CountOption != NeighbourCountOption.WithAllDiagonalNeighbours)
                neighbourDiff = neighbourNonDiagDiff;
            for (int i = 0; i < neighbourDiff.Length; i++)
            {
                var neighbour = new PixelPoint((ushort)(point.xCoord + neighbourDiff[i].x), (ushort)(point.yCoord + neighbourDiff[i].y));
                if (HashedPoints.Contains(neighbour))
                    neighbourCount++;
            }
            return neighbourCount;
        }
        private bool IsInYpsilon(PixelPoint point)
        {

            for (int i = 0; i < YpsilonConfig.YpsilonVersionCount; i++)
            {
                bool isInYpsilon = true;
                var (neighbours, forbidden) = YpsilonConfig.GetNeighboursDiff(i);
                for (int j = 0; j < neighbours.Length; j++)
                {
                    if (!HashedPoints.Contains(new PixelPoint((ushort)(point.xCoord + neighbours[j].x), (ushort)(point.yCoord + neighbours[j].y))))
                        isInYpsilon = false;
                }
                if (HashedPoints.Contains(new PixelPoint((ushort)(point.xCoord + forbidden.forbidX), (ushort)(point.yCoord + forbidden.forbidY))))
                    isInYpsilon = false;
                if (isInYpsilon)
                    return true;

            }
            return false;
        }

    }
}
