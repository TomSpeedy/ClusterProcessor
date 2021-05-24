using System;
using System.IO;
using ClusterCalculator;
using System.Threading;
using System.Globalization;
namespace ClusterCalculator
{

    public interface IClusterReader 
    {
        void GetTextFileNames(TextReader reader, string iniPath, out string pxFile, out string clFile);
        Cluster LoadFromText(StreamReader pixelStream, StreamReader clusterStream, int clusterNumber = 1);
        Cluster LoadByClInfo(StreamReader pixelStream, ClusterInfo clInfo);

    }
    /// <summary>
    /// handles the read operations of a cluster
    /// </summary>
    public class MMClusterReader : IClusterReader
    {
        public MMClusterReader()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
        }
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
            
            var prefixPath = PathParser.GetPrefixPath(iniPath);
            reader.ReadLine();
            string[] tokens1 = reader.ReadLine().Split('=');
            pxFile = prefixPath + tokens1[1];
            string[] tokens2 = reader.ReadLine().Split('=');
            clFile = prefixPath + tokens2[1];
     

        }
        /// <summary>
        /// Loads cluster from the text file
        /// </summary>
        /// <param name="pixelStream">px file</param>
        /// <param name="clusterStream">cl file</param>
        /// <param name="clusterNumber">cl index</param>
        /// <returns>initialized cluster</returns>
        public Cluster LoadFromText(StreamReader pixelStream, StreamReader clusterStream, int clusterNumber = 1)
        {
            try
            {
                string[] clusterInfo = getLine(clusterNumber, clusterStream)?.Split(' ');
                if (clusterInfo == null)
                    return null;

                Cluster cluster = new Cluster(FirstToA: double.Parse(clusterInfo[0]),
                                              PixelCount: uint.Parse(clusterInfo[1]),
                                              ByteStart: ulong.Parse(clusterInfo[3]));
                cluster.Points = new PixelPoint[cluster.PixelCount];

                pixelStream.DiscardBufferedData();
                pixelStream.BaseStream.Position = (long)cluster.ByteStart;
                try
                {
                    for (int i = 0; i < cluster.PixelCount; i++)
                    {
                        string[] pixel = pixelStream.ReadLine().Split(' ');
                        cluster.Points[i] = new PixelPoint(ushort.Parse(pixel[0]), ushort.Parse(pixel[1]), double.Parse(pixel[2]), double.Parse(pixel[3]));
                    }
                }
                catch 
                {
                    throw new Exception("Cluster was not successfully loaded because there was an error parsing px file");
                }

                return (cluster);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Load clsuter by its clInfo object
        /// </summary>
        /// <param name="pixelStream">px file</param>
        /// <param name="clInfo"> current clInfo</param>
        /// <returns> initialized clsuter</returns>
        public Cluster LoadByClInfo(StreamReader pixelStream, ClusterInfo clInfo)
        {
            Cluster cluster = new Cluster(FirstToA: clInfo.FirstToA,
                                             PixelCount: clInfo.PixCount,
                                             ByteStart: clInfo.ByteStart);
            cluster.Points = new PixelPoint[cluster.PixelCount];

            pixelStream.DiscardBufferedData();
            pixelStream.BaseStream.Position = (long)cluster.ByteStart;

            for (int i = 0; i < cluster.PixelCount; i++)
            {
                string[] pixel = pixelStream.ReadLine().Split(' ');
                cluster.Points[i] = new PixelPoint(ushort.Parse(pixel[0]), ushort.Parse(pixel[1]), double.Parse(pixel[2]), double.Parse(pixel[3]));
            }

            return (cluster);
        }
    }
}
