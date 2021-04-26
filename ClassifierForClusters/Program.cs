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
        public string[] FindAllClasses(string jsonPath)
        {
            StreamReader sReader = new StreamReader(jsonPath);
            JsonTextReader jReader = new JsonTextReader(sReader);
            jReader.Read();
            List<string> classes = new List<string>();
            const string classKey = "Class";
            while (sReader.Peek() != -1)
            {
                jReader.Read();
                JObject jsonRecord = JObject.Load(jReader);
                if (!jsonRecord.ContainsKey(classKey))
                    throw new ArgumentException($"Error, Required \"{classKey}\" property is not included in given input file");

                string currentClass = jsonRecord[classKey].ToString();
                if (!classes.Contains(currentClass))
                    classes.Add(currentClass);
            }
            return classes.ToArray();
        }
        public Interval[] CalculateNormIntervals(string jsonPath, ClusterAttribute[] usableKeys)
        {
            StreamReader sReader = new StreamReader(jsonPath);
            JsonTextReader jReader = new JsonTextReader(sReader);
            var squeezeIntervals = new Interval[usableKeys.Length];
            var _numVector = ReadJsonToVector(jReader, usableKeys);
            for (int i = 0; i < squeezeIntervals.Length; i++)
            {
                squeezeIntervals[i] = new Interval(double.MaxValue, double.MinValue);
            }
            while (sReader.Peek() != -1)
            {
                var numVector = ReadJsonToVector(jReader, usableKeys);
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
        void Load(string[] filePaths);
        ClassPrediction Classify(double[] inputVector);
        ClusterAttribute[] ValidFields { get; }
        string[] OutputClasses { get; }
    }
    public class ClassPrediction
    {
        public string MostProbableClassName { get; set; }

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
        public ClassPrediction CombineWith(ClassPrediction sonPrediction, string splittingClass, string[] classesToIgnore = null)
        {
            if (classesToIgnore == null)
                classesToIgnore = new string[0];
            if (splittingClass != MostProbableClassName && !classesToIgnore.Contains(MostProbableClassName))
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
        public double CalcConfidence()
        {            
            var maximumProb = ClassProbabilities[MostProbableClassName];
            var newProbabilities = ClassProbabilities.Values.ToList();
            newProbabilities.Sort();
            var secondMaxProb = newProbabilities[ClassProbabilities.Keys.Count() - 2];
            return (maximumProb - secondMaxProb) / maximumProb;
        }

        private ClassPrediction()
        {
        }

    }
    public class NNClassifier
    {
        DeepBeliefNetwork Network { get; set; }
        public string Name { get; set; }
        public Interval[] SqueezeIntervals { get; private set; }
        public Interval TrainInterval { get; set; } = new Interval(0, 1);
        public Interval TestInterval { get; set; } = new Interval(0, 1);
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
        public NNClassifier(int inputLen, int outputLen, int[] layerSizes, IActivationFunction activationFunction, Interval[] squeezeIntervals, string[] outputClasses, string name)
        {
            var layerNewSizes = layerSizes.Append(outputLen).ToArray();
            Network = new DeepBeliefNetwork(inputLen, layerNewSizes);
            Network.SetActivationFunction(activationFunction);
            SqueezeIntervals = squeezeIntervals;
            SetDefaultTeacher();
            Classes = outputClasses;
            Name = name;

        }
        public NNClassifier(DeepBeliefNetwork network, IActivationFunction activationFunction, Interval[] squeezeIntervals, string[]outputClasses, string name)
        {
            Network = network;
            Network.SetActivationFunction(activationFunction);
            SqueezeIntervals = squeezeIntervals;
            Classes = outputClasses;
            SetDefaultTeacher();
            Name = name;
        }
        public NNClassifier(int inputLen, int outputLen, int[] layerSizes, IActivationFunction activationFunction, string name)
        {
            var layerNewSizes = layerSizes.Append(outputLen).ToArray();
            Network = new DeepBeliefNetwork(inputLen, layerNewSizes);
            Network.SetActivationFunction(activationFunction);
            SetDefaultTeacher();
            Name = name;
        }
        public void SetClassesAndIntervalsFromData(string learnFile, ClusterAttribute[] validProperties)
        {
            NNInputProcessor preprocessor = new NNInputProcessor();
            SqueezeIntervals = preprocessor.CalculateNormIntervals(learnFile, validProperties);
            Classes = preprocessor.FindAllClasses(learnFile);
        }
        public double Learn(int epochSize, string learnJsonPath, ClusterAttribute[] validProperties, double successThreshold, bool eval = true)
        {
            var inputStream = new StreamReader(learnJsonPath);
            var jsonStream = new JsonTextReader(inputStream);

            NNInputProcessor preprocessor = new NNInputProcessor();
            //empty read
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, validProperties, Classes, out int _classIndex);
            var iteration = 0;
            double errorSum = 0;
            bool done = false;
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length * TrainInterval.Max && !done )
            {
                
                double[][] input = new double[epochSize][];
                double[][] output = new double[epochSize][];

                for (int j = 0; j < epochSize; j++)
                {
                    var originVector = preprocessor.ReadTrainJsonToVector(jsonStream, validProperties, Classes, out int classIndex);
                    if (originVector == null)
                    {
                        done = true;
                        break;
                    }
                    var inputVector = preprocessor.NormalizeElements(originVector, SqueezeIntervals);
                    var outputVector = new double[Classes.Length];
                    outputVector[classIndex] = 1;
                    input[j] = inputVector;
                    output[j] = outputVector;
                }

                if (inputStream.BaseStream.Position < inputStream.BaseStream.Length * TrainInterval.Min || done)
                    continue;

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
            if (!eval)
                return -1;
            return Eval(learnJsonPath, validProperties, preprocessor, successThreshold);
        }
        private double Eval(string learnJsonPath, ClusterAttribute[] validProperties, NNInputProcessor preprocessor, double successThreshold)
        {
            int[][] confusionMatrix = new int[Classes.Length][];
            for (int j = 0; j < Classes.Length; j++)
            {
                confusionMatrix[j] = new int[Classes.Length];
            }
            var inputStream = new StreamReader(learnJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            //initial empty read
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, validProperties, Classes, out int ___classIndex);
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length * TestInterval.Max)
            {
                var unNormalizedVect = preprocessor.ReadTrainJsonToVector(jsonStream, validProperties, Classes, out int classIndex);
                if (unNormalizedVect == null)
                {
                    break;
                }
                var inputVector = preprocessor.NormalizeElements(unNormalizedVect, SqueezeIntervals);
                if (inputStream.BaseStream.Position > inputStream.BaseStream.Length * TestInterval.Min)
                {
                    var outputVector = new double[Classes.Length];
                    outputVector[classIndex] = 1;

                    var result = Network.Compute(inputVector);
                    var prediction = new ClassPrediction(result, Classes).MostProbableClassName;
                    confusionMatrix[classIndex][Classes.IndexOf(prediction)]++;
                }

            }
            var totalSum = 0;
            var diagSum = 0;
            for (int j = 0; j < Classes.Length; j++)
            {

                for (int k = 0; k < Classes.Length; k++)
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
            {
                successRate = (Math.Truncate(successRate * 1000)) / 1000d;
                StoreToFile(learnJsonPath + "_trained " + successRate + ".txt");
            }
            return successRate;
        }
        public void StoreToFile(string outJsonPath)
        {
            Network.Save(outJsonPath);

            StreamWriter writer = new StreamWriter(outJsonPath + "_support");
            string intervalsString = System.Text.Json.JsonSerializer.Serialize(SqueezeIntervals);
            string classesString = System.Text.Json.JsonSerializer.Serialize(Classes);
            writer.Write(intervalsString.Substring(0, intervalsString.Length - 1));
            writer.Write("," + System.Text.Json.JsonSerializer.Serialize(Name));
            writer.Write("," + classesString.Substring(1));
            writer.Close();
        }
        /// <summary>
        /// Calculates the K-fold cross validation - expects the data to be ordered randomly
        /// </summary>
        /// <param name="kFoldCount"></param>
        /// <param name="epochSize"></param>
        /// <param name="learnJsonPath"></param>
        /// <param name="validProperties"></param>
        /// <param name="successThreshold"></param>
        /// <returns></returns>
        public double[] CrossValidate(int kFoldCount, int epochSize, string learnJsonPath, ClusterAttribute[] validProperties,int learnIterationsCount = 1)
        {
            double[] accuracies = new double[kFoldCount];
            const double Epsilon = 0.00001;
            var originInterval = TrainInterval;
            for (int i = 0; i < kFoldCount; i++)
            {              
                for (int learnIteration = 0; learnIteration < learnIterationsCount; learnIteration++)
                {
                    TrainInterval = new Interval(originInterval.Min, (i / (double)kFoldCount) * originInterval.Size + originInterval.Min);
                    if (Math.Abs(TrainInterval.Min - TrainInterval.Max) > Epsilon)
                        Learn(epochSize, learnJsonPath, validProperties, 0, eval: false);
                    TrainInterval = new Interval(((i + 1) / (double)kFoldCount) * originInterval.Size + originInterval.Min, originInterval.Max);
                    if (Math.Abs(TrainInterval.Min - TrainInterval.Max) > Epsilon)
                        Learn(epochSize, learnJsonPath, validProperties, 0, eval: false);
                }
                TestInterval = new Interval((i / (double)kFoldCount) * originInterval.Size + originInterval.Min, ((i + 1)/ (double)kFoldCount) * originInterval.Size + originInterval.Min);               
                accuracies[i] = Eval(learnJsonPath, validProperties, new NNInputProcessor(), 0);
                Network.Randomize();
            }
            TrainInterval = originInterval;
            return accuracies;
        }
        public static NNClassifier LoadFromFile(string inJsonPath)
        {
            var classifier = new NNClassifier();
            classifier.Network = (DeepBeliefNetwork) DeepBeliefNetwork.Load(inJsonPath);
            classifier.SqueezeIntervals = new Interval[classifier.Network.InputsCount];
            StreamReader sReader = new StreamReader(inJsonPath + "_support");
            JsonTextReader jReader = new JsonTextReader(sReader);
            jReader.Read();          
            for (int i = 0; i < classifier.SqueezeIntervals.Length; i++)
            {
                jReader.Read();
                JObject intervalRecord = JObject.Load(jReader);
                classifier.SqueezeIntervals[i] = new Interval((double)intervalRecord["Min"], (double)intervalRecord["Max"]);
            }
            jReader.Read();
            JToken networkName = JToken.Load(jReader);
            classifier.Name = networkName.ToString();
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
        SupportVectorMachine<Gaussian> Svm { get; set; }
        public virtual ClusterAttribute[] ValidFields { get; } = new ClusterAttribute[]{
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
        public virtual string[] OutputClasses { get; } = new string[] {
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
        Dictionary<string, NNClassifier> Classifiers { get; set; }
        public MultiLayeredClassifier()
        {
            
        }
        public MultiLayeredClassifier(IList<NNClassifier> trainedModels)
        {
            Classifiers = new Dictionary<string, NNClassifier>();
            foreach (var classifier in trainedModels)
            {
                Classifiers.Add(classifier.Name, classifier);
            }
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
        public virtual void Load(string[] trainedModelsPaths)
        {
            Classifiers = new Dictionary<string, NNClassifier>();
            for (int i = 0; i < trainedModelsPaths.Length; i++)
            {
                var classifier = NNClassifier.LoadFromFile(trainedModelsPaths[i]);
                Classifiers.Add(classifier.Name, classifier);
            }
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
            NNInputProcessor preprocessor = new NNInputProcessor();
            var intervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields);
            const string name = "fragHeFe";
            var epochSize = 8;
            NNClassifier fragClassifier = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervals, outputClasses, name);
            double success = 0;
            for(int i = 0; i < 8; i++)
            {
                success = fragClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
                if (success > acceptableSuccessRate)
                    return success;
            };
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
            const string name = "lead";
            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] intervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields);
            int epochSize = 32;
            NNClassifier commonRareClassifier = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervals, outputClasses, name);
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
            const string name = "prLe";
            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] intervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields);
            int epochSize = 6;
            NNClassifier multiClassifier = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervals, outputClasses, name);
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
            const string name = "elMuPi";
            Interval[] intervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields);
            NNClassifier multiClassifier = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 10, 10 }, new SigmoidFunction(1), intervals, outputClasses, name);
            multiClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
            return multiClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
        }
        double LearnAll(double acceptableSuccessRate)
        {

            string jsonFilePath = "../../../ClusterDescriptionGen/bin/Debug/allTestData2.json";

            StreamReader inputStream = new StreamReader(jsonFilePath);
            NNInputProcessor preprocessor = new NNInputProcessor();


            var commonRareIntervals = preprocessor.CalculateNormIntervals(jsonFilePath, ValidFields);
            const string name = "all";
            // loop
            int epochSize = 256;
            NNClassifier commonRareClassifier = new NNClassifier(ValidFields.Length, OutputClasses.Length, new int[] { 16, 16 }, new SigmoidFunction(1), commonRareIntervals, OutputClasses, name);
            return commonRareClassifier.Learn(epochSize, jsonFilePath, ValidFields, acceptableSuccessRate);
        }
        public double TestModel(string testInputPath)
        {
            var testJsonPath = "../../../ClusterDescriptionGen/bin/Debug/testCollection.json";
            int[][] ConfusionMatrix = ConfusionMatrix = new int[OutputClasses.Length + 1][];
            for (int i = 0; i < OutputClasses.Length + 1; i++)
            {
                ConfusionMatrix[i] = new int[OutputClasses.Length + 1];
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
                    if(resultClass == "unclassified")
                        ConfusionMatrix[classIndex][OutputClasses.Length]++;
                    else
                        ConfusionMatrix[classIndex][OutputClasses.IndexOf(resultClass)]++;
                    classesCounts[classIndex]++;
                    c++;
                }

            }
            var rowSums = ConfusionMatrix.Select(x => x.Sum()).ToArray();
            double totalSum = 0;
            double diagSum = 0;
            for (int j = 0; j < OutputClasses.Length; j++)
            {

                for (int k = 0; k < OutputClasses.Length; k++)
                {
                    //Console.Write(Math.Truncate(1000d * ConfusionMatrix[j][k] / (double)rowSums[j]) / 10d);
                    //Console.Write("\t");
                    totalSum += ConfusionMatrix[j][k]/(double)rowSums[j];
                    if (j == k)
                        diagSum += ConfusionMatrix[j][k]/(double)rowSums[j];


                }
                //Console.WriteLine();
            }
            var successRate = diagSum / (double)totalSum;
            return successRate;
           // Console.WriteLine("Success rate is : " + successRate);
        }
        public virtual ClassPrediction Classify(double[] inputVector)
        {
            const string unclassified = "unclassified";
            const double epsilonConfidence = 0.05;
            var leadPrediction = Classifiers["lead"].ClassifySingle(inputVector);
            var fragPrediction = Classifiers["fragHeFe"].ClassifySingle(inputVector);
            var prLePrediction = Classifiers["prLe"].ClassifySingle(inputVector);
            var elMuPiPredition = Classifiers["elMuPi"].ClassifySingle(inputVector);
            const string otherClass = "other";
            var treePrediction = leadPrediction.CombineWith(fragPrediction, otherClass, new string[] { "he", "fe", "frag" }).CombineWith(prLePrediction, otherClass).CombineWith(elMuPiPredition, "elMuPi");
            if (treePrediction.CalcConfidence() < epsilonConfidence)
                treePrediction.MostProbableClassName = unclassified;
            return treePrediction;
        }
        public Dictionary<string, int> ClassifyCollection(string testJsonPath, string specialOut = null)
        {
            var classHistogram = new Dictionary<string, int>();
            const string unclassified = "unclassified";
            JSONDecriptionWriter specialsWriter = null;
            if (specialOut != null)
            {
                specialsWriter = new JSONDecriptionWriter(new StreamWriter(specialOut));
            }
            int[][] ConfusionMatrix = ConfusionMatrix = new int[OutputClasses.Length][];
            for (int i = 0; i < OutputClasses.Length; i++)
            {
                ConfusionMatrix[i] = new int[OutputClasses.Length];
                classHistogram.Add(OutputClasses[i], 0);
            }
            classHistogram.Add(unclassified, 0);
            StreamReader inputStream = new StreamReader(testJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            NNInputProcessor preprocessor = new NNInputProcessor();
            var _inputVector = preprocessor.ReadJsonToVector(jsonStream, ValidFields);
            int[] classesCounts = new int[OutputClasses.Length];
            long processedCount = 0;
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < inputStream.BaseStream.Length)
            {
                var inputVector = preprocessor.ReadJsonToVector(jsonStream, ValidFields);
                var predictedClass = Classify(inputVector).MostProbableClassName;
                classHistogram[predictedClass]++;
                processedCount++;
                if (predictedClass == unclassified && specialOut != null)
                    CheckSpecialClusters(inputVector, specialsWriter, processedCount);

            }
            specialsWriter.Close();
            return classHistogram;
        }
        public virtual void CheckSpecialClusters(double [] inputVector, JSONDecriptionWriter writer, long processedCount)
        {
            const int lowestPixCount = 100;
            const int lowestBranchCount = 5;
            if(inputVector[Array.IndexOf(ValidFields, ClusterAttribute.PixelCount)] > lowestPixCount)
                if(inputVector[Array.IndexOf(ValidFields, ClusterAttribute.BranchCount)] > lowestBranchCount)
                {
                    var attributePairs = new Dictionary<ClusterAttribute, object>();
                    IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();

                    foreach (var checkedAttribute in ValidFields)
                    {
                        var attributeName = checkedAttribute;
                        attributePairs.Add(attributeName, null);
                        attributesToGet.Add(attributeName);
                    }
                    for(int i = 0; i < inputVector.Length;i++)
                    {
                        attributePairs[ValidFields[i]] = inputVector[i]; 
                    }
                    writer.WriteDescription(attributePairs, processedCount);
                }

        }

    }
    class Program
    {
        //using the default classifier 
        static void Main(string[] args)
        {            
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            MultiLayeredClassifier classifier = new MultiLayeredClassifier();
            const string NNFragHeFe = "../../trained_models/trainFragHeFeNew.json_trained 0.961.txt";
            const string NNPrLe = "../../trained_models/trainPrLe_ElMuPi.json_trained 0.966.txt";
            const string NNLead = "../../trained_models/trainLeadMulti.json_trained 0.909.txt";
            const string NNElMuPi = "../../trained_models/trainElMuPi.json_trained 0.801.txt";
            classifier.Load(new string[] { NNLead, NNFragHeFe, NNPrLe, NNElMuPi});
            /*if (args.Length <= 1)
                classifier.TestModel();
            else
                classifier.ClassifyCollection(args[1]);*/

            var histo = classifier.ClassifyCollection("../../../ClusterDescriptionGen/bin/Debug/testAtlas.json", "../../../ClusterDescriptionGen/bin/Debug/specials.json");
            foreach(var pair in histo)
            {
                Console.WriteLine(pair.Key + ":" + pair.Value);

            }
            /*double maxSuccess = 0.96;
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


