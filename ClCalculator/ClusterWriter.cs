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
        void WriteDescription(Dictionary<ClusterAttribute, object> attrtibutes);
        void Close();
    }

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
        public void WriteDescription(Dictionary<ClusterAttribute, object> attributes)
        {

            if (WrittenCount > 0)
            {
                OutputStream.Write(',');
                OutputStream.WriteLine();
            }
            ++WrittenCount;
            OutputStream.WriteLine('{');
            OutputStream.WriteLine($"\t\"id\":{WrittenCount},");
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
        public void Close()
        {
            OutputStream.WriteLine();
            OutputStream.WriteLine("]");
            OutputStream.Close();
        }
    }

}
