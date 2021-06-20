using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ClusterCalculator;
using System.Threading;
using System.Globalization;
namespace ClusterCalculator
{
    public interface IClusterWriter
    {
        void WriteClusterInfo(ClusterInfo clusterInfo);
    }
    /// <summary>
    /// Writes the cluster to the given output in MM format
    /// </summary>
    public class MMClusterWriter : IClusterWriter
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
    public interface IDescriptionWriter
    {
        void WriteDescription(Dictionary<ClusterAttribute, object> attrtibutes, long id = 0);
        void Close();
    }
    /// <summary>
    /// writes the JSON attribute description of a cluster
    /// </summary>
    public class JSONDecriptionWriter : IDescriptionWriter
    {
        private StreamWriter OutputStream { get; set; }
        private long WrittenCount { get; set; } = 0;
        public JSONDecriptionWriter(StreamWriter outputStream)
        {
            OutputStream = outputStream;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            outputStream.WriteLine("[");
        }
        public void WriteDescription(Dictionary<ClusterAttribute, object> attributes, long id = 0)
        {

            if (WrittenCount > 0)
            {
                OutputStream.Write(',');
                OutputStream.WriteLine();
            }        
            ++WrittenCount;
            if (id == 0)
                id = WrittenCount;
            OutputStream.WriteLine('{');
            OutputStream.WriteLine($"\t\"id\":{id},");
            int depth = 1;
            int attributeProcessed = 0;
            foreach (var attribute in attributes)
            {
                if (attribute.Value is List<Dictionary<BranchAttribute, object>>)
                {
                    var innerId = 0;
                    OutputStream.WriteLine($"\t\"{attribute.Key}\":");
                    OutputStream.WriteLine("\t[");
                    foreach (var branchDict in (List<Dictionary<BranchAttribute, object>>)attribute.Value)
                    {
                        WriteDescription(branchDict, depth + 1);
                        if (innerId < ((List<Dictionary<BranchAttribute, object>>)attribute.Value).Count - 1)
                            OutputStream.Write(',');
                        OutputStream.WriteLine();
                        innerId++;
                    }
                    OutputStream.Write("\t]");
                }     
                else
                    OutputStream.Write($"\t\"{attribute.Key}\":{attribute.Value}");
                attributeProcessed++;
                if (attributeProcessed < attributes.Count)
                    OutputStream.Write(',');
                OutputStream.WriteLine();

            }
            OutputStream.Write("}");
        }
        public void WriteDescription(Dictionary<BranchAttribute, object> attributes, int depth)
        {
            string prefix = new string('\t', depth);
            OutputStream.WriteLine(prefix + '{');
            int attributeProcessed = 0;
            foreach (var attribute in attributes)
            { 
                if (attribute.Value is List<Dictionary<BranchAttribute, object>>)
                {

                    var subBranchesProcessed = 0;
                    var branchAttributes = (List<Dictionary<BranchAttribute, object>>)attribute.Value;
                    OutputStream.WriteLine($"{prefix}\t\"{attribute.Key}\":");
                    OutputStream.WriteLine($"{prefix}\t[");
                foreach (var subBranch in branchAttributes)
                    {
                        WriteDescription(subBranch, depth + 2);
                        subBranchesProcessed++;
                        if (subBranchesProcessed < branchAttributes.Count)
                        {
                            OutputStream.WriteLine(',');
                        }
                        else
                            OutputStream.WriteLine();
                    }
                    OutputStream.WriteLine($"{prefix}\t]");
                }
                else
                    OutputStream.Write(prefix + $"\t\"{attribute.Key}\":{attribute.Value}");
                attributeProcessed++;
                if (attributeProcessed < attributes.Count)
                    OutputStream.Write(',');
                OutputStream.WriteLine();
            }
            OutputStream.Write(prefix + '}');
        }
        public void Write(string clusterRecord)
        {           
            if (WrittenCount > 0)
            {
                OutputStream.Write(',');
                OutputStream.WriteLine();
            }
            OutputStream.Write(clusterRecord);
            WrittenCount++;
        }
        public void Close()
        {
            if(OutputStream.BaseStream.CanWrite)
            {
                OutputStream.WriteLine();
                OutputStream.WriteLine("]");
                OutputStream.Close();
            }
        }
    }

}
