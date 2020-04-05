using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Filters;

namespace ClusterVisualizer
{
    public partial class Form1 : Form
    {
        const string confFileName = "infoBin.ini";
        const int clusterNumber = 0;
        public PictureBox box = new PictureBox();
        public Form1()
        {
            InitializeComponent();
            box.Width = 800;
            box.Height = 800;
            box.Parent = this;
            box.Visible = true;
            box.Location = new Point(0,0);
            Cluster cluster = Cluster.LoadFromBinary(confFileName, clusterNumber);
            cluster.View(this);
            
        }

    }
  
    static class Reader
    {
        
    }
    class Cluster
    {
        const int clusterDataSize = 21; //in bytes
        public double FirstToA { get; private set; }
        public uint PixelCount { get; private set; }
        public ulong ByteStart { get; private set; }
        double[,] data = new double[256, 256];
        Bitmap pixels = new Bitmap(256, 256);
        private static void getTextFileNames(TextReader reader, out string pxFile, out string clFile)
        {
            string[] tokens1 = reader.ReadLine().Split('=');
            pxFile = tokens1[1];
            string[] tokens2 = reader.ReadLine().Split('=');
            clFile = tokens2[1];         
        }
        private static void getBinaryFileNames(BinaryReader reader, out string pxFile, out string clFile)
        {
            throw new NotImplementedException();
        }
        private static string getLine(int clusterNumber, TextReader reader)
        {
            for (int i = 0; i < clusterNumber - 1; i++)
            {
                reader.ReadLine();
            }
            return (reader.ReadLine());

        }
        public void View(Form1 form)
        {
            for (int i = 0;i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    if (data[i, j] > 0)
                    {
                        pixels.SetPixel(i, j, Color.FromArgb(255, 255, 0, 0));
                    }
                    else
                    {
                        pixels.SetPixel(i, j, Color.FromArgb(255, 0, 0, 0));
                    }
                }
            }
            form.box.SizeMode = PictureBoxSizeMode.StretchImage;
            form.box.Image = pixels;



        }
        public static int GetMinCluster(int minimumPixelCount, StreamReader reader)
        {
            string [] tokens = reader.ReadLine().Split(' ');
            while (reader.Peek() != -1 && Int32.Parse(tokens[1]) < minimumPixelCount)
            {
                tokens = reader.ReadLine().Split(' ');
            }
            return Int32.Parse(tokens[2]);

        }
        public static Cluster LoadFromText(string confFileName, int clusterNumber)
        {
            StreamReader confReader = new StreamReader(confFileName);
            confReader.ReadLine();
            getTextFileNames(confReader, out string pixelFileName, out string clusterFileName);
           
            StreamReader pixelReader = new StreamReader(File.Open(pixelFileName, FileMode.Open));
            StreamReader clusterReader = new StreamReader(File.Open(clusterFileName, FileMode.Open));
            Cluster cluster = new Cluster();

            int lineNum = GetMinCluster(500, clusterReader);
            string[] clusterInfo = getLine(lineNum, /*clusterNumber,*/ clusterReader).Split(' ');
            
            
            for (int i = 0; i < Int32.Parse(clusterInfo[2]); i++)
            {
                pixelReader.ReadLine();
            }
            for (int i = 0; i < Int32.Parse(clusterInfo[1]); i++)
            {
                string[] pixel = pixelReader.ReadLine().Split(' ');
                cluster.data[Int32.Parse(pixel[0]), Int32.Parse(pixel[1])] = Double.Parse(pixel[3].Replace('.', ','));   //pixel[2] contains info about time 
            }
            return (cluster);
        }
        public static Cluster LoadFromBinary(string confFileName, int clusterNumber)
        {
            BinaryReader confReader = new BinaryReader(File.Open(confFileName, FileMode.Open));
            getBinaryFileNames(confReader, out string pixelFileName, out string clusterFileName);

            BinaryReader pixelReader = new BinaryReader(File.Open(pixelFileName, FileMode.Open));
            BinaryReader clusterReader = new BinaryReader(File.Open(clusterFileName, FileMode.Open));
            Cluster cluster = new Cluster();
            clusterReader.BaseStream.Seek(clusterDataSize * clusterNumber, SeekOrigin.Current);
            
            byte Zerobyte = clusterReader.ReadByte();
            cluster.FirstToA = clusterReader.ReadDouble();
            cluster.PixelCount = clusterReader.ReadUInt32();
            cluster.ByteStart = clusterReader.ReadUInt64();
            //byte Zerobyte = clusterReader.ReadByte();

            pixelReader.BaseStream.Seek((long)cluster.ByteStart, SeekOrigin.Current);
            for (int i = 0; i < cluster.PixelCount; i++)
            {
                ushort x = pixelReader.ReadUInt16();
                ushort y = pixelReader.ReadUInt16();
                double ToA = pixelReader.ReadDouble();
                double ToT = pixelReader.ReadDouble();
                cluster.data[x, y] = ToT;
            }
            return cluster;
            
           
            
        }


    }

    
}

