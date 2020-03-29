using System;
using System.IO;

namespace ReaderForClusters
{
    class Image
    {
        public static Image LoadFromFile(string confFileName, int clusterNumber)
        {
            StreamReader reader = new StreamReader(confFileName);
            for (int i = 0; i < clusterNumber; i++)
            {
                reader.ReadLine()
            }
            
        }
        int[,] data = new int[256, 256];

    }
        
    class Program
    {
        const string confFileName = "info.ini";
        const int clusterNumber = 4;
        static void Main(string[] args)
        {
            Image image = Image.LoadFromFile(confFileName, clusterNumber);

        }
    }
}
