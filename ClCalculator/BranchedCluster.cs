﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{

    public class BranchedCluster : Cluster
    {
        public BranchedCluster(Cluster cluster, List<Branch> mainBranches)
            : base(cluster.FirstToA, cluster.PixelCount, cluster.ByteStart)
        {
            MainBranches = mainBranches;
        }
        public List<Branch> MainBranches { get; private set; }
        public List<Dictionary<BranchAttribute, object>> ToDictionaries()
        {
            List<Dictionary<BranchAttribute, object>> dicts = new List<Dictionary<BranchAttribute, object>>();            
            foreach (var branch in MainBranches)
                dicts.Add(branch.ToDictionary());
            return dicts;
        }
    }
}
