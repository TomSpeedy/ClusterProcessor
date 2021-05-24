using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    /// <summary>
    /// serves as a conversion between ToA and energy and for energy based calculations
    /// </summary>
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
        public EnergyCalculator()
        {
            
        }
        public double ToElectronVolts(double ToT, ushort x, ushort y)
        {

            double a = aConf[x][y];
            double b = bConf[x][y];
            double c = cConf[x][y];
            double t = tConf[x][y];
            const double defaultEnergy = 3.00;
            double D = Math.Pow((-a * t - ToT + b), 2) - 4 * a * (-b * t - c + ToT * t);
            double energy = (a * t + ToT + b + Math.Sqrt(D)) / (2 * a);
            if(double.IsNaN(energy))
            {
                energy = defaultEnergy;
            }
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
        public double CalcTotalEnergy(IEnumerable<PixelPoint> points)
        {
            var totalEnergy = 0d;
            foreach (var point in points)
            {
                totalEnergy += point.Energy;
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
                configArray[0][j] = double.Parse(stringValues[j]);
            }
            configArray[0][255] = configArray[0][254];
            for (int i = 1; i < configArray.Length; i++)
            {
                stringValues = configFile.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                configArray[i] = new double[256];
                for (int j = 0; j < configArray[i].Length; j++)
                {
                    configArray[i][j] = double.Parse(stringValues[j]);
                }

            }
        }
        public void CalibrateCollection(string iniFile, string newIniFile)
        {
            IClusterReader clusterReader = new MMClusterReader();
            clusterReader.GetTextFileNames(new StreamReader(iniFile), iniFile, out string pxFile, out string clFile);
            ClusterInfoCollection collection = new ClusterInfoCollection(new StreamReader(clFile), new StreamReader(pxFile));
            StreamReader PixelFile = new StreamReader(pxFile);
            StreamWriter newIniStream = new StreamWriter(newIniFile);
            string newPxFile = newIniFile + "_px_calibrated.txt";
            string newClFile = newIniFile + "_cl_calibrated.txt";
            newIniStream.WriteLine("[Measurement]");
            newIniStream.WriteLine($"PxFile={PathParser.GetSuffixPath(newPxFile)}");
            newIniStream.WriteLine($"ClFile={PathParser.GetSuffixPath(newClFile)}");
            newIniStream.Close();
            long bytesWritten = 0;
            StreamWriter newPxStream = new StreamWriter(newPxFile);
            StreamWriter newClStream = new StreamWriter(newClFile);
            foreach (var clInfo in collection)
            {
                PixelFile.DiscardBufferedData();

                PixelFile.BaseStream.Seek((long)clInfo.ByteStart, SeekOrigin.Begin);
                var points = new List<PixelPoint>();
                newClStream.WriteLine($"{clInfo.FirstToA} {clInfo.PixCount} {clInfo.LineStart} {bytesWritten}");
                for (int i = 0; i < clInfo.PixCount; i++)
                {
                    var tokens = PixelFile.ReadLine().Split();
                    if (tokens[0] == "#")
                    {
                        if (i == 0)
                        {
                            tokens = PixelFile.ReadLine().Split();
                        }
                        else
                            throw new InvalidOperationException();

                    }

                    ushort.TryParse(tokens[0], out ushort x);
                    ushort.TryParse(tokens[1], out ushort y);
                    double.TryParse(tokens[2], out double ToA);
                    double.TryParse(tokens[3], out double ToT);
                    string outLine = $"{x} {y} {ToA} {ToElectronVolts(ToT, x, y)}\n";
                    newPxStream.Write(outLine);
                    bytesWritten += System.Text.UTF8Encoding.ASCII.GetByteCount(outLine);
                    
                }
                newPxStream.Write("#\n");
                bytesWritten += System.Text.UTF8Encoding.ASCII.GetByteCount("#\n");
            }
            newPxStream.Close();
            newIniStream.Close();
            newClStream.Close();

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
