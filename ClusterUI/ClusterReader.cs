using System;
using System.IO;
using System.Windows.Forms;
using ClusterCalculator;
namespace ClusterUI
{
    interface IClusterReader {
        void GetTextFileNames(TextReader reader, string iniPath, out string pxFile, out string clFile);
        Cluster LoadFromText(StreamReader pixelStream, StreamReader clusterStream, int clusterNumber = 1);

    }
    class MMClusterReader : IClusterReader
    {
        private string getLine(int clusterNumber, StreamReader reader)
        {
            for (int i = 0; i < clusterNumber - 1; i++)
            {
                reader.ReadLine();
            }
            return reader.ReadLine();

        }
        public void GetTextFileNames(TextReader reader, string iniPath, out string pxFile, out string clFile)
        {
            try
            {
                var prefixPath = PathParser.GetPrefixPath(iniPath);
                reader.ReadLine();
                string[] tokens1 = reader.ReadLine().Split('=');
                pxFile = prefixPath + tokens1[1];
                string[] tokens2 = reader.ReadLine().Split('=');
                clFile = prefixPath + tokens2[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Processing error, Selected '.ini' file is not in correct format or is inaccessible.");
                throw (ex);
            }
        }
        public Cluster LoadFromText(StreamReader pixelStream, StreamReader clusterStream, int clusterNumber = 1)
        {
            try
            {
                string[] clusterInfo = getLine(clusterNumber, clusterStream)?.Split(' ');
                if (clusterInfo == null)
                    return null;

                Cluster cluster = new Cluster(FirstToA: double.Parse(clusterInfo[0].Replace('.', ',')),
                                              PixelCount: uint.Parse(clusterInfo[1]),
                                              ByteStart: ulong.Parse(clusterInfo[3]));
                cluster.Points = new PixelPoint[cluster.PixelCount];

                pixelStream.DiscardBufferedData();
                pixelStream.BaseStream.Position = (long)cluster.ByteStart;

                for (int i = 0; i < cluster.PixelCount; i++)
                {
                    string[] pixel = pixelStream.ReadLine().Split(' ');
                    cluster.Points[i] = new PixelPoint(ushort.Parse(pixel[0]), ushort.Parse(pixel[1]), double.Parse(pixel[2].Replace('.', ',')), double.Parse(pixel[3].Replace('.', ',')));
                }

                return (cluster);
            }
            catch
            {
                MessageBox.Show("Cluster was not successfully loaded because either there was an error parsing input file");
                return null;
            }
        }
    }
}
