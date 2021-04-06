using System;
using System.IO;
using System.Collections.Generic;
using Accord.Neuro.Learning;
using Accord.Neuro;
using Accord.Statistics;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Accord.Math;
using System.Globalization;
using System.Threading;

namespace ClusterClassifier
{
    [Serializable]
    struct Interval
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Size => Max - Min;

        public Interval(double min, double max)
        {
            Min = min;
            Max = max;
        }

    }
    class NNInputProcessor
    {
        public double[] NormalizeElements(double[] originVector, Interval[] intervalVector)
        {
            double[] newVector = new double[originVector.Length];
            for (int i = 0; i < originVector.Length; i++)
            {
                newVector[i] =  (originVector[i] - intervalVector[i].Min) / (intervalVector[i].Size);
            }
            return newVector;
        }
        public Interval[] CalculateNormIntervals(string jsonPath, string[] usableKeys, string[] classes)
        {
            StreamReader sReader = new StreamReader(jsonPath);
            JsonTextReader jReader = new JsonTextReader(sReader);
            var squeezeIntervals = new Interval[usableKeys.Length];
            for (int i = 0; i < squeezeIntervals.Length; i++)
            {
                squeezeIntervals[i] = new Interval(double.MaxValue, double.MinValue);
            }
            while (sReader.Peek() != -1)
            {
                var numVector = ReadJsonToVector(jReader, usableKeys, classes, out int classIndex);
                if (classIndex == -1)
                    continue;
                for(int j = 0; j < numVector.Length;j++)
                {
                    if (numVector[j] < squeezeIntervals[j].Min)
                        squeezeIntervals[j].Min = numVector[j];
                    if (numVector[j] > squeezeIntervals[j].Max)
                        squeezeIntervals[j].Max = numVector[j];

                }
            }
            return squeezeIntervals;
        }
        public double[] ReadJsonToVector(JsonTextReader reader, string[] usableKeys, string[] classes, out int classIndex)
        {

            if (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {

                    List<double> resultVector = new List<double>();
                    JObject jsonRecord = JObject.Load(reader);
                    foreach (var usableKey in usableKeys)
                    {
                        double attributeVal;
                        if (!jsonRecord.ContainsKey(usableKey))
                            throw new ArgumentException($"Error, Required property \"{usableKey}\" for training set is not included in given input file");

                        if (double.TryParse(jsonRecord[usableKey].ToString(), out attributeVal))
                        {
                            resultVector.Add(attributeVal);
                        }

                    }
                    const string ClassKey = "Class";
                    if (!jsonRecord.ContainsKey(ClassKey))
                        throw new ArgumentException($"Error, Required \"{ClassKey}\" property is not included in given input file");

                    classIndex = Array.IndexOf(classes, jsonRecord[ClassKey].ToString());
                    if (classIndex < 0)
                        throw new ArgumentException($"Error, Class value: \"{jsonRecord[ClassKey]}\" is not valid class value");
                    return resultVector.ToArray();
                }
            }
            classIndex = -1;
            return null;

        }
    }
    class NNClassifier
    {
        ActivationNetwork Network { get; set; }
        public Interval[] SqueezeIntervals { get; set; }
        private NNClassifier() { }
        public NNClassifier(int inputLen, int outputLen, int[] layerSizes,  IActivationFunction activationFunction, Interval[] squeezeIntervals)
        {
           Network = new ActivationNetwork(
                activationFunction,
                inputLen,
                layerSizes.Append(outputLen).ToArray()
                );
            SqueezeIntervals = squeezeIntervals;
                    
        }
        public double Learn(int epochSize, string learnJsonPath, string[] validProperties, string []outputClasses, double successThreshold)
        {
            var inputStream = new StreamReader(learnJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            NNInputProcessor preprocessor = new NNInputProcessor();
            //empty read
            var _inputVector = preprocessor.ReadJsonToVector(jsonStream, validProperties, outputClasses, out int _classIndex);
            BackPropagationLearning teacher = new BackPropagationLearning(Network);
            teacher.Momentum = 0.5;
            teacher.LearningRate = 0.7;
            
            var iteration = 0;
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length * 0.9)
            {
                double[][] input = new double[epochSize][];
                double[][] output = new double[epochSize][];

                for (int j = 0; j < epochSize; j++)
                {
                    var inputVector = preprocessor.NormalizeElements(preprocessor.ReadJsonToVector(jsonStream, validProperties, outputClasses, out int classIndex), SqueezeIntervals);
                    var outputVector = new double[outputClasses.Length];
                    outputVector[classIndex] = 1;
                    input[j] = inputVector;
                    output[j] = outputVector;
                }

                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output);
                if (iteration % 50 == 0)
                    Console.WriteLine(error);
                iteration++;
            }
            int[][] confusionMatrix = new int[outputClasses.Length][];
            for (int j = 0; j < outputClasses.Length; j++)
            {
                confusionMatrix[j] = new int[outputClasses.Length];
            }
            inputStream = new StreamReader(learnJsonPath);
            jsonStream = new JsonTextReader(inputStream);
            _inputVector = preprocessor.ReadJsonToVector(jsonStream, validProperties, outputClasses, out int ___classIndex);
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length * 0.1)
            {
                var unNormalizedVect = preprocessor.ReadJsonToVector(jsonStream, validProperties, outputClasses, out int classIndex);
                var inputVector = preprocessor.NormalizeElements(unNormalizedVect, SqueezeIntervals);
                var outputVector = new double[outputClasses.Length];
                outputVector[classIndex] = 1;

                var result = Network.Compute(inputVector);
                var predictedClass = result.ArgMax();
                confusionMatrix[predictedClass][classIndex]++;

            }
            var totalSum = 0;
            var diagSum = 0;
            for (int j = 0; j < outputClasses.Length; j++)
            {

                for (int k = 0; k < outputClasses.Length; k++)
                {
                    Console.Write(confusionMatrix[j][k]);
                    Console.Write("\t");
                    totalSum += confusionMatrix[j][k];
                    if (j == k)
                        diagSum += confusionMatrix[j][k];


                }
                Console.WriteLine();
            }
            var successRate = diagSum / (double)totalSum;
            Console.WriteLine("Success rate is : " + successRate);
            if (successRate > successThreshold)
                StoreToFile(learnJsonPath + "trained "+ successRate +".txt");
            return successRate;
        }
        public void StoreToFile(string outJsonPath)
        {
            Network.Save(outJsonPath);

            StreamWriter writer = new StreamWriter(outJsonPath + "_intervals");
            writer.Write(System.Text.Json.JsonSerializer.Serialize(SqueezeIntervals));
            writer.Close();
        }
        public static NNClassifier LoadFromFile(string inJsonPath)
        {
            var classifier = new NNClassifier();
            classifier.Network = (ActivationNetwork) ActivationNetwork.Load(inJsonPath);
                    classifier.SqueezeIntervals = new Interval[classifier.Network.InputsCount];
            StreamReader sReader = new StreamReader(inJsonPath + "_intervals");
            JsonTextReader jReader = new JsonTextReader(sReader);
            jReader.Read();
            
            for (int i = 0; i < classifier.SqueezeIntervals.Length; i++)
            {
                jReader.Read();
                JObject intervalRecord = JObject.Load(jReader);
                classifier.SqueezeIntervals[i] = new Interval((double)intervalRecord["Min"], (double)intervalRecord["Max"]);
            }
            jReader.Close();
            sReader.Close();
            return classifier;
        }
        public int ClassifySingle(double[] inputVector)
        {
            NNInputProcessor preprocessor = new NNInputProcessor();
            var resultVector = Network.Compute(preprocessor.NormalizeElements(inputVector,SqueezeIntervals));
            return resultVector.ArgMax();
        }
        
    }
    class MultiLayeredClassifier
    {
        NNClassifier NNFrag = NNClassifier.LoadFromFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/fragBinary4.jsontrained 0.846.txt");
        NNClassifier NNHePrFeLe = NNClassifier.LoadFromFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/HePrFeLe_ElMuPi.jsontrainedBEST.txt");
        NNClassifier NNLead = NNClassifier.LoadFromFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/leadBinary.jsontrained0.996BEST.txt");
        NNClassifier NNElMuPi = NNClassifier.LoadFromFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/ElMuPi.jsontrained 0.73BEST.txt");
        NNInputProcessor preprocessor = new NNInputProcessor();

        string[] ValidClasses { get; set; }
        int[][] ConfusionMatrix { get; set; }
        //rework storing valid Classes
        public MultiLayeredClassifier(int classesCount)
        {

            ConfusionMatrix = new int[classesCount][];
            for (int i = 0; i < classesCount; i++)
            {
                ConfusionMatrix[i] = new int[classesCount];
            }
        }
        public int Classify(double[] inputVector)
        {
            var resultIndex = NNLead.ClassifySingle(inputVector);
            if (resultIndex == 1)
                return 0;
            resultIndex = NNFrag.ClassifySingle(inputVector);
            if (resultIndex == 0)
                    return 1;
            resultIndex = NNHePrFeLe.ClassifySingle(inputVector);
            switch (resultIndex)
            {
                case 0:
                    return 2;
                case 1:
                    return 3;
                case 2:
                    return 4;
                case 4:
                    return 5;
                default:
                    break;
                
            }
            resultIndex = NNElMuPi.ClassifySingle(inputVector);
            if (resultIndex == 0)
                return 6;
            if (resultIndex < 6)
                return 7;
            if (resultIndex < 9)
                return 8;

             return 9;
        }
    }
    class Program
    {
        static Random rand = new Random();
        static string[] validFields = new string[]{
                 "TotalEnergy",
                 "AverageEnergy",
                 "MaxEnergy",
                 "PixelCount",
                 "Convexity",
                 "Width",
                 "CrosspointCount",
                 "VertexCount",
                 "RelativeHaloSize",
                 "BranchCount"
                // "StdOfEnergy",
                 //"StdOfArrival",
                 //"RelLowEnergyPixels"
                 };
        static string[] outputClasses = new string[] {
            "lead",
            "frag",       
            "he",
            "proton",
            "fe",
            "low_electron",
            "muon",
            "electron",        
            "pion",
            "elPi0"
        };
        static double LearnFrag(double acceptableSuccessRate)
        {
            var jsonFilePath = "../../../../ClusterDescriptionGen/bin/Debug/trainFragMulti.json";
            var outputClasses = new string[] {
                 "frag",
                 "he",
                 "fe",
                 "other"
             };

            var inputStream = new StreamReader(jsonFilePath);

            NNInputProcessor preprocessor = new NNInputProcessor();


            var commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, validFields, outputClasses);
                //new Interval[]{ new Interval(0, 9000), new Interval(0, 50), new Interval(0, 500), new Interval(0, 400), new Interval(0, 1),
                                    //   new Interval(0, 150), new Interval(0, 15), new Interval(0, 15), new Interval(0,1), new Interval(0,10) };

            var epochSize = 16;
            NNClassifier fragClassifier = new NNClassifier(validFields.Length, outputClasses.Length, new int[] { 16, 16}, new SigmoidFunction(1), commonRareIntervals);
            //fragClassifier.Learn(epochSize, jsonFilePath, validFields, outputClasses, acceptableSuccessRate);
            //fragClassifier.Learn(epochSize, jsonFilePath, validFields, outputClasses, acceptableSuccessRate);
            return fragClassifier.Learn(epochSize, jsonFilePath, validFields, outputClasses, acceptableSuccessRate);
        }
        static double LearnLead(double acceptableSuccessRate)
        {
            
            string jsonFilePath = "../../../../ClusterDescriptionGen/bin/Debug/leadBinary.json";
            string[] outputClasses = new string[] {
                 "noLead",
                 "lead"
             };

            StreamReader inputStream = new StreamReader(jsonFilePath);
            

            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, validFields, outputClasses);//{ new Interval(0, 60000), new Interval(0, 100), new Interval(0, 600), new Interval(0, 1000), new Interval(0, 1),
                                                                                                                       // new Interval(0, 200), new Interval(0, 50), new Interval(0, 30), new Interval(0,1), new Interval(0,30) };
                                                                                                               
            int epochSize = 32;
            NNClassifier commonRareClassifier = new NNClassifier(validFields.Length, outputClasses.Length, new int[] { 16, 16 }, new SigmoidFunction(1), commonRareIntervals);
            return commonRareClassifier.Learn( epochSize, jsonFilePath, validFields, outputClasses, acceptableSuccessRate);
        }
        static double LearnHePrFeLe_ElMuPi(double acceptableSuccessRate)
        {

            string jsonFilePath = "../../../../ClusterDescriptionGen/bin/Debug/HePrFeLe_ElMuPi.json";
            string[] outputClasses = new string[] {
                 "he",
                 "proton",
                 "fe",
                 "elMuPi",
                 "low_electr",

            };

            StreamReader inputStream = new StreamReader(jsonFilePath);
            

            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, validFields, outputClasses);// { new Interval(0, 5000), new Interval(0, 70), new Interval(0, 400), new Interval(0, 500), new Interval(0, 1),
                                                                                                                       //  new Interval(0, 100), new Interval(0, 10), new Interval(0, 10), new Interval(0,1), new Interval(0,10) };
                                                                                                                       // loop
            int epochSize = 32;
            NNClassifier multiClassifier = new NNClassifier(validFields.Length, outputClasses.Length, new int[] { 16, 16 }, new SigmoidFunction(1), commonRareIntervals);
            return multiClassifier.Learn(epochSize, jsonFilePath, validFields, outputClasses, acceptableSuccessRate);
        }
        static double LearnElMuPi(double acceptableSuccessRate)
        {
            
            string jsonFilePath = "../../../../ClusterDescriptionGen/bin/Debug/ElMuPi.json";
            string[] outputClasses = new string[] {
                 "muon",
                 "electron15",
                 "electron30",
                 "electron45",
                 "electron60",
                 "electron75",
                 "pion22",
                 "pion45",
                 "pion60",
                 "elPi0"


            };
            int epochSize = 1;
            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] elMuPiIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, validFields, outputClasses);//{ new Interval(0, 500), new Interval(0, 70), new Interval(0, 150), new Interval(0, 120), new Interval(0, 1),
                                                                                                                       //new Interval(0, 70), new Interval(0, 10), new Interval(0, 10), new Interval(0,1), new Interval(0,5) };
            NNClassifier multiClassifier = new NNClassifier(validFields.Length, outputClasses.Length, new int[] { 13,13 }, new SigmoidFunction(1), elMuPiIntervals);
            return multiClassifier.Learn(epochSize, jsonFilePath, validFields, outputClasses, acceptableSuccessRate);
        }
        static double LearnAll(double acceptableSuccessRate)
        {

            string jsonFilePath = "../../../../ClusterDescriptionGen/bin/Debug/allTestData2.json";

            StreamReader inputStream = new StreamReader(jsonFilePath);
            NNInputProcessor preprocessor = new NNInputProcessor();


            var commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, validFields, outputClasses);
                // { new Interval(0, 20000), new Interval(0, 100), new Interval(0, 600), new Interval(0, 1000), new Interval(0, 1),
                                         // new Interval(0, 200), new Interval(0, 50), new Interval(0, 30), new Interval(0,1), new Interval(0,30) };

            // loop
            int epochSize = 256;
            NNClassifier commonRareClassifier = new NNClassifier(validFields.Length, outputClasses.Length, new int[] { 16, 16 }, new SigmoidFunction(1), commonRareIntervals);
            return commonRareClassifier.Learn(epochSize, jsonFilePath, validFields, outputClasses, acceptableSuccessRate);
        }
       
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            /*double maxSuccess = 0.75;
           for (int i = 0; i < 30; i++)
            {
                double success = LearnAll(maxSuccess);
                if (success > maxSuccess)
                    maxSuccess = success;
            }
            *//*
           var maxSuccess = 0.915;
            for (int i = 0; i < 50; i++)
            {
                double success = LearnFrag(maxSuccess);
                if (success > maxSuccess)
                    maxSuccess = success;
            }
            */
            /*maxSuccess = 0.97;
            for (int i = 0; i < 30; i++)
            {
                double success = LearnHePrFeLe_ElMuPi(maxSuccess);
                if (success > maxSuccess)
                    maxSuccess = success;
            }
            */
            var testJsonPath = "D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/allTestData2.json";
            int[][] ConfusionMatrix = ConfusionMatrix = new int[outputClasses.Length][];
            for (int i = 0; i < outputClasses.Length; i++)
            {
                ConfusionMatrix[i] = new int[outputClasses.Length];
            }
            StreamReader inputStream = new StreamReader(testJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            NNInputProcessor preprocessor = new NNInputProcessor();
            var _inputVector = preprocessor.ReadJsonToVector(jsonStream, validFields, outputClasses, out int _classIndex);
            MultiLayeredClassifier classifier = new MultiLayeredClassifier(outputClasses.Length);
            int[] classesCounts = new int[outputClasses.Length];
            int c = 0;
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < 0.2 * inputStream.BaseStream.Length && c < 3000)
            {
                var inputVector = preprocessor.ReadJsonToVector(jsonStream, validFields, outputClasses, out int classIndex);
                var resultIndex = classifier.Classify(inputVector);
                if (inputStream.BaseStream.Position < 0.3 * inputStream.BaseStream.Length && classesCounts[classIndex] < 300)
                {
                    ConfusionMatrix[classIndex][resultIndex]++;
                    classesCounts[classIndex]++;
                    c++;
                }
                
            }
            var totalSum = 0;
            var diagSum = 0;
            for (int j = 0; j < outputClasses.Length; j++)
            {

                for (int k = 0; k < outputClasses.Length; k++)
                {
                    Console.Write(ConfusionMatrix[j][k]);
                    Console.Write("\t");
                    totalSum += ConfusionMatrix[j][k];
                    if (j == k)
                        diagSum += ConfusionMatrix[j][k];


                }
                Console.WriteLine();
            }
            var successRate = diagSum / (double)totalSum;
            Console.WriteLine("Success rate is : " + successRate);
        /*
        var fragIntervals = new Interval[]{ new Interval(0, 6000), new Interval(0, 50), new Interval(0, 500), new Interval(0, 400), new Interval(0, 1),
                                   new Interval(0, 200), new Interval(0, 15), new Interval(0, 15), new Interval(0,1), new Interval(0,10) };
        Interval[] leadIntervals = { new Interval(0, 60000), new Interval(0, 100), new Interval(0, 600), new Interval(0, 1000), new Interval(0, 1),
                                      new Interval(0, 200), new Interval(0, 50), new Interval(0, 30), new Interval(0,1), new Interval(0,30) };
        Interval[] hePrFeLeIntervals = { new Interval(0, 5000), new Interval(0, 70), new Interval(0, 400), new Interval(0, 500), new Interval(0, 1),
                                      new Interval(0, 100), new Interval(0, 10), new Interval(0, 10), new Interval(0,1), new Interval(0,10) };
        Interval[] elMuPiIntervals = { new Interval(0, 500), new Interval(0, 70), new Interval(0, 150), new Interval(0, 120), new Interval(0, 1),
                                     new Interval(0, 70), new Interval(0, 10), new Interval(0, 10), new Interval(0,1), new Interval(0,5) };

            NNClassifier NNFrag = NNClassifier.LoadFromFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/fragBinary.jsontrained0.95BEST.txt");
            NNClassifier NNHePrFeLe = NNClassifier.LoadFromFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/HePrFeLe_ElMuPi.jsontrainedBEST.txt");
            NNClassifier NNLead = NNClassifier.LoadFromFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/leadBinary.jsontrained0.996BEST.txt");
            NNClassifier NNElMuPi = NNClassifier.LoadFromFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/ElMuPi.jsontrained 0.73BEST.txt");
            NNFrag.SqueezeIntervals = fragIntervals;
            NNHePrFeLe.SqueezeIntervals = hePrFeLeIntervals;
            NNLead.SqueezeIntervals = leadIntervals;
            NNElMuPi.SqueezeIntervals = elMuPiIntervals;
            NNFrag.StoreToFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/fragBinary.jsontrained0.95BEST.txt");
            NNHePrFeLe.StoreToFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/HePrFeLe_ElMuPi.jsontrainedBEST.txt");
              NNLead.StoreToFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/leadBinary.jsontrained0.996BEST.txt");
            NNElMuPi.StoreToFile("D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/ElMuPi.jsontrained 0.73BEST.txt");
          
            
            */
        //var _inputVector = preprocessor.ReadJsonToVector(jsonStream, validFields, outputClasses, out int _classIndex);


        //var iteration = 0;
        /*while (inputStream.BaseStream.Position < inputStream.BaseStream.Length)
        {



                //var inputVector = preprocessor.ReadJsonToVector(jsonStream, validFields, outputClasses, out int classIndex);
            //if(inputStream)


            // run epoch of learning procedure

            iteration++;
        }*/

    }
}
}

