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

namespace ClusterVisualizer
{
    public partial class Form1 : Form
    {
        const string confFileName = "info.ini";
        const int clusterNumber = 20908;
        public PictureBox box = new PictureBox();
        public Form1()
        {
            InitializeComponent();
            box.Width = 800;
            box.Height = 800;
            box.Parent = this;
            box.Visible = true;
            box.Location = new Point(0,0);
            Cluster cluster = Cluster.LoadFromFile(confFileName, clusterNumber);
            cluster.View(this);
        }

    }
  
    static class Reader
    {
        
    }
    class Cluster
    {
        double[,] data = new double[256, 256];
        Bitmap pixels = new Bitmap(256, 256);
        
        private static string getFileName(string line)
        {
            int index = line.IndexOf('=');
            return (line.Substring(index+ 1));
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
                    pixels.SetPixel(i, j, Color.FromArgb(255, Convert.ToInt32(data[i, j]),0 , 0));
                    
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
        public static Cluster LoadFromFile(string confFileName, int clusterNumber)
        {
            StreamReader confReader = new StreamReader(confFileName);
            confReader.ReadLine();
            string pixelFileName = getFileName(confReader.ReadLine());
            string clusterFileName = getFileName(confReader.ReadLine());
            StreamReader pixelReader = new StreamReader(pixelFileName);
            StreamReader clusterReader = new StreamReader(clusterFileName);
            Cluster cluster = new Cluster();
            int lineNum = GetMinCluster(500, clusterReader);
            string[] clusterInfo = getLine(lineNum, clusterReader).Split(' ');
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

    }

    
}

