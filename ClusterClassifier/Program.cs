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

        static double[] ReadJsonToVector(JsonTextReader reader, string[] usableKeys, string[] classes, out int classIndex)
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
                            if (usableKey == "MaxEnergy" || usableKey == "AverageEnergy" || usableKey == "TotalEnergy")
                                resultVector.Add(attributeVal/1000000d);
                            else
                                resultVector.Add(attributeVal);
                        }

                        }
                        const string ClassKey = "Class";
                        if (!jsonRecord.ContainsKey(ClassKey))
                            throw new ArgumentException($"Error, Required \"{ClassKey}\" property is not included in given input file");

                        classIndex = Array.IndexOf(classes, jsonRecord[ClassKey].ToString());
                        if (classIndex < 0)
                            throw new ArgumentException($"Error, Class value: \"{jsonRecord[ClassKey]}\" is not valid class value");
                        return resultVector.ToArray().Normalize();
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
            const string jsonFilePath = "../../../../ClusterDescriptionGen/bin/Debug/outTest.json";
            string[] outputClasses = new string[] { 
                "proton",
                "muon",
                "electron",
                "fe"
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
            var myNN = new MLP(new uint [] { 16, 16, 16}, costFunction);
            myNN.CreateInput(new double[10]);
            myNN.CreateOutput(4);

            ActivationNetwork network = new ActivationNetwork(
                new SigmoidFunction(2),
                validFields.Length, // two inputs in the network
                16, // two neurons in the first layer
                16,
                outputClasses.Length); // one neuron in the second layer
                    // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            var jsonStream = new JsonTextReader(inputStream);
            var _inputVector = ReadJsonToVector(jsonStream, validFields, outputClasses, out int _classIndex);
            
            // loop
            int i = 0;
            while (i < 500000 && inputStream.Peek() != -1)
            {
                double[][] input = new double[32][];
                double[][] output = new double[32][];
                
                for (int j = 0; j < 32; j++)
                {
                    var inputVector = ReadJsonToVector(jsonStream, validFields, outputClasses, out int classIndex);
                    var outputVector = new double[outputClasses.Length];
                    outputVector[classIndex] = 1;
                    input[j] = inputVector;
                    output[j] = outputVector;
                }
                var err = myNN.ProcessTrainingSet(input, output.ToList());

                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output);
                if (i % 10 == 0)
                    Console.WriteLine(error);
                if (i % 100 == 0)
                { }
                    // check error value to see if we need to stop
                    // ...
                    i++;
                
            }         
            /*var result = network.Compute(one);
            result = network.Compute(two);
            result = network.Compute(SwitchRandomVal(four));
            result = network.Compute(SwitchRandomVal(SwitchRandomVal(four)));
            
            const int testSetSize = 6;
            ICostFunction costFunction = new SquareDiffCostFunction();
            var myNN = new MLP(layerSizes, costFunction);
            double[] one = new double[] { 1, 0, 0, 1, 0, 0, 1, 0, 0 };
            double[] one2 = new double[] { 0, 1, 0, 0, 1, 0, 0, 1, 0 };
            double[] two = new double[] { 1, 1, 1, 0, 1, 0, 1, 1, 1 };
            double[] four = new double[] { 0, 1, 0, 0, 1, 1, 0, 0, 1 };
            myNN.CreateInput(new double[] { 1, 0, 0, 1, 0, 0, 1, 0, 0 });
            myNN.CreateOutput();
            Gradient gradient = new Gradient();
            for (int j = 0; j < 15000; j++)
            {
                if(j==14999)
                { }
                List<double[]> inputTrainSet = new List<double[]>();
                List<double[]> outputTrainSet = new List<double[]>();
                for (int i = 0; i < testSetSize; i++)
                {                
                    double[] input = new double[myNN.inputLayer.GetSize()];
                    double[] output = new double[myNN.outputLayer.GetSize()];
                    if (i % 3 == 0)
                    {
                        if (rand.NextDouble() < 0.3)
                            input = one2;
                         else
                          input = one;
                        
                        output = new double[] { 1, 0, 0 };
                    }
                    else if (i % 3 == 1)
                    {
                        if (rand.NextDouble() < 0.1)
                            myNN.SetInput(SwitchRandomVal(two));
                        else
                        input = two;
                        output = new double[] { 0, 1, 0 };
                    }
                    else
                    {

                        if (rand.NextDouble() < 0.1)
                            input = SwitchRandomVal(four);
                        else
                            input = four;
                            output = new double[] { 0, 0, 1 };
                    }
                    inputTrainSet.Add(input);
                    outputTrainSet.Add(output);

                }
                myNN.ProcessTrainingSet(inputTrainSet, outputTrainSet);

            }
            myNN.SetInput(two);
            myNN.Process();
            myNN.SetInput(one2);
            myNN.Process();
            myNN.SetInput(one);
            myNN.Process();
            myNN.SetInput(SwitchRandomVal(SwitchRandomVal(four)));
            myNN.Process();*/
        }
        }
}

