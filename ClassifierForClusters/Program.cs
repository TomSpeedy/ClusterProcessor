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
        static void TrimFile()
        {
            const string filePath = "D:/source/repos/Example_data/train_data/trainElMuPi.json";
            StreamReader reader = new StreamReader(filePath);
            JsonTextReader jReader = new JsonTextReader(reader);
            StreamWriter writer = new StreamWriter(filePath + "_new");
            int lineCount = 300000;
            jReader.Read();
            for (int i = 0; i < lineCount; i++)
            {
                jReader.Read();
                JObject jObject = JObject.Load(jReader);
                writer.WriteLine(jObject);
                
            }
            writer.Close();
        }
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            const string distrOption = "--distr";     
            const string printClassesAndSpecialsOption = "--specials";
            const string multiOption = "--multi";
            IClassifier classifier;
            if (args.Contains(multiOption))
            {
                classifier = new MultiLayeredClassifier();
                Console.WriteLine(args[0]);
                classifier.LoadFromFile(args[0]);
                
            }
            else
            {
                classifier = new NNClassifier();
                classifier.LoadFromFile(args[0]);
            }
            
            if (args.Contains(distrOption))
            {
                Console.WriteLine("Classification in progress..");
                var histo = classifier.ClassifyCollection(args[1], ClassificationOutputType.Histogram);
                PrintHistogram(histo);
            }
            else 
            {
                Console.WriteLine("Classification in progress..");
                var histo = classifier.ClassifyCollection(args[1], 
                    args.Contains(printClassesAndSpecialsOption) ? ClassificationOutputType.PrintClassesAndSpecials
                       : ClassificationOutputType.PrintClasses);
                PrintHistogram(histo);
            }

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


