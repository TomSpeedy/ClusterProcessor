using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Filters;
using System.Diagnostics;

namespace Filters
{
    interface IClusterFilter
    {
        bool MatchesFilter(double FirstToA, uint pixelCount, long? lineOfStart = null, ulong? byteOfStart = null );
        void Process(BinaryReader inputFile, BinaryWriter outputFile);

    }
    
    public class EnergyFilter : IClusterFilter
    {
        private BinaryReader PixelReader { get; set; }
        BinaryReader pixelFile;
        BinaryReader aFile;
        BinaryReader bFile;
        BinaryReader cFile;
        BinaryReader tFile;
        private double LowerBound { get; }
        private double UpperBound { get; }
        public EnergyFilter(BinaryReader pixelFile, BinaryReader aFile, BinaryReader bFile, BinaryReader cFile, BinaryReader tFile, double lowerBound = 0D, double upperBound = double.MaxValue)
        {
            this.PixelReader = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }/*
        private double ToElectronVolts(double ToT, ushort x, ushort y)
        {

            double a = aFile.Seek(); //upravit podla fileov
            double b = bFile.Seek();
            double c = cFile.Seek();
            double t = tFile.Seek();
            double D = Math.Pow((-a * t - ToT - b), 2) - 4 * a * (-b * t - c + ToT * t);
            double energy1 = (a * t + ToT + b + Math.Sqrt(D)) / (2 * a);
        }*/
        public bool MatchesFilter(double FirstToA, uint pixelCount, long? lineOfStart = null, ulong? byteOfStart = null)
        {
            PixelReader.BaseStream.Seek((long)byteOfStart, SeekOrigin.Current);
            double totalEnergy = 0D;
            for (int i = 0; i < pixelCount; i++)
            {
                ushort x = PixelReader.ReadUInt16();
                ushort y = PixelReader.ReadUInt16();
                double ToA = PixelReader.ReadDouble();
                double ToT = PixelReader.ReadDouble();
                //double Energy = ToElectronVolts(ToT, x, y);
                totalEnergy += Energy;
            }
            if ((totalEnergy >= LowerBound) && (totalEnergy <= UpperBound))
            {
                return true;
            }
            return false;

        }
        public void Process(BinaryReader inputCLFile, BinaryWriter outputCLFile)
        {
            while (inputCLFile.BaseStream.Position <= inputCLFile.BaseStream.Length)
            {
                byte Zerobyte = inputCLFile.ReadByte();
                double FirstToA = inputCLFile.ReadDouble();
                uint pixCount = inputCLFile.ReadUInt32();
                ulong byteStart = inputCLFile.ReadUInt64();
                if (MatchesFilter(FirstToA, pixCount, null, byteStart))
                {
                    outputCLFile.Write((byte)0);
                    outputCLFile.Write(FirstToA);
                    outputCLFile.Write(pixCount);
                    outputCLFile.Write(byteStart);

                }
            }
        }

    }
    public class PixelCountFilter : IClusterFilter
    {
        private FileStream PixelFile { get; }
        private double LowerBound { get; }
        private double UpperBound { get; }

        public PixelCountFilter(FileStream pixelFile, int lowerBound, int upperBound)
        {
            this.PixelFile = pixelFile;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }
        public bool MatchesFilter(double FirstToA, uint pixelCount, long? lineOfStart = null, ulong? byteOfStart = null)
        {
            if ((pixelCount >= this.LowerBound) && (pixelCount <= this.UpperBound))
            {
                return true;
            }
            return false;
        }
        public void Process(BinaryReader inputCLFile, BinaryWriter outputCLFile)
        {
            while (inputCLFile.BaseStream.Position <= inputCLFile.BaseStream.Length)
            {
                byte Zerobyte = inputCLFile.ReadByte();
                double FirstToA = inputCLFile.ReadDouble();
                uint pixCount = inputCLFile.ReadUInt32();
                ulong byteStart = inputCLFile.ReadUInt64();
                if (MatchesFilter(FirstToA, pixCount, null, byteStart))
                {
                    outputCLFile.Write((byte)0);
                    outputCLFile.Write(FirstToA);
                    outputCLFile.Write(pixCount);
                    outputCLFile.Write(byteStart);

                }
            }
        }



    }
    
}
