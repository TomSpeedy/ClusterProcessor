using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ClusterUI
{
    interface IClusterWriter
    {
        void WriteClusterInfo(ClusterInfo clusterInfo);
    }
    class MMClusterWriter : IClusterWriter
    {
        private StreamWriter OutputStream { get; set; }
        public MMClusterWriter(StreamWriter outputStream)
        {
            OutputStream = outputStream;
        }
        public void WriteClusterInfo(ClusterInfo clusterInfo)
        {
            OutputStream.WriteLine($"{clusterInfo.FirstToA} {clusterInfo.PixCount} {clusterInfo.LineStart} {clusterInfo.ByteStart}");
        }
    }
}
