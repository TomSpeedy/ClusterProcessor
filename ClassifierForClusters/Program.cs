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
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            const string distrOption = "--distr";     
            const string printClassesAndSpecialsOption = "--specials";
            const string multiOption = "--multi";
            const string multiFileOption = "--multiFile";

            IClassifier classifier;
            if (args.Contains(multiOption))
            {
                classifier = new MultiLevelClassifier();
                classifier.LoadFromFile(args[0]);
                
            }
            else
            {
                classifier = new NNClassifier();
                classifier.LoadFromFile(args[0]);
            }
            ClassificationOutputType? outputType = null;
            if (args.Contains(distrOption))
            {
                outputType = ClassificationOutputType.Histogram;
            }
            else if(args.Contains(printClassesAndSpecialsOption))
            {               
                outputType = ClassificationOutputType.SplitClassesAndSpecials;                                      
            }
            else
            {
                outputType = ClassificationOutputType.SplitClasses;
            }          
            Console.WriteLine("Classification in progress..");
            var histo = classifier.ClassifyCollection(args[1], outputType.Value, 
                args.Contains(multiFileOption) ? ClassificationOutputFileCount.Multiple : ClassificationOutputFileCount.Single);
            PrintHistogram(histo);
            Console.ReadLine();
           
        }
        static void PrintHistogram(Dictionary<string, int> histogram)
        {
            foreach (var key in histogram.Keys)
            {
                Console.WriteLine($"{key} : {histogram[key]}");
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


