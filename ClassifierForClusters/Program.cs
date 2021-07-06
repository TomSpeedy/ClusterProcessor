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
            const string splitOption = "--split";
            const string multiOption = "--multi";
            const string multiFileOption = "--multiFile";
            const string singleFileOption = "--singleFile";
            args = new string[] {
                "D:/source/repos/Example_data/trained_models/bestClassifier.csf",
                "D:/source/repos/Example_data/test_data/testCollection.json",
                "--default",
                "--singleFile",
                "--multi"
            };
            IClassifier classifier;
            if (args.Contains(multiOption))
            {
                classifier = new MultiLayeredClassifier();
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
            var histo = classifier.ClassifyCollection(args[1], ClassificationOutputType.SplitClasses, 
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


