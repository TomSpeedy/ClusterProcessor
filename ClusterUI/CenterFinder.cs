using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterUI
{
    class EnergyCenterFinder
    {
        private EnergyCalculator EnergyCalc;
        const int Epsilon = 3;
        public EnergyCenterFinder(StreamReader aFile, StreamReader bFile, StreamReader cFile, StreamReader tFile) 
        {
            EnergyCalc = new EnergyCalculator(aFile, bFile, cFile, tFile);
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
            for (int i = 0; i < points.Count; i++) {
                var currentPointSurrEnergy = CalcSurroundEnergy(points[i], hashedPoints);
                if (currentPointSurrEnergy > maxEnergy)
                {
                    maxEnergy = currentPointSurrEnergy;
                    maxEnergyPoint = points[i];
                }
            }
            return maxEnergyPoint;
        }
    }
}
