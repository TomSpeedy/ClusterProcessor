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
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
namespace ClusterClassifier
{
    struct Interval
    {
        public double Min { get; }

        public double Max { get; }
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
    }
    class Program
    {
        static Random rand = new Random();
        static double[] SwitchRandomVal(double[] input)
        {
            double[] result = new double[input.Length];
            input.CopyTo(result, 0);
            int swapIndex = rand.Next(0, input.Length - 1);
            result[swapIndex] = result[swapIndex] == 1d ? 0 : 1;
            return result;
        }

        static double[] ReadJsonToVector(JsonTextReader reader, string[] usableKeys, string[] classes,  out int classIndex)
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
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            /*
            double[] one = new double[] { 1, 0, 0, 1, 0, 0, 1, 0, 0 };
            double[] one2 = new double[] { 0, 1, 0, 0, 1, 0, 0, 1, 0 };
            double[] two = new double[] { 1, 1, 1, 0, 1, 0, 1, 1, 1 };
            double[] four = new double[] { 0, 1, 0, 0, 1, 1, 0, 0, 1 };
            uint[] layerSizes = { 16, 16, 16 };
            // initialize input and output values
            double[][] input = new double[6][];
            input[0] = rand.NextDouble() < 0.3 ? one2 : one;
            input[1] = rand.NextDouble() < 0.1 ? SwitchRandomVal(two) : two;
            input[2] = rand.NextDouble() < 0.1 ? SwitchRandomVal(four) : four;
            input[3] = rand.NextDouble() < 0.3 ? one2 : one;
            input[4] = rand.NextDouble() < 0.1 ? SwitchRandomVal(two) : two;
            input[5] = rand.NextDouble() < 0.1 ? SwitchRandomVal(four) : four;
        
        double[][] output = new double[6][] {
   new double[] { 1, 0, 0 }, new double[] { 0, 1, 0 },
    new double[] { 0, 0, 1 }, new double[] { 1, 0, 0 }, new double[] { 0, 1, 0 }, new double[] { 0, 0, 1 } };
    */
            // create neural network
            const string jsonFilePath = "../../../../ClusterDescriptionGen/bin/Debug/fiveCategories.json";
            string[] outputClasses = new string[] {
                "proton",
                "he",
                "low_electron",
                "electron",
                "muon"

            };

            StreamReader inputStream = new StreamReader(jsonFilePath);
            var validFields = new string[]{
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
                };
            ICostFunction costFunction = new SquareDiffCostFunction();
            var myNN = new MLP(new uint[] { 20, 20 }, costFunction);
            myNN.CreateInput(new double[10]);
            myNN.CreateOutput((uint)outputClasses.Length);

            ActivationNetwork network = new ActivationNetwork(
                new SigmoidFunction(1),
                validFields.Length, 
                new int[] {16, 16, outputClasses.Length}
                ); // one neuron in the second layer
                    // create teacher
           BackPropagationLearning teacher = new BackPropagationLearning(network);
            teacher.Momentum = 0.5;
            teacher.LearningRate = 0.1;
            var jsonStream = new JsonTextReader(inputStream);
            var _inputVector = ReadJsonToVector(jsonStream, validFields, outputClasses, out int _classIndex);
            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] inputIntervals = { new Interval(0, 1500), new Interval(0, 100), new Interval(0, 500), new Interval(0, 100), new Interval(0, 1),
                                         new Interval(0, 100), new Interval(0, 10), new Interval(0, 10), new Interval(0,1), new Interval(0,10) };
            // loop
            int i = 0;
            const int epochSize = 32;
            
            while (i < 30000 && inputStream.Peek() != -1)
            {
                double[][] input = new double[epochSize][];
                double[][] output = new double[epochSize][];
                
                for (int j = 0; j < epochSize; j++)
                {
                    var inputVector = preprocessor.NormalizeElements(ReadJsonToVector(jsonStream, validFields, outputClasses, out int classIndex),inputIntervals);
                    var outputVector = new double[outputClasses.Length];
                    outputVector[classIndex] = 1;
                    input[j] = inputVector;
                    output[j] = outputVector;
                }
               // tree = teach.Learn(input, output)
                //var err = myNN.ProcessTrainingSet(input, output.ToList());

                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output);
                
                if (i % 10 == 0)

                    Console.WriteLine(error);
                //if (i % 10000 == 0)
                //{ }
                    // check error value to see if we need to stop
                    // ...
                    i++;
                if (i >= 10000)
                { }
            }
            double[][] inp = new double[16][];
            double[][] outp = new double[16][];
            for (int j = 0; j < 16; j++)
            {
                
                var inputVector = preprocessor.NormalizeElements(ReadJsonToVector(jsonStream, validFields, outputClasses, out int classIndex), inputIntervals);
                var outputVector = new double[outputClasses.Length];
                outputVector[classIndex] = 1;
                inp[j] = inputVector;
                outp[j] = outputVector;
                var result = network.Compute(inp[j]);
            }
            var erro = myNN.ProcessTrainingSet(inp, outp.ToList());
            
        }
        }
}

