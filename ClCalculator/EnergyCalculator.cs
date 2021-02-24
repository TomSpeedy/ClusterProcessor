using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    public class EnergyCalculator
    {
        const int confSize = 256;
        double[][] aConf = new double[confSize][];
        double[][] bConf = new double[confSize][];
        double[][] cConf = new double[confSize][];
        double[][] tConf = new double[confSize][];
        public EnergyCalculator(Calibration calib)
        {
            LoadConfigFile(calib.AFile, this.aConf);
            LoadConfigFile(calib.BFile, this.bConf);
            LoadConfigFile(calib.CFile, this.cConf);
            LoadConfigFile(calib.TFile, this.tConf);
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

        public double ToTimeOverThreshold(double Energy, ushort x, ushort y)
        {
            double a = aConf[x][y];
            double b = bConf[x][y];
            double c = cConf[x][y];
            double t = tConf[x][y];
            return a * Energy + b - (c / Energy - t);
        }
        public double CalcTotalEnergy(IList<PixelPoint> points)
        {
            var totalEnergy = 0d;
            for (int i = 0; i < points.Count; ++i)
            {
                totalEnergy += ToElectronVolts(points[i].ToT, points[i].xCoord, points[i].yCoord);
            }
            return totalEnergy;
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
                    configArray[i][j] = double.Parse(stringValues[j].Replace('.', ','));
                }

            }
        }
    }
    public struct Calibration
    {
        public StreamReader AFile { get; }
        public StreamReader BFile { get; }
        public StreamReader CFile { get; }
        public StreamReader TFile { get; }
        public Calibration(string directory)
        {
            const string aFile = "a.txt";
            const string bFile = "b.txt";
            const string cFile = "c.txt";
            const string tFile = "t.txt";

            AFile = new StreamReader(directory + aFile);
            BFile = new StreamReader(directory + bFile);
            CFile = new StreamReader(directory + cFile);
            TFile = new StreamReader(directory + tFile);
        }
    }
}
