using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassifierForClusters;
namespace ClassifierTrainer
{
    class Program
    {
        static string GetOptionValue(string[] args, string optionName)
        {
            if (!args.Contains(optionName))
                return null;
            var optionIndex = Array.IndexOf(args, optionName);
            if (optionIndex == args.Length - 1)
                throw new ArgumentException($"Error - Option {optionName} was not followed by any value");
            return args[optionIndex + 1];
        }
        static void Main(string[] args)
        {
            const int expectedArgLen = 3;
            const string seedOption = "--seed";
            const string minAccurOption = "--acc";
            const string maxRepetOption = "--maxrep";
            const string trainedModelOption = "--trained";
            if (args.Length < expectedArgLen)
            {
                Console.WriteLine("Error - insufficient number of arguments passed");
                return;
            }
            string trainingData = args[1];
            string configData = args[2];
            string seedParsed = GetOptionValue(args, seedOption);
            string maxRepParsed = GetOptionValue(args, maxRepetOption);
            string minAccParsed = GetOptionValue(args, minAccurOption);
            string trainedModelParsed = GetOptionValue(args, trainedModelOption);
            ITrainableClassifier classifier;
            if (trainedModelParsed == "")
                classifier = TrainableClassifierFactory.CreateNew("defaultMLP");
            else
            {
                classifier = new NNClassifier();
                classifier.LoadFromFile(trainedModelParsed);
            }
            int maxRepetCount = 1;
            if (uint.TryParse(maxRepParsed, out uint chosenRepetCount))
            {
                maxRepetCount = (int)chosenRepetCount;
            }
            double minAccuracy = 0;
            if (double.TryParse(minAccParsed, out double chosenMinAccuracy))
            {
                minAccuracy = chosenMinAccuracy;
            }
            int seed = 0;
            if (int.TryParse(seedParsed, out int parsedSeed))
            {
                seed = parsedSeed;
            }
            int iterationCount = 0;
            double accuracy = 0;
            bool stopped = false;
            while (iterationCount < maxRepetCount && accuracy <= minAccuracy)
            {
                accuracy = classifier.Train(configData, trainingData, ref stopped, minAccuracy, seed);
                iterationCount++;
            }
            Console.WriteLine("The training process has successfully ended");

        }
    }
}
