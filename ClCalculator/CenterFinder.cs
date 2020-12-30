using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    public class EnergyCenterFinder
    {
        private EnergyCalculator EnergyCalc;
        private NeighbourCountFilter NeighbourFilter;
        const int MinNeighbourCount = 3;
        const int Epsilon = 3;
        public EnergyCenterFinder(StreamReader aFile, StreamReader bFile, StreamReader cFile, StreamReader tFile) 
        {
            EnergyCalc = new EnergyCalculator(aFile, bFile, cFile, tFile);
            NeighbourFilter = new NeighbourCountFilter(neighbourCount => neighbourCount >= MinNeighbourCount, NeighbourCountOption.WithYpsilonNeighbours); //we only count non diagonals neighbours
        }
        private double CalcSurroundEnergy(PixelPoint point, HashSet<PixelPoint> points)
        {
            double surroundEnergy = 0;
            for (int i = point.xCoord - Epsilon; i < point.xCoord + Epsilon; ++i)
            {
                for (int j = point.yCoord - Epsilon; j < point.yCoord + Epsilon; ++j)
                {
                    var neighbour = new PixelPoint((ushort)i, (ushort)j);
                    if (i < ushort.MaxValue && j < ushort.MaxValue && points.TryGetValue(neighbour, out PixelPoint actualNeighbour))
                    {
                        surroundEnergy += EnergyCalc.ToElectronVolts(actualNeighbour.ToA, actualNeighbour.xCoord, actualNeighbour.yCoord);
                    }
                }
            }
            return surroundEnergy;
        }

        public PixelPoint CalcCenterPoint(IList<PixelPoint> points)
        {
            var hashedPoints = points.ToHashSet();
            double maxEnergy = 0;
            PixelPoint maxEnergyPoint = new PixelPoint();
            IList<PixelPoint> possibleMidPoints = NeighbourFilter.Process(points); //
            if (possibleMidPoints.Count == 0)
                possibleMidPoints = points;
            for (int i = 0; i < possibleMidPoints.Count; i++) {
                var currentPointSurrEnergy = CalcSurroundEnergy(possibleMidPoints[i], hashedPoints);
                if (currentPointSurrEnergy > maxEnergy)
                {
                    maxEnergy = currentPointSurrEnergy;
                    maxEnergyPoint = possibleMidPoints[i];
                }
            }
            return maxEnergyPoint;
        }
    }
    
}
