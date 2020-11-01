using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ClusterUI
{
    public class Cluster
    {

        public double FirstToA { get; private set; }
        public uint PixelCount { get; private set; }
        public ulong ByteStart { get; private set; }
        public PixelPoint[] Points { get; set; }
        public Cluster(double FirstToA, uint PixelCount, ulong ByteStart) {
            this.FirstToA = FirstToA;
            this.PixelCount = PixelCount;
            this.ByteStart = ByteStart;
        }

        public Color ToColor(double color)
        {
            if (double.IsNaN(color))
                return Color.FromArgb(255, 255, 255, 255);
            if (color < 0.20)
                return Color.FromArgb(255, 255, 255, (int)Math.Round((1 - 5*color) * 255) );
            return Color.FromArgb(255, 255, (int)Math.Round((1 - color) * 255), 0);
        }
               
    }
    public struct PixelPoint
    {

        public PixelPoint(ushort xCoord, ushort yCoord, double ToA = 0, double ToT = 0)
        {
            this.xCoord = xCoord;
            this.yCoord = yCoord;
            this.ToA = ToA;
            this.ToT = ToT;

        }
        public ushort xCoord { get; }
        public ushort yCoord { get; }
        public double ToA { get; }
        public double ToT { get; }
        public override int GetHashCode()
        {
            return (xCoord << 8) + yCoord;
        }
        public override bool Equals(object obj)
        {
            var other = (PixelPoint)obj;
            return this.xCoord == other.xCoord && this.yCoord == other.yCoord;
        }
    }
    public struct ClusterInfo
    { 
        public double FirstToA { get; set; }
        public uint PixCount { get; set; }
        public ulong ByteStart { get; set; }
        public ulong LineStart { get; set; }
        public override string ToString()
        {
            return $"{FirstToA} {PixCount} {LineStart} {ByteStart}";
        }
    }
    class ClusterInfoCollection : IEnumerable<ClusterInfo>
    {
        private readonly StreamReader ClFile; 
        public ClusterInfoCollection(StreamReader clFile)
        {
            this.ClFile = clFile;
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public IEnumerator<ClusterInfo> GetEnumerator()
        {
            while (ClFile.BaseStream.Position < ClFile.BaseStream.Length)
            {
                string[] tokens = ClFile.ReadLine().Split();
                var clInfo = new ClusterInfo()
                {
                    FirstToA = double.Parse(tokens[0].Replace('.', ',')),
                    PixCount = uint.Parse(tokens[1]),
                    LineStart = ulong.Parse(tokens[2]),
                    ByteStart = ulong.Parse(tokens[3])
                };
                yield return clInfo;
            }
        }

    }
}
