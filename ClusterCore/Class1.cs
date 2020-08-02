using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ClusterCore
{
    public class Cluster
    {

        public double FirstToA { get; private set; }
        public uint PixelCount { get; private set; }
        public ulong ByteStart { get; private set; }
        public PixelPoint[] Points { get; set; }

        public static void GetTextFileNames(TextReader reader, string iniPath, out string pxFile, out string clFile)
        {
            var prefixPath = GetPrefixPath(iniPath);
            reader.ReadLine();
            string[] tokens1 = reader.ReadLine().Split('=');
            pxFile = prefixPath + tokens1[1];
            string[] tokens2 = reader.ReadLine().Split('=');
            clFile = prefixPath + tokens2[1];
        }
        public static string GetPrefixPath(string iniPath)
        {
            if (!iniPath.Contains('/') && !iniPath.Contains('\\'))
                return ""; //relative adress is used
            int lastIndex = 0;
            for (int i = 0; i < iniPath.Length; i++)
            {
                if (iniPath[i] == '/' || iniPath[i] == '\\')
                    lastIndex = i;
            }
            return iniPath.Substring(0, lastIndex + 1);
        }
        private static string getLine(int clusterNumber, StreamReader reader)
        {
            for (int i = 0; i < clusterNumber - 1; i++)
            {
                //if (reader.BaseStream.Position < reader.BaseStream.Length)
                    reader.ReadLine();
                //else return null;
            }
            //if (reader.BaseStream.Position < reader.BaseStream.Length)
                return reader.ReadLine();
            //else 
             // return null;

        }


        public Color ToColor(double color)
        {
            if (double.IsNaN(color))
                return Color.FromArgb(255, 255, 255, 255);
            return Color.FromArgb(255, 255, (int)Math.Round((1 - color) * 255), 0);
        }

        public static string[] GetMinCluster(int minimumPixelCount, StreamReader reader)
        {
            string[] tokens = reader.ReadLine().Split(' ');
            int i = 0;
            while (reader.Peek() != -1 && Int32.Parse(tokens[1]) < minimumPixelCount)
            {
                i++;
                tokens = reader.ReadLine().Split(' ');
            }
            return (tokens);

        }
        public static Cluster LoadFromText(StreamReader pixelReader, StreamReader clusterReader, bool isFullLoad, int clusterNumber = 1)
        {
            Cluster cluster = new Cluster();

            string[] clusterInfo = getLine(clusterNumber, clusterReader)?.Split(' ');
            if (clusterInfo == null)
                return null;
            cluster.FirstToA = double.Parse(clusterInfo[0].Replace('.', ','));
            cluster.PixelCount = uint.Parse(clusterInfo[1]);
            cluster.ByteStart = ulong.Parse(clusterInfo[3]);

            
                cluster.Points = new PixelPoint[cluster.PixelCount];
            if (isFullLoad)
            {
                pixelReader.DiscardBufferedData();
                pixelReader.BaseStream.Position = (long)cluster.ByteStart;

                for (int i = 0; i < cluster.PixelCount; i++)
                {
                    string[] pixel = pixelReader.ReadLine().Split(' ');
                    cluster.Points[i] = new PixelPoint(ushort.Parse(pixel[0]), ushort.Parse(pixel[1]), double.Parse(pixel[2].Replace('.', ',')), double.Parse(pixel[3].Replace('.', ',')));
                }
            }
            return (cluster);
        }
        /*public static string[] SimpleLoadNext(StreamReader pixelReader, StreamReader clusterReader)
        {
            string[] clusterInfo = getLine(1, clusterReader).Split(' ');
            cluster.FirstToA = double.Parse(clusterInfo[0]);
            cluster.PixelCount = uint.Parse(clusterInfo[1]);
            cluster.ByteStart = ulong.Parse(clusterInfo[3]);
            cluster.Points = new PixelPoint[cluster.PixelCount];
            pixelReader.BaseStream.Seek((long)cluster.ByteStart, SeekOrigin.Begin);
            for (int i = 0; i < cluster.PixelCount; i++)
            {
                string[] pixel = pixelReader.ReadLine().Split(' ');
                cluster.Points[i] = new PixelPoint(ushort.Parse(pixel[0]), ushort.Parse(pixel[1]));
                cluster.data[Int32.Parse(pixel[0]), Int32.Parse(pixel[1])] = Double.Parse(pixel[3].Replace('.', ','));   //pixel[2] contains info about time 
            }
            return (cluster);
        }*/



    }
    public struct PixelPoint
    {

        public PixelPoint(ushort xCoord, ushort yCoord, double ToA, double ToT)
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
    }
}
