using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using ClusterCalculator;

namespace ClassifierForClusters
{

       

    class Program
    {
        
        static void Main(string[] args)
        {
            args = new string[] { "program", "D:/source/repos/Celko 2020 Example data/trained_models/bestClassifier.csf",
                "D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/etstNoBackSlash.json", "--classes"  };
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            const string distrOption = "--distr";     
            const string printClassesAndSpecialsOption = "--specials";
            const string multiOption = "--multi";
            IClassifier classifier;
            if(args.Contains(multiOption))
            { 
                classifier = new MultiLayeredClassifier();
            classifier.LoadFromFile(args[1]);
            }
            else
            {
                classifier = new NNClassifier();
                classifier.LoadFromFile(args[1]);
            }
            if (args.Contains(distrOption))
            {
                classifier.LoadFromFile(args[1]);
                var histo = classifier.ClassifyCollection(args[2], ClassificationOutputType.Histogram);
                foreach (var pair in histo)
                {
                    Console.WriteLine(pair.Key + ":" + pair.Value);
                }
            }
            else 
            {
                classifier.LoadFromFile(args[1]);
                var histo = classifier.ClassifyCollection(args[2], 
                    args.Contains(printClassesAndSpecialsOption) ? ClassificationOutputType.PrintClassesAndSpecials
                       : ClassificationOutputType.PrintClasses);
            }

        }
        
    }
    static class DictionaryExtensions
    {
        public static Dictionary<TKey, object> ToObjectDictionary<TKey, TValue>(this Dictionary<TKey, TValue> origDict)
        {
            Dictionary<TKey, object> result = new Dictionary<TKey, object>();
            foreach (var keyValPair in origDict)
            {
                result.Add(keyValPair.Key, keyValPair.Value);
            }
            return result;
        }
    }
}


