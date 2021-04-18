using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines.Learning;
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
    [Serializable]
    public struct Interval
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

    public class NNInputProcessor
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
        public Interval[] CalculateNormIntervals(string jsonPath, ClusterAttribute[] usableKeys, string[] classes)
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
                var numVector = ReadTrainJsonToVector(jReader, usableKeys, classes, out int classIndex);
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
        public double[] ReadTrainJsonToVector(JsonTextReader reader, ClusterAttribute[] usableKeys, string[] classes, out int classIndex)
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
                        if (!jsonRecord.ContainsKey(usableKey.ToString()))
                            throw new ArgumentException($"Error, Required property \"{usableKey}\" for training set is not included in given input file");

                        if (double.TryParse(jsonRecord[usableKey.ToString()].ToString(), out attributeVal))
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
        public double[] ReadJsonToVector(JsonTextReader reader, ClusterAttribute[] usableKeys)
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
                        if (!jsonRecord.ContainsKey(usableKey.ToString()))
                            throw new ArgumentException($"Error, Required property \"{usableKey}\" for training set is not included in given input file");

                        if (double.TryParse(jsonRecord[usableKey.ToString()].ToString(), out attributeVal))
                        {
                            resultVector.Add(attributeVal);
                        }

                    }

                    return resultVector.ToArray();
                }
            }
            return null;
        }
    }
    public interface IClassifier
    {
        void Load();
        ClassPrediction Classify(double[] inputVector);
        ClusterAttribute[] ValidFields { get; }
        string[] OutputClasses { get; }
    }
    public class ClassPrediction
    {
        //public int MostProbableClassIndex { get; private set; }
        public string MostProbableClassName { get; private set; }
        //public double[] Probabilities { get; private set; }
        //public string[] Classes { get; private set; }
        public Dictionary<string, double> ClassProbabilities { get; private set; } = new Dictionary<string, double>();
        public ClassPrediction(double[] outputVector, string[] classes)
        {

            var probabilities = Special.Softmax(outputVector);
            for (int i = 0; i < classes.Length; i++)
                ClassProbabilities.Add(classes[i], probabilities[i]);
            double maxProb = 0;
            string mostProbableClass = null;
            foreach (KeyValuePair<string, double> pair in ClassProbabilities)
            {
                if (pair.Value > maxProb)
                {
                    maxProb = pair.Value;
                    mostProbableClass = pair.Key;
                }
            }
            MostProbableClassName = mostProbableClass;
        }
        public ClassPrediction CombineWith(ClassPrediction sonPrediction, string parentClass, string[] classesToIgnore = null)
        {
            if (classesToIgnore == null)
                classesToIgnore = new string[0];

            if (parentClass != MostProbableClassName && !classesToIgnore.Contains(MostProbableClassName))
            {
                return this;
            }
            return sonPrediction;
            /*foreach (var classToIgnore in classesToIgnore)
            {
                ClassProbabilities[parentClass] += ClassProbabilities[classToIgnore];
            }
            foreach (var parentKey in ClassProbabilities.Keys)
            {
                if (parentKey != parentClass && !classesToIgnore.Contains(parentKey))
                    result.ClassProbabilities.Add(parentKey, ClassProbabilities[parentKey]);           
            }
            foreach (var sonKey in sonPrediction.ClassProbabilities.Keys)
            {
                result.ClassProbabilities.Add(sonKey, sonPrediction.ClassProbabilities[sonKey] * ClassProbabilities[parentClass]);
            }
            double maxProb = 0;
            string mostProbableClass = null;
            foreach (KeyValuePair<string, double> pair in result.ClassProbabilities)
            {
                if (pair.Value >  maxProb)
                {
                    maxProb = pair.Value;
                    mostProbableClass = pair.Key;
                }
            }
            result.MostProbableClassName = mostProbableClass;*/
        }
        private ClassPrediction()
        {
        }

    }
    public class NNClassifier
    {
        DeepBeliefNetwork Network { get; set; }
        public Interval[] SqueezeIntervals { get; private set; }
        public double TrainRatio { get; set; } = 0.9;
        public int PrintInterval { get; set; } = 50;
        public bool Silent { get; set; } = false;
        public bool Evaluate { get; set; } = true;
        public string[] Classes { get; private set; }
        public DeepNeuralNetworkLearning Teacher { get; set; }
        private NNClassifier() { }
        private void SetDefaultTeacher()
        {
            Teacher = new DeepNeuralNetworkLearning(Network);
            Teacher.Algorithm = (activationNetwork, index) =>
            {

                var backProp = new BackPropagationLearning(activationNetwork);
                backProp.Momentum = 0.5;
                backProp.LearningRate = 0.6;
                return backProp;
            };
            Teacher.LayerCount = Network.Layers.Length;
            Teacher.LayerIndex = 0;
        }
        public NNClassifier(int inputLen, int outputLen, int[] layerSizes, IActivationFunction activationFunction, Interval[] squeezeIntervals, string[] outputClasses)
        {
            var layerNewSizes = layerSizes.Append(outputLen).ToArray();
            Network = new DeepBeliefNetwork(inputLen, layerNewSizes);
            Network.SetActivationFunction(activationFunction);
            SqueezeIntervals = squeezeIntervals;
            SetDefaultTeacher();
            Classes = outputClasses;

        }
        public NNClassifier(DeepBeliefNetwork network, IActivationFunction activationFunction, Interval[] squeezeIntervals, string[]outputClasses)
        {
            Network = network;
            Network.SetActivationFunction(activationFunction);
            SqueezeIntervals = squeezeIntervals;
            Classes = outputClasses;
            SetDefaultTeacher();
        }
        public double Learn(int epochSize, string learnJsonPath, ClusterAttribute[] validProperties, double successThreshold)
        {
            var inputStream = new StreamReader(learnJsonPath);
            var jsonStream = new JsonTextReader(inputStream);

            NNInputProcessor preprocessor = new NNInputProcessor();
            //empty read
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, validProperties, Classes, out int _classIndex);
            var iteration = 0;
            double errorSum = 0;
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length * TrainRatio)
            {
                double[][] input = new double[epochSize][];
                double[][] output = new double[epochSize][];

                for (int j = 0; j < epochSize; j++)
                {
                    var inputVector = preprocessor.NormalizeElements(preprocessor.ReadTrainJsonToVector(jsonStream, validProperties, Classes, out int classIndex), SqueezeIntervals);
                    var outputVector = new double[Classes.Length];
                    outputVector[classIndex] = 1;
                    input[j] = inputVector;
                    output[j] = outputVector;
                }
                // run epoch of learning procedure
                double error = Teacher.RunEpoch(input, output);
                errorSum += error;
                if (iteration % PrintInterval == 0)
                {
                    if (!Silent)
                        Console.WriteLine(errorSum);
                    errorSum = 0;
                }
                iteration++;
            }
            return Eval(learnJsonPath, validProperties, Classes, preprocessor, successThreshold);
        }
        private double Eval(string learnJsonPath, ClusterAttribute[] validProperties, string[] outputClasses, NNInputProcessor preprocessor, double successThreshold)
        {
            int[][] confusionMatrix = new int[outputClasses.Length][];
            for (int j = 0; j < outputClasses.Length; j++)
            {
                confusionMatrix[j] = new int[outputClasses.Length];
            }
            var inputStream = new StreamReader(learnJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            //initial empty read
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, validProperties, outputClasses, out int ___classIndex);
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length * TrainRatio)
            {
                var unNormalizedVect = preprocessor.ReadTrainJsonToVector(jsonStream, validProperties, outputClasses, out int classIndex);
                var inputVector = preprocessor.NormalizeElements(unNormalizedVect, SqueezeIntervals);
                var outputVector = new double[outputClasses.Length];
                outputVector[classIndex] = 1;

                var result = Network.Compute(inputVector);
                var prediction = new ClassPrediction(result, outputClasses).MostProbableClassName;
                confusionMatrix[classIndex][outputClasses.IndexOf(prediction)]++;

            }
            var totalSum = 0;
            var diagSum = 0;
            for (int j = 0; j < outputClasses.Length; j++)
            {

                for (int k = 0; k < outputClasses.Length; k++)
                {
                    if (!Silent)
                    {
                        Console.Write(confusionMatrix[j][k]);
                        Console.Write("\t");
                    }
                    totalSum += confusionMatrix[j][k];
                    if (j == k)
                        diagSum += confusionMatrix[j][k];


                }
                if (!Silent)
                    Console.WriteLine();
            }
            var successRate = diagSum / (double)totalSum;
            if (!Silent)
                Console.WriteLine("Success rate is : " + successRate);
            if (successRate >= successThreshold)
                StoreToFile(learnJsonPath + "trained " + successRate + ".txt");
            return successRate;
        }
        public void StoreToFile(string outJsonPath)
        {
            Network.Save(outJsonPath);

            StreamWriter writer = new StreamWriter(outJsonPath + "_intervals");
            string intervalsString = System.Text.Json.JsonSerializer.Serialize(SqueezeIntervals);
            string classesString = System.Text.Json.JsonSerializer.Serialize(Classes);
            writer.Write(intervalsString.Substring(0, intervalsString.Length - 1));
            writer.Write("," + classesString.Substring(1));
            writer.Close();
        }
        public static NNClassifier LoadFromFile(string inJsonPath)
        {
            var classifier = new NNClassifier();
            classifier.Network = (DeepBeliefNetwork) DeepBeliefNetwork.Load(inJsonPath);
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
            classifier.Classes = new string[classifier.Network.OutputCount];
            for (int i = 0; i < classifier.Network.OutputCount; i++)
            {
                jReader.Read();
                JToken classRecord = JToken.Load(jReader);
                classifier.Classes[i] = classRecord.ToString();
            }
            
            jReader.Close();
            sReader.Close();
            return classifier;
        }
        public ClassPrediction ClassifySingle(double[] inputVector)
        {
            NNInputProcessor preprocessor = new NNInputProcessor();
            var resultVector = Network.Compute(preprocessor.NormalizeElements(inputVector,SqueezeIntervals));
            return new ClassPrediction(resultVector, Classes);
        }
        

    }
    public class MultiLayeredClassifier : IClassifier
    {
        NNClassifier NNFragFeHe { get; set; }
        NNClassifier NNPrLe_ElMuPi { get; set; }
        NNClassifier NNLead { get; set; }
        NNClassifier NNElMuPi { get; set; }
        SupportVectorMachine<Gaussian> Svm { get; set; }
        public ClusterAttribute[] ValidFields { get; } = new ClusterAttribute[]{
                 ClusterAttribute.TotalEnergy,
                ClusterAttribute.AverageEnergy,
                 ClusterAttribute.MaxEnergy,
                ClusterAttribute. PixelCount,
                 ClusterAttribute.Convexity,
                 ClusterAttribute.Width,
                 ClusterAttribute.CrosspointCount,
                 ClusterAttribute.VertexCount,
                 ClusterAttribute.RelativeHaloSize,
                 ClusterAttribute.BranchCount,
                 ClusterAttribute.StdOfEnergy,
                ClusterAttribute.StdOfArrival,
                ClusterAttribute.RelLowEnergyPixels
                 };
        public string[] OutputClasses { get; } = new string[] {
            "lead",
            "frag",
            "he",
            "proton",
            "fe",
            "low_electr",
            "muon",
            "electron",
            "pion",
            "elPi0"
        };
        public MultiLayeredClassifier()
        {

        }
        public void TestSvm()
        {
            string[] outputClasses = new string[] {
                 "muon",
                 "electron",
                 "pion",
                 "elPi0"
            };
            var testJsonPath = "../../../ClusterDescriptionGen/bin/Debug/trainElMuPi.json";
            int[][] ConfusionMatrix = ConfusionMatrix = new int[outputClasses.Length][];
            for (int i = 0; i < outputClasses.Length; i++)
            {
                ConfusionMatrix[i] = new int[outputClasses.Length];
            }
            StreamReader inputStream = new StreamReader(testJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            NNInputProcessor preprocessor = new NNInputProcessor();
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, outputClasses, out int _classIndex);
            int[] classesCounts = new int[outputClasses.Length];
            int c = 0;
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < 0.1 * inputStream.BaseStream.Length/* && c < 3000*/)
            {
                var inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, outputClasses, out int classIndex);
                //var resultClass = Classify(inputVector).MostProbableClassName;
                if (/*inputStream.BaseStream.Position > 0.5 * inputStream.BaseStream.Length && classesCounts[classIndex] < 300*/true)
                {
                    var isMuon = Svm.Decide(inputVector);
                    var expectedClassIndex = isMuon ? 0 : 1;
                    classIndex = classIndex > 0 ? 1 : 0;
                    ConfusionMatrix[classIndex][expectedClassIndex]++;
                    classesCounts[classIndex]++;
                    c++;
                }

            }
            var rowSums = ConfusionMatrix.Select(x => x.Sum()).ToArray();
            var totalSum = 0;
            var diagSum = 0;
            for (int j = 0; j < outputClasses.Length; j++)
            {

                for (int k = 0; k < outputClasses.Length; k++)
                {
                    Console.Write(Math.Truncate(1000d * ConfusionMatrix[j][k] / (double)rowSums[j]) / 10d);
                    Console.Write("\t");
                    totalSum += ConfusionMatrix[j][k];
                    if (j == k)
                        diagSum += ConfusionMatrix[j][k];


                }
                Console.WriteLine();
            }
            var successRate = diagSum / (double)totalSum;
            Console.WriteLine("Success rate is : " + successRate);
        }
        public void LearnSvm()
        {
            string jsonFilePath = "../../../ClusterDescriptionGen/bin/Debug/trainElMuPi.json";
            string[] outputClasses = new string[] {
                 "muon",
                 "electron",
                 "pion",
                 "elPi0"
            };
            var learn = new SequentialMinimalOptimization<Gaussian>()
            {
                UseComplexityHeuristic = true,
                UseKernelEstimation = true
            };
            const int dataSize = 10000;
            double[][] input = new double[dataSize][];
            bool[] output = new bool[dataSize];
            NNInputProcessor preprocessor = new NNInputProcessor();
            var inputStream = new StreamReader(jsonFilePath);
            var jsonStream = new JsonTextReader(inputStream);
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, outputClasses, out int _classIndex);
            for (int j = 0; j < dataSize; j++)
            {
                var inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, outputClasses, out int classIndex);
                //var outputVector = new double[outputClasses.Length];

                //outputVector[classIndex] = 1;
                input[j] = inputVector;
                output[j] = classIndex == 0 ? true : false;
            }
            Svm = learn.Learn(input, output);
        }
        public void Load()
        {
            /*NNFragFeHe = NNClassifier.LoadFromFile("../../../ClusterDescriptionGen/bin/Debug/trainFragHeFe.jsontrained 0.94892705185916.txt");
            NNPrLe_ElMuPi = NNClassifier.LoadFromFile("../../../ClusterDescriptionGen/bin/Debug/BESTtrainPrLe_ElMuPi.jsontrained 0.966.txt");
            NNLead = NNClassifier.LoadFromFile("../../../ClusterDescriptionGen/bin/Debug/BESTtrainLeadMulti.jsontrained 0.894.txt");
            NNElMuPi = NNClassifier.LoadFromFile("../../../ClusterDescriptionGen/bin/Debug/BESTtrainElMuPi.jsontrained 0.802.txt");*/
            NNFragFeHe = NNClassifier.LoadFromFile("../../../ClusterDescriptionGen/bin/Debug/trainFragHeFeNew.jsontrained 0.956999028588936.txt");
            NNPrLe_ElMuPi = NNClassifier.LoadFromFile("../../../ClusterDescriptionGen/bin/Debug/trainPrLe_ElMuPi.jsontrained 0.966647243937912.txt");
            NNLead = NNClassifier.LoadFromFile("../../../ClusterDescriptionGen/bin/Debug/trainLeadMulti.jsontrained 0.909667030881592.txt");
            NNElMuPi = NNClassifier.LoadFromFile("../../../ClusterDescriptionGen/bin/Debug/trainElMuPi.jsontrained 0.801654638267911.txt");
            //LearnSvm();
            //TestSvm(); 

        }
        public double LearnFrag(double acceptableSuccessRate)
        {
            var jsonFilePath = "../../../ClusterDescriptionGen/bin/Debug/trainFragHeFeNew.json";
            var outputClasses = new string[] {
                 "frag",
                 "he",
                 "fe",
                 "other"
             };
            var inputStream = new StreamReader(jsonFilePath);
            NNInputProcessor preprocessor = new NNInputProcessor();
            var commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields, outputClasses);

            var epochSize = 8;
            NNClassifier fragClassifier = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), commonRareIntervals, outputClasses);
            fragClassifier.TrainRatio = 0.9;
            double success = 0;
            success = fragClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
            if (success > acceptableSuccessRate)
                return success;
            success = fragClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
            if (success > acceptableSuccessRate)
                return success;
            success = fragClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
            if (success > acceptableSuccessRate)
                return success;
            success = fragClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
            if (success > acceptableSuccessRate)
                return success;
            return fragClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
        }
        public double LearnLead(double acceptableSuccessRate)
        {

            string jsonFilePath = "../../../ClusterDescriptionGen/bin/Debug/trainLeadMulti.json";
            string[] outputClasses = new string[] {
                 "lead",
                  "he",
                 "fe",
                 "frag",
                 "other"
             };

            StreamReader inputStream = new StreamReader(jsonFilePath);
            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields, outputClasses);
            int epochSize = 32;
            NNClassifier commonRareClassifier = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), commonRareIntervals, outputClasses);
            return commonRareClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
        }
        public double LearnPrLe_ElMuPi(double acceptableSuccessRate)
        {

            string jsonFilePath = "../../../ClusterDescriptionGen/bin/Debug/trainPrLe_ElMuPi.json";
            string[] outputClasses = new string[] {
                 "proton",
                 "elMuPi",
                 "low_electr",
            };

            StreamReader inputStream = new StreamReader(jsonFilePath);


            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields, outputClasses);

            // loop
            int epochSize = 6;
            NNClassifier multiClassifier = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), commonRareIntervals, outputClasses);
            return multiClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
        }
        public double LearnElMuPi(double acceptableSuccessRate)
        {

            string jsonFilePath = "../../../ClusterDescriptionGen/bin/Debug/trainElMuPi.json";
            string[] outputClasses = new string[] {
                 "muon",
                 "electron",
                 "pion",
                 "elPi0"
            };
            int epochSize = 8;
            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] elMuPiIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields, outputClasses);
            NNClassifier multiClassifier = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 10, 10 }, new SigmoidFunction(1), elMuPiIntervals, outputClasses);
            multiClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
            return multiClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
        }
        double LearnAll(double acceptableSuccessRate)
        {

            string jsonFilePath = "../../../ClusterDescriptionGen/bin/Debug/allTestData2.json";

            StreamReader inputStream = new StreamReader(jsonFilePath);
            NNInputProcessor preprocessor = new NNInputProcessor();


            var commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields, OutputClasses);

            // loop
            int epochSize = 256;
            NNClassifier commonRareClassifier = new NNClassifier(ValidFields.Length, OutputClasses.Length, new int[] { 16, 16 }, new SigmoidFunction(1), commonRareIntervals, OutputClasses);
            return commonRareClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
        }
        public void TestModel()
        {
            var testJsonPath = "../../../ClusterDescriptionGen/bin/Debug/testAll.json";
            int[][] ConfusionMatrix = ConfusionMatrix = new int[OutputClasses.Length][];
            for (int i = 0; i < OutputClasses.Length; i++)
            {
                ConfusionMatrix[i] = new int[OutputClasses.Length];
            }
            StreamReader inputStream = new StreamReader(testJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            NNInputProcessor preprocessor = new NNInputProcessor();
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int _classIndex);
            int[] classesCounts = new int[OutputClasses.Length];
            int c = 0;
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < 1 * inputStream.BaseStream.Length/* && c < 3000*/)
            {
                var inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int classIndex);
                var resultClass = Classify(inputVector).MostProbableClassName;
                if (/*inputStream.BaseStream.Position > 0.5 * inputStream.BaseStream.Length && classesCounts[classIndex] < 300*/true)
                {
                    ConfusionMatrix[classIndex][OutputClasses.IndexOf(resultClass)]++;
                    classesCounts[classIndex]++;
                    c++;
                }

            }
            var rowSums = ConfusionMatrix.Select(x => x.Sum()).ToArray();
            var totalSum = 0;
            var diagSum = 0;
            for (int j = 0; j < OutputClasses.Length; j++)
            {

                for (int k = 0; k < OutputClasses.Length; k++)
                {
                    Console.Write(Math.Truncate(1000d * ConfusionMatrix[j][k] / (double)rowSums[j]) / 10d);
                    Console.Write("\t");
                    totalSum += ConfusionMatrix[j][k];
                    if (j == k)
                        diagSum += ConfusionMatrix[j][k];


                }
                Console.WriteLine();
            }
            var successRate = diagSum / (double)totalSum;
            Console.WriteLine("Success rate is : " + successRate);
        }


        public ClassPrediction Classify(double[] inputVector)
        {
            var leadPrediction = NNLead.ClassifySingle(inputVector);
            var fragPrediction = NNFragFeHe.ClassifySingle(inputVector);
            var prLePrediction = NNPrLe_ElMuPi.ClassifySingle(inputVector);
            var elMuPiPredition = NNElMuPi.ClassifySingle(inputVector);
            const string otherClass = "other";
            var treePrediction = leadPrediction.CombineWith(fragPrediction, otherClass, new string[] { "he", "fe", "frag" }).CombineWith(prLePrediction, otherClass).CombineWith(elMuPiPredition, "elMuPi");
            return treePrediction;
        }
        public Dictionary<string, int> ClassifyCollection(string testJsonPath)
        {
            var classHistogram = new Dictionary<string, int>(); 
            
            int[][] ConfusionMatrix = ConfusionMatrix = new int[OutputClasses.Length][];
            for (int i = 0; i < OutputClasses.Length; i++)
            {
                ConfusionMatrix[i] = new int[OutputClasses.Length];
                classHistogram.Add(OutputClasses[i], 0);
            }
            StreamReader inputStream = new StreamReader(testJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            NNInputProcessor preprocessor = new NNInputProcessor();
            var _inputVector = preprocessor.ReadJsonToVector(jsonStream, ValidFields);
            int[] classesCounts = new int[OutputClasses.Length];
            int c = 0;
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < 1* inputStream.BaseStream.Length/* && c < 3000*/)
            {
                var inputVector = preprocessor.ReadJsonToVector(jsonStream, ValidFields);
                var resultClass = Classify(inputVector).MostProbableClassName;
                if (/*inputStream.BaseStream.Position > 0.5 * inputStream.BaseStream.Length && classesCounts[classIndex] < 300*/true)
                {
                    classHistogram[resultClass]++;
                    //classesCounts[classIndex]++;
                    c++;
                }

            }
            return classHistogram;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            MultiLayeredClassifier classifier = new MultiLayeredClassifier();
            classifier.Load();
            classifier.TestModel();

            var histo = classifier.ClassifyCollection("../../../ClusterDescriptionGen/bin/Debug/testAtlas.json");
            foreach(var pair in histo)
            {
                Console.WriteLine(pair.Key + ":" + pair.Value);

            }
           /* double maxSuccess = 0.956;
            for (int i = 0; i < 30; i++)
            {
                var success = classifier.LearnFrag(maxSuccess);
                if (success > maxSuccess)
                    maxSuccess = success;
            }*/
            
            //TestModel();
            /*
             * var learn = new SequentialMinimalOptimization<Gaussian>()
            {
            UseComplexityHeuristic = true,
            UseKernelEstimation = true
            };
             SupportVectorMachine<Gaussian> svm = learn.Learn(inputs, xor);

            // Finally, we can obtain the decisions predicted by the machine:
            bool[] prediction = svm.Decide(inputs);
             
             */

        }
        static void LearnSVM()
        {
            
        }
}
}


