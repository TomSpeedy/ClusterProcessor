using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    public class ClusterClassPartition
    {
        public string ConfigPath { get; }
        public ClusterInfoCollection Collection { get; }
        public IEnumerator<ClusterInfo> Enumerator { get; private set; }
        public EnergyCenterFinder CenterFinder { get; }
        public EnergyCalculator EnergyCalc { get; }
        public BranchAnalyzer BranchAnalyzer { get; }
        public VertexFinder VertexFinder { get; }
        public ISkeletonizer Skeletonizer { get; }
        public double MaxRead { get; set; } = 1;

        public ClusterClassPartition(ClusterInfoCollection collection, string configPath)
        {
            ConfigPath = configPath;
            Collection = collection;
            Enumerator = collection.GetEnumerator();
            CenterFinder = new EnergyCenterFinder();
            BranchAnalyzer = new BranchAnalyzer(CenterFinder);
            EnergyCalc = new EnergyCalculator();
            VertexFinder = new VertexFinder();
            Skeletonizer = new ThinSkeletonizer();
        }
        public void ResetEtor()
        {
            Enumerator = Collection.GetEnumerator();
        }
        public bool CheckPosition()
        {
            return Collection.ClFile.BaseStream.Position < Collection.ClFile.BaseStream.Length * MaxRead;
        }
    }
    public class ClusterClassCollection
    {
        public string Class;
        public List<ClusterClassPartition> Partitions;
        public int PartitionIndex { get; private set; }
        private Random random { get; set; }
        public IEnumerator<ClusterInfo> CurrentEnumerator { get; set; }
        public long CalcLength() { return Partitions.Select(partition => partition.Collection.ClFile.BaseStream.Length).Sum(); }


        public ClusterClassCollection(ClusterClassPartition startPartition, string clusterClass)
        {
            Class = clusterClass;
            Partitions = new List<ClusterClassPartition>();
            Partitions.Add(startPartition);
            PartitionIndex = 0;
            CurrentEnumerator = Partitions[PartitionIndex].Enumerator;
            random = new Random();
        }

        public void SetNewCurrentEnumerator(bool chooseRandomly = true)
        {
            if (Partitions.Count > 0)
            {
                if (chooseRandomly)
                    PartitionIndex = random.Next(0, Partitions.Count);
                else
                    PartitionIndex = (PartitionIndex + 1) % Partitions.Count;
                CurrentEnumerator = Partitions[PartitionIndex].Enumerator;
            }
        }

        public bool RemovePartition(bool chooseRandomly = true)
        {
            if (Partitions.Count > 1)
            {
                Partitions[PartitionIndex].Enumerator.Dispose();
                Partitions.RemoveAt(PartitionIndex);
                SetNewCurrentEnumerator(chooseRandomly);
                return true;
            }
            if (Partitions.Count == 1)
            {
                Partitions[0].Enumerator.Dispose();
                Partitions.RemoveAt(0);
            }
            return false;
        }
    

    }
}
