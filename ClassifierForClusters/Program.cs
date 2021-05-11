using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.IO;
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
    public class Interval
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
        public int ClustersProcessed { get; set; } = 0;
        public Dictionary<ClusterAttribute, double> NormalizeElements(Dictionary<ClusterAttribute, double> valuePairs, Dictionary<ClusterAttribute, Interval> intervalPairs)
        {
            Dictionary<ClusterAttribute, double> newPairs = new Dictionary<ClusterAttribute, double>();
            foreach (var valuePair in valuePairs)
            {
                newPairs.Add(valuePair.Key, (valuePair.Value - intervalPairs[valuePair.Key].Min) / (intervalPairs[valuePair.Key].Size));
            }
            return newPairs;
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
                ClustersProcessed++;
            }
            return classes.ToArray();
        }
        public Dictionary<ClusterAttribute, Interval> CalculateNormIntervals(string jsonPath, IEnumerable<ClusterAttribute> usableKeys)
        {
            StreamReader sReader = new StreamReader(jsonPath);
            JsonTextReader jReader = new JsonTextReader(sReader);
            var squeezeIntervals = new Dictionary<ClusterAttribute, Interval>();
            var _numVector = ReadJsonToVector(jReader, usableKeys);
            foreach(var attribute in usableKeys)
            {
                squeezeIntervals.Add(attribute, new Interval(double.MaxValue, double.MinValue));
            }
            while (sReader.Peek() != -1)
            {
                var intervalPairs = ReadJsonToVector(jReader, usableKeys);
                foreach (var attribute in usableKeys)
                {
                    if (intervalPairs[attribute] < squeezeIntervals[attribute].Min)
                        squeezeIntervals[attribute].Min = intervalPairs[attribute];
                    if (intervalPairs[attribute] > squeezeIntervals[attribute].Max)
                        squeezeIntervals[attribute].Max = intervalPairs[attribute];

                }
                ClustersProcessed++;
            }
            return squeezeIntervals;
        }
        public Dictionary<ClusterAttribute, double> ReadTrainJsonToVector(JsonTextReader reader, IEnumerable<ClusterAttribute> usableKeys, string[] classes, out int classIndex)
        {

            if (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {

                    Dictionary<ClusterAttribute, double> valuePairs = new Dictionary<ClusterAttribute, double>();
                    JObject jsonRecord = JObject.Load(reader);
                    foreach (var usableKey in usableKeys)
                    {
                        double attributeVal;
                        if (!jsonRecord.ContainsKey(usableKey.ToString()))
                            throw new ArgumentException($"Error, Required property \"{usableKey}\" for training set is not included in given input file");

                        if (double.TryParse(jsonRecord[usableKey.ToString()].ToString(), out attributeVal))
                        {
                            valuePairs.Add(usableKey, attributeVal);
                        }

                    }
                    const string ClassKey = "Class";
                    if (!jsonRecord.ContainsKey(ClassKey))
                        throw new ArgumentException($"Error, Required \"{ClassKey}\" property is not included in given input file");

                    classIndex = Array.IndexOf(classes, jsonRecord[ClassKey].ToString());
                    if (classIndex < 0)
                        throw new ArgumentException($"Error, Class value: \"{jsonRecord[ClassKey]}\" is not valid class value");
                    ClustersProcessed++;
                    return valuePairs;
                }
            }
            classIndex = -1;
            return null;
        }
        public Dictionary<ClusterAttribute, double> ReadJsonToVector(JsonTextReader reader, IEnumerable<ClusterAttribute> usableKeys)
        {

            if (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {

                    Dictionary<ClusterAttribute, double> valuePairs = new Dictionary<ClusterAttribute, double>();
                    JObject jsonRecord = JObject.Load(reader);
                    foreach (var usableKey in usableKeys)
                    {
                        double attributeVal;
                        if (!jsonRecord.ContainsKey(usableKey.ToString()))
                            throw new ArgumentException($"Error, Required property \"{usableKey}\" for training set is not included in given input file");

                        if (double.TryParse(jsonRecord[usableKey.ToString()].ToString(), out attributeVal))
                        {
                            valuePairs.Add(usableKey, attributeVal);
                        }

                    }
                    ClustersProcessed++;
                    return valuePairs;
                }
            }
            return null;
        }
        public Dictionary<ClusterAttribute, double> ReadWholeJsonToVector(JsonTextReader reader, IEnumerable<ClusterAttribute> usableKeys, long id, out string wholeRecord)
        {
            wholeRecord = "";
            if (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {

                    Dictionary<ClusterAttribute, double> valuePairs = new Dictionary<ClusterAttribute, double>();
                    JObject jsonRecord = JObject.Load(reader);                   
                    wholeRecord = jsonRecord.ToString();
                    foreach (var usableKey in usableKeys)
                    {
                        double attributeVal;
                        if (!jsonRecord.ContainsKey(usableKey.ToString()))
                            throw new ArgumentException($"Error, Required property \"{usableKey}\" for training set is not included in given input file");

                        if (double.TryParse(jsonRecord[usableKey.ToString()].ToString(), out attributeVal))
                        {
                            valuePairs.Add(usableKey, attributeVal);
                        }

                    }
                    ClustersProcessed++;
                    return valuePairs;
                }
            }
            return null;
        }
        public int CountAllRecords(string filePath)
        {

            StreamReader sReader = new StreamReader(filePath);
            JsonTextReader jReader = new JsonTextReader(sReader);
            jReader.Read();
            while (sReader.Peek() != -1)
            {
                jReader.Read();
                jReader.Skip();
                ClustersProcessed++;
            }
            jReader.Close();
            sReader.Close();
            return ClustersProcessed;
        }
    }
    public interface IClassifier
    {
       void LoadFromFile(string filePath);
        ClassPrediction Classify(Dictionary<ClusterAttribute, double> inputPairs);
        ClusterAttribute[] ValidFields { get; }
        string[] OutputClasses { get; }
    }
    public interface ITrainableClassifier : IClassifier
    {
        double Train(string configFilePath, string trainDataPath, ref bool stopCondition,  double minimumAccuracy = 0, int seed = 42);

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
    public static class ActivationFunctionFatory
    {
        public static IActivationFunction CreateNew(string actFuncionName)
        {
            switch (actFuncionName)
            {
                case "relu":
                    return new RectifiedLinearFunction();
                case "sigmoid":
                    return new SigmoidFunction(1);
                default:
                    throw new ArgumentException("Error - Activation function was not specified or is not supported");
            }
        }
    }
    public static class TrainableClassifierFactory
    {
        public static ITrainableClassifier CreateNew(string classifierType)
        {
            switch (classifierType)
            {
                case "defaultMLP":
                    return new NNClassifier();
                default:
                    throw new ArgumentException("Error - Invalid classifierType");
            }
        }
    }
    public static class TeacherFatory
    {
        public static ISupervisedLearning CreateNew(string teacherName, ActivationNetwork network)
        {
            switch (teacherName)
            {
                case "backProp":
                    return new BackPropagationLearning(network);
                case "leven-marq":
                    return new LevenbergMarquardtLearning(network);
                default:
                    throw new ArgumentException("Error - Learning algorithm was not specified or is not supported");
            }
        }
    }
    
    [Serializable]
    public class NNClassifier : ITrainableClassifier
    {
        public DeepBeliefNetwork Network { get; private set; }
        public string Name { get; set; }
        public Dictionary<ClusterAttribute, Interval> IntervalsSqueeze { get; set; }

        public Interval TrainInterval { get; set; } = new Interval(0, 0.9);
        public Interval TestInterval { get; set; } = new Interval(0, 0.1);
        public int PrintInterval { get; set; } = 200;
        public bool Silent { get; set; } = false;
        public bool Evaluate { get; set; } = true;
        public string[] OutputClasses { get; set; }
        public int EpochSize { get; set; }
        public List<string> EvaluationClasses { get; set; }
        public Dictionary<string, string> EvaluationClassesMap { get; set; }
        public ClusterAttribute[] ValidFields { get; set; }
        private DeepNeuralNetworkLearning Teacher { get; set; }
        public NNClassifier() { }
        public void SetNetwork(DeepBeliefNetwork network)
        {
            Network = network;
        }
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
        private void SetTeacher(string teacherName, double? momentum , double? learningRate)
        {
            Teacher = new DeepNeuralNetworkLearning(Network);
            Teacher.Algorithm = (activationNetwork, index) =>
            {

                var teacher = TeacherFatory.CreateNew(teacherName, activationNetwork);
                if (teacher.GetType() == typeof(BackPropagationLearning))
                {
                    ((BackPropagationLearning)teacher).Momentum = momentum.HasValue ? momentum.Value : 0.5;
                    ((BackPropagationLearning)teacher).LearningRate = learningRate.HasValue ? learningRate.Value : 0.6;
                }
                if (teacher.GetType() == typeof(LevenbergMarquardtLearning))
                {
                    ((LevenbergMarquardtLearning)teacher).LearningRate = learningRate.HasValue ? learningRate.Value : 0.6;
                }
                return teacher;
            };
            Teacher.LayerCount = Network.Layers.Length;
            Teacher.LayerIndex = 0;
        }
        public NNClassifier(int inputLen, int outputLen, int[] layerSizes, IActivationFunction activationFunction, Interval[] squeezeIntervals, string[] outputClasses, string name)
        {
            var layerNewSizes = layerSizes.Append(outputLen).ToArray();
            Network = new DeepBeliefNetwork(inputLen, layerNewSizes);
            Network.SetActivationFunction(activationFunction);
            //SqueezeIntervals = squeezeIntervals;
            SetDefaultTeacher();
            OutputClasses = outputClasses;
            Name = name;

        }
        public NNClassifier(DeepBeliefNetwork network, IActivationFunction activationFunction, Interval[] squeezeIntervals, string[]outputClasses, string name)
        {
            Network = network;
            Network.SetActivationFunction(activationFunction);
            //SqueezeIntervals = squeezeIntervals;
            OutputClasses = outputClasses;
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
        public int SetClassesAndIntervalsFromData(string learnFile)
        {
            NNInputProcessor preprocessor = new NNInputProcessor();
            IntervalsSqueeze = preprocessor.CalculateNormIntervals(learnFile, ValidFields);

            OutputClasses = preprocessor.FindAllClasses(learnFile);
            return preprocessor.ClustersProcessed / 2;
        }
        public int ConfigureParams(string[] inputFiles)
        {
            if (inputFiles == null || inputFiles.Length != 2)
                throw new ArgumentException("Error - Incorrect number of configuration files passed");
            string configFile = inputFiles[0];
            string trainDataFile = inputFiles[1];
            StreamReader sReader = new StreamReader(configFile);
            JsonTextReader jReader = new JsonTextReader(sReader);
            jReader.Read();
            if (jReader.TokenType != JsonToken.StartObject)
                throw new ArgumentException("Error - Invalid JSON format");
            JObject networkParameters = JObject.Load(jReader);
            if (!networkParameters.ContainsKey("validAttributes"))
                throw new ArgumentException("Error - Valid properties for the training of the classifier were not specified");
            ValidFields = networkParameters["validAttributes"].Select(jObject => jObject.ToString().ToAttribute()).ToArray();

            if (networkParameters.ContainsKey("outputClasses"))
                OutputClasses = networkParameters["outputClasses"].Select(jObject => jObject.ToString()).ToArray();

            int clusterCount = SetClassesAndIntervalsFromData(trainDataFile);

            if (!networkParameters.ContainsKey("layerSizes"))
                throw new ArgumentException("Error - LauerSizes of the classifier were not specified");
            int[] layerSizes = networkParameters["layerSizes"].Select(jObject =>
                {
                    if (uint.TryParse(jObject.ToString(), out uint layerSize))
                        return (int)layerSize;
                    throw new ArgumentException("Error - the value of the layer Size was not valid");
                }).ToArray();
            Network = new DeepBeliefNetwork(ValidFields.Length, layerSizes.Append(OutputClasses.Length).ToArray());
            if (!networkParameters.ContainsKey("activationFunction"))
                throw new ArgumentException("Error - activation function of the classifier was not specified");
            var activationFunctions = networkParameters["activationFunction"].Select(jObject => jObject.ToString()).ToArray();
            if (activationFunctions.Length == 1)
                Network.SetActivationFunction(ActivationFunctionFatory.CreateNew(activationFunctions[0]));
            else
            {
                throw new NotImplementedException("Multiple activation functions are not yet supported");
            }

            if (!networkParameters.ContainsKey("learningAlgorithm"))
                throw new ArgumentException("Error - learning algorithm of the classifier was not specified");
            string teacherName = networkParameters["learningAlgorithm"].ToString();

            double? learnRate = null;
            if (networkParameters.ContainsKey("learningRate"))
            {
                if (double.TryParse(networkParameters["learningRate"].ToString(), out double learnRateJson))
                    learnRate = learnRateJson;
                else
                    throw new ArgumentException("Error - the value of the learning rate was not valid");
            }

            double? momentum = null;
            if (networkParameters.ContainsKey("momentum"))
            {
                if (double.TryParse(networkParameters["momentum"].ToString(), out double momentumJson))
                    momentum = momentumJson;
                else
                    throw new ArgumentException("Error - the value of the momentum was not valid");
            }
            SetTeacher(teacherName, learnRate, momentum);

            if (!networkParameters.ContainsKey("name"))
                throw new ArgumentException("Error - name of the classifier was not specified");
            Name = networkParameters["name"].ToString();
            if (networkParameters.ContainsKey("epochSize"))
            {
                if (uint.TryParse(networkParameters["epochSize"].ToString(), out uint epochSize))
                    EpochSize = (int)epochSize;
                else
                throw new ArgumentException("Error - the value of the momentum was not valid");
            }
            else
                throw new ArgumentException("Error - name of the classifier was not specified");
            if(networkParameters.ContainsKey("usedTrainDataSize"))
            {
                if (double.TryParse(networkParameters["usedTrainDataSize"].ToString(), out double usedTrainData))
                {
                    if (usedTrainData <= 0 || usedTrainData > 1)
                        throw new ArgumentException("Error the used train data parameter must have value between 0 and 1");
                   TrainInterval.Max = usedTrainData;
                   TrainInterval.Min = 1 - usedTrainData;
                }
                else
                    throw new ArgumentException("Error - the value of the momentum was not valid");
            }
            if (networkParameters.ContainsKey("evaluationMultiClasses"))
            {
                var multiClassesJson = networkParameters["evaluationMultiClasses"].ToObject<JObject>();
                EvaluationClassesMap = new Dictionary<string, string>();
                EvaluationClasses = new List<string>();
                foreach (var multiClassJson in multiClassesJson.Properties())
                {
                    if (EvaluationClasses.Contains(multiClassJson.Name))
                        throw new ArgumentException("Error - Multiclass already exists");
                    EvaluationClasses.Add(multiClassJson.Name);
                    foreach (var evalClass in multiClassJson.Value.ToArray())
                    {
                        string evalClassName = evalClass.ToString();
                        if (!OutputClasses.Contains(evalClassName))
                            throw new ArgumentException("Error - invalid class name");
                        EvaluationClassesMap.Add(evalClassName, multiClassJson.Name);                     
                    }                  
                }
                if(EvaluationClassesMap.Keys.Count != OutputClasses.Length)
                    throw new ArgumentException("Error - Uncategorized class - when evaluationMultiClasses attribute is present, all classes must belong to some multiClass");
            }
            return clusterCount;
        }

        public double Train(string configFilePath, string trainDataPath, ref bool stopCondition, double minimumAccuracy = 0, int seed = 42)
        {

            int clusterCount = ConfigureParams(new string[] { configFilePath, trainDataPath });
            Random rand = new Random(seed);
            int[] trainIndices = Enumerable.Range(0, clusterCount)
                .OrderBy(index => rand.Next())    //permutating the indices
                .Take((int)(TrainInterval.Max * clusterCount))
                .OrderBy(index => index).ToArray();  //sorting for faster further browsing
            return Learn(trainDataPath, minimumAccuracy, ref stopCondition, eval: true, trainIndices, seed);

        }
        public double Learn(string learnJsonPath, double successThreshold, ref bool stopCondition, bool eval = true, int[] trainIndices = null, int seed = 42)
        {
            if (trainIndices == null)
            {
                NNInputProcessor inputProcesor = new NNInputProcessor();
                int clusterNumber = inputProcesor.CountAllRecords(learnJsonPath);
                Random rand = new Random(seed);
                trainIndices = Enumerable.Range(0, clusterNumber)
                .OrderBy(index => rand.Next())    
                .Take((int)(TrainInterval.Max * clusterNumber))
                .OrderBy(index => index).ToArray();
            }
            int clusterCount = trainIndices.Length;
            int currentTrainIndex = 0;
            var inputStream = new StreamReader(learnJsonPath);
            var jsonStream = new JsonTextReader(inputStream);

            NNInputProcessor preprocessor = new NNInputProcessor();
            //empty read
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int _classIndex);
            var iteration = 0;
            double errorSum = 0;
            bool done = false;
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length && !done )
            {               
                double[][] input = new double[EpochSize][];
                double[][] output = new double[EpochSize][];
                int epochIteration = 0;
                while (epochIteration < EpochSize)
                {                   
                    var unsqueezedPairs = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int classIndex);
                    if (unsqueezedPairs == null)
                    {
                        done = true;
                        break;
                    }
                    if (currentTrainIndex > trainIndices.Length - 1 || iteration != trainIndices[currentTrainIndex])
                    {
                        iteration++;
                        continue; 
                    }
                    var squeezedPairs = preprocessor.NormalizeElements(unsqueezedPairs, IntervalsSqueeze);
                    List<double> orderedVector = new List<double>();
                    foreach (var field in ValidFields)
                    {
                        orderedVector.Add(squeezedPairs[field]);
                    }
                    var outputVector = new double[OutputClasses.Length];
                    outputVector[classIndex] = 1;
                    input[epochIteration] = orderedVector.ToArray();
                    output[epochIteration] = outputVector;
                    epochIteration++;
                    currentTrainIndex++;
                }
                if (inputStream.BaseStream.Position < inputStream.BaseStream.Length * TrainInterval.Min || done)
                        continue;
                double error = Teacher.RunEpoch(input, output);
                errorSum += error;
                if (currentTrainIndex % PrintInterval == 0)
                {
                    if (!Silent)
                        Console.WriteLine(errorSum);
                    errorSum = 0;
                }
                iteration++;
                
            }
            if (!eval)
                return -1;
            return Eval(learnJsonPath, successThreshold, trainIndices);
            
        }
        private double Eval(string learnJsonPath, double successThreshold, int[] trainIndices)
        {
            NNInputProcessor preprocessor = new NNInputProcessor();
            int matrixLength = EvaluationClasses == null ? OutputClasses.Length : EvaluationClasses.Count();
            int[][] confusionMatrix = new int[matrixLength][];
            int currentTrainIndex = 0;
            int iteration = 0;


            for (int j = 0; j < matrixLength; j++)
            {
                confusionMatrix[j] = new int[matrixLength];
            }
            var inputStream = new StreamReader(learnJsonPath);
            var jsonStream = new JsonTextReader(inputStream);
            //initial empty read
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int ___classIndex);
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length)
            {
                var unNormalizedVect = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int classIndex);
                if (unNormalizedVect == null)
                {
                    break;
                }
                if (currentTrainIndex < trainIndices.Length  && iteration == trainIndices[currentTrainIndex])
                {
                    iteration++;
                    currentTrainIndex++;
                    continue;
                }
                if (inputStream.BaseStream.Position > inputStream.BaseStream.Length * TestInterval.Min)
                {
                    var outputVector = new double[OutputClasses.Length];
                    outputVector[classIndex] = 1;

                    var squeezedPairs = preprocessor.NormalizeElements(unNormalizedVect, IntervalsSqueeze);
                    List<double> orderedVector = new List<double>();
                    foreach (var field in ValidFields)
                    {
                        orderedVector.Add(squeezedPairs[field]);
                    }
                    var result = Network.Compute(orderedVector.ToArray());
                    var prediction = new ClassPrediction(result, OutputClasses).MostProbableClassName;
                    if (EvaluationClasses != null)
                    {
                        prediction = EvaluationClassesMap[prediction];
                        confusionMatrix[EvaluationClasses.IndexOf(EvaluationClassesMap[OutputClasses[classIndex]])][EvaluationClasses.IndexOf(prediction)]++;
                    }
                    else
                        confusionMatrix[classIndex][OutputClasses.IndexOf(prediction)]++;
                    iteration++;
                }

            }
            var totalSum = 0;
            var diagSum = 0;
            for (int j = 0; j < confusionMatrix.Length; j++)
            {

                for (int k = 0; k < confusionMatrix.Length; k++)
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
                StoreToFile(learnJsonPath + "_trained " + successRate + ".csf");
            }
            return successRate;
        }
        public void StoreToFile(string outJsonPath)
        {
         
            Network.Save(outJsonPath);

            StreamWriter supportWriter = new StreamWriter(outJsonPath + "_support");
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            supportWriter.Write(JsonConvert.SerializeObject(this, settings));
            supportWriter.Close();
            /*
            StreamWriter writer = new StreamWriter(outJsonPath + "_support");
            string intervalsString = System.Text.Json.JsonSerializer.Serialize(SqueezeIntervals);
            string classesString = System.Text.Json.JsonSerializer.Serialize(OutputClasses);
            writer.Write(intervalsString.Substring(0, intervalsString.Length - 1));
            writer.Write("," + System.Text.Json.JsonSerializer.Serialize(Name));
            writer.Write("," + classesString.Substring(1));
            writer.Close();*/
        }
        /// <summary>
        /// Calculates the K-fold cross validation - expects the data to be ordered randomly
        /// </summary>
        /// <param name="kFoldCount"></param>
        /// <param name="EpochSize"></param>
        /// <param name="learnJsonPath"></param>
        /// <param name="ValidFields"></param>
        /// <param name="successThreshold"></param>
        /// <returns></returns>
        public double[] CrossValidate(int kFoldCount, string learnJsonPath, int seed, int learnIterationsCount = 1)
        {
            double[] accuracies = new double[kFoldCount];
            var originInterval = TrainInterval;
            NNInputProcessor preprocessor = new NNInputProcessor();
            int clusterCount = preprocessor.CountAllRecords(learnJsonPath);
            Random rand = new Random();
            int[] trainIndices = Enumerable.Range(0, clusterCount)
                        .OrderBy(index => rand.Next())
                        .ToArray();
            int[][] splitTrainIndices = new int[kFoldCount][];
            for (int i = 0; i < kFoldCount; i++)
            {
                int[] currentTrainIndices = new int[(trainIndices.Length ) / kFoldCount];
                Array.Copy(trainIndices, i * (trainIndices.Length / kFoldCount), currentTrainIndices, 0, trainIndices.Length / kFoldCount);
                splitTrainIndices[i] = currentTrainIndices;
            }
             bool done = false;
            for (int i = 0; i < kFoldCount; i++)
            {
                int[] currentTrainIndices = new int[(trainIndices.Length * (kFoldCount - 1)) / kFoldCount];
                for (int j = 0; j < kFoldCount; j++)
                {
                    if (j != i)
                        splitTrainIndices[j].CopyTo(currentTrainIndices, j * trainIndices.Length / kFoldCount);
                }
                for (int learnIteration = 0; learnIteration < learnIterationsCount; learnIteration++)
                {                   
                    Learn(learnJsonPath, 1, ref done, eval: false, currentTrainIndices, seed);
                }               
                accuracies[i] = Eval(learnJsonPath, 1, splitTrainIndices[i]);
                Network.Randomize();
            }
            TrainInterval = originInterval;
            return accuracies;
        }
        public void LoadFromFile(string inJsonPath)
        {
            this.Network =  DeepBeliefNetwork.Load(inJsonPath);

            //this.SqueezeIntervals = new Interval[this.Network.InputsCount];
            using (StreamReader sReader = new StreamReader(inJsonPath + "_support"))
            {
                using (JsonTextReader jReader = new JsonTextReader(sReader))
                {
                    var classifier = JsonConvert.DeserializeObject<NNClassifier>(sReader.ReadToEnd());
                    Name = classifier.Name;
                    EpochSize = classifier.EpochSize;
                    EvaluationClasses = classifier.EvaluationClasses;
                    EvaluationClassesMap = classifier.EvaluationClassesMap;
                    OutputClasses = classifier.OutputClasses;
                    PrintInterval = classifier.PrintInterval;
                    Silent = classifier.Silent;
                    IntervalsSqueeze = classifier.IntervalsSqueeze;
                    //SqueezeIntervals = classifier.SqueezeIntervals;
                    Teacher = classifier.Teacher;
                    TestInterval = classifier.TestInterval;
                    TrainInterval = classifier.TrainInterval;
                    ValidFields = classifier.ValidFields;
                    /*jReader.Read();          
                    for (int i = 0; i < this.SqueezeIntervals.Length; i++)
                    {
                        jReader.Read();
                        JObject intervalRecord = JObject.Load(jReader);
                        this.SqueezeIntervals[i] = new Interval((double)intervalRecord["Min"], (double)intervalRecord["Max"]);
                    }
                    jReader.Read();
                    JToken networkName = JToken.Load(jReader);
                    this.Name = networkName.ToString();
                    this.OutputClasses = new string[this.Network.OutputCount];
                    for (int i = 0; i < this.Network.OutputCount; i++)
                    {
                        jReader.Read();
                        JToken classRecord = JToken.Load(jReader);
                        this.OutputClasses[i] = classRecord.ToString();
                    }       */
                    jReader.Close();
                    sReader.Close();
                }
            };
            
        }
        public ClassPrediction Classify(Dictionary<ClusterAttribute, double> inputPairs)
        {
            NNInputProcessor preprocessor = new NNInputProcessor();
            var squeezedPairs = preprocessor.NormalizeElements(inputPairs, IntervalsSqueeze);
            List<double> orderedVector = new List<double>();
            foreach (var field in ValidFields)
            {
                orderedVector.Add(squeezedPairs[field]);
            }

            var resultVector = Network.Compute(orderedVector.ToArray());


            return new ClassPrediction(resultVector, OutputClasses);
        }
        

    }
    [Serializable]
    public class ClassifierNode<T> where T : IClassifier
    {
        public IClassifier Model { get; set; }
        public Dictionary<string, ClassifierNode<T>> Descendants { get; set; } = new Dictionary<string, ClassifierNode<T>>();
        public ClassifierNode(IClassifier baseClassifier)
        {
            Model = baseClassifier;
            
        }
        public virtual void SerializeNN(FileStream writer)
        {
            if (Model.GetType() == typeof(NNClassifier))
            {
                var classifier = (NNClassifier)Model;
                classifier.Network.Save(writer);
                //Serializer.Save(classifier.Teacher, writer);
                foreach (var descendantKey in Descendants.Keys.Sorted())
                {
                    Descendants[descendantKey].SerializeNN(writer);
                }
            }
        }
        public virtual void DeserializeNN(FileStream reader)
        {
            if (Model.GetType() == typeof(NNClassifier))
            {
                var classifier = (NNClassifier)Model;
                classifier.SetNetwork(DeepBeliefNetwork.Load(reader));
                //classifier.Teacher = Serializer.Load<DeepNeuralNetworkLearning>(reader);
                foreach (var descendantKey in Descendants.Keys.Sorted())
                {
                    Descendants[descendantKey].DeserializeNN(reader);
                }
            }
        }
               
        public ClassPrediction Classify(Dictionary<ClusterAttribute, double> inputPairs/*double[] inputVector*/)
        {
            var thisPrediction = this.Model.Classify(inputPairs);
            if (Descendants.ContainsKey(thisPrediction.MostProbableClassName))
            {
                return Descendants[thisPrediction.MostProbableClassName].Classify(inputPairs);
            }
            return thisPrediction;
        }
        public string[] GetAllOuputClasses()
        {
            IEnumerable<string> result = Model.OutputClasses.Where(outClass => !Descendants.ContainsKey(outClass));
            foreach (var descendant in Descendants)
            {
                result = result.Union(descendant.Value.GetAllOuputClasses());
            }
            return result.ToArray();
        }
        public ClusterAttribute[] GetAllUsedAttributes()
        {
            IEnumerable<ClusterAttribute> result = Model.ValidFields;
            foreach (var descendant in Descendants)
            {
                result.Union(descendant.Value.GetAllUsedAttributes());
            }
            return result.ToArray();
        }
    }
    [Serializable]
    public class ClassifierRoot<T> : ClassifierNode<T> where T : IClassifier 
    {
        public ClassifierRoot(IClassifier rootClassifier) : base (rootClassifier)
        {
        }
        
    }
    [Serializable]
    public class MultiLayeredClassifier : IClassifier
    {
        SupportVectorMachine<Gaussian> Svm { get; set; }
        public virtual ClusterAttribute[] ValidFields { get; set; } 
        public virtual string[] OutputClasses { get; set; }
        public ClassifierRoot<IClassifier> ClassifierTree { get; set; }
        public void FromLinearTrees(List<IClassifier> topDownClassifiers, List<string> splitClasses)
        {
            if (splitClasses == null || topDownClassifiers == null || splitClasses.Count + 1 != topDownClassifiers.Count)
            {
                throw new ArgumentException("Invalid number of the simple classifiers was passed - must be equal to split classes count + 1");
            }
            if (topDownClassifiers.Count == 1)
            {

                ClassifierTree = new ClassifierRoot<IClassifier>(topDownClassifiers[0]);
            }
            else
            {
                var node = new ClassifierNode<IClassifier>(topDownClassifiers[topDownClassifiers.Count - 1]);
                for (int i = topDownClassifiers.Count - 2; i > 0; i--)
                {

                    var parent = new ClassifierNode<IClassifier>(topDownClassifiers[i]);
                    parent.Descendants.Add(splitClasses[i], node);
                    node = parent;
                }
                ClassifierTree = new ClassifierRoot<IClassifier>(topDownClassifiers[0]);
                ClassifierTree.Descendants.Add(splitClasses[0], node);
            }
            OutputClasses = ClassifierTree.GetAllOuputClasses();
            ValidFields = ClassifierTree.GetAllUsedAttributes();

        }
        public void FromDefault()
        {
            //"../../trained_models/trainLeadMulti.json_trained_0.909.txt"
            const string modelLead = "../../trained_models/trainLeadNew.json_trained_0.994.csf";
            const string modelFrag = "../../trained_models/trainFragHeFeNew.json_trained_0.961.csf";
            const string modelPrLe = "../../trained_models/trainPrLe_ElMuPi.json_trained_0.966.csf";
            const string modelElMuPi = "../../trained_models/trainElMuPi.json_trained_0.801.csf";

            var leadNN = new NNClassifier();
            leadNN.LoadFromFile(modelLead);
            var fragHeFeNN = new NNClassifier();
            fragHeFeNN.LoadFromFile(modelFrag);
            var prLeNN = new NNClassifier();
            prLeNN.LoadFromFile(modelPrLe);
            var elMuPiNN = new NNClassifier();
            elMuPiNN.LoadFromFile(modelElMuPi);

            ClassifierNode<IClassifier> elMuPiNode = new ClassifierNode<IClassifier>(elMuPiNN);
            ClassifierNode<IClassifier> prLeNode = new ClassifierNode<IClassifier>(prLeNN);
            ClassifierNode<IClassifier> fragHeFeNode = new ClassifierNode<IClassifier>(fragHeFeNN);
            ClassifierRoot<IClassifier> leadNode = new ClassifierRoot<IClassifier>(leadNN);


            prLeNode.Descendants.Add("elMuPi", elMuPiNode);
            fragHeFeNode.Descendants.Add("other", prLeNode);
            leadNode.Descendants.Add("frag", fragHeFeNode);
            leadNode.Descendants.Add("he", fragHeFeNode);
            leadNode.Descendants.Add("fe", fragHeFeNode);
            leadNode.Descendants.Add("other", fragHeFeNode);                
            ClassifierTree = leadNode;
            OutputClasses = ClassifierTree.GetAllOuputClasses();
            ValidFields = ClassifierTree.GetAllUsedAttributes();
        }
       /* public void TestSvm()
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
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < 0.1 * inputStream.BaseStream.Length && c < 3000)
            {
                var inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, outputClasses, out int classIndex);
                //var resultClass = Classify(inputVector).MostProbableClassName;
                if (inputStream.BaseStream.Position > 0.5 * inputStream.BaseStream.Length && classesCounts[classIndex] < 300true)
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
        }*/
        public virtual void LoadFromFile(string trainedModelsPath)
        {
            using (StreamReader mainDeserializer = new StreamReader(trainedModelsPath))
            {
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                MultiLayeredClassifier classifier = JsonConvert.DeserializeObject<MultiLayeredClassifier>(mainDeserializer.ReadToEnd(), settings);
                this.ClassifierTree = classifier.ClassifierTree;
                this.OutputClasses = classifier.OutputClasses;
                this.ValidFields = classifier.ValidFields;


                FileStream NNDeserializer = new FileStream(trainedModelsPath + "_support", FileMode.Open);
                ClassifierTree.DeserializeNN(NNDeserializer);
            }
                //LearnSvm();
            //TestSvm(); 

        }
        public virtual void StoreToFile(string outputFilePath)
        {
            StreamWriter writer = new StreamWriter(outputFilePath);
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            writer.Write(JsonConvert.SerializeObject(this, settings));
            writer.Close();

            FileStream supportWriter = new FileStream(outputFilePath + "_support", FileMode.OpenOrCreate);
            ClassifierTree.SerializeNN(supportWriter);
            supportWriter.Close();
            supportWriter.Dispose();
        }
            /*public double LearnFrag(double acceptableSuccessRate)
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
                    success = fragClassifier.Learn(epochSize, jsonFilePath,  acceptableSuccessRate);
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
            }*/
        public double TestModel(string testInputPath)
        {

            int[][] ConfusionMatrix = ConfusionMatrix = new int[OutputClasses.Length + 1][];
            for (int i = 0; i < OutputClasses.Length + 1; i++)
            {
                ConfusionMatrix[i] = new int[OutputClasses.Length + 1];
            }
            StreamReader inputStream = new StreamReader(testInputPath);
            var jsonStream = new JsonTextReader(inputStream);
            NNInputProcessor preprocessor = new NNInputProcessor();
            var _inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int _classIndex);
            int[] classesCounts = new int[OutputClasses.Length];
            int c = 0;
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < 1 * inputStream.BaseStream.Length/* && c < 3000*/)
            {
                var inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int classIndex);
                var resultClass = Classify(inputVector).MostProbableClassName;
                if (true)
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
                    Console.Write(Math.Truncate(1000d * ConfusionMatrix[j][k] / (double)rowSums[j]) / 10d);
                    Console.Write("\t");
                    totalSum += ConfusionMatrix[j][k]/(double)rowSums[j];
                    if (j == k)
                        diagSum += ConfusionMatrix[j][k]/(double)rowSums[j];


                }
                Console.WriteLine();
            }
            var successRate = diagSum / (double)totalSum;
            return successRate;
        }
        public virtual ClassPrediction Classify(Dictionary<ClusterAttribute, double> inputVector)
        {
            const string unclassified = "unclassified";
            const double epsilonConfidence = 0.05;

            var treePrediction = ClassifierTree.Classify(inputVector);

            if (treePrediction.CalcConfidence() < epsilonConfidence)
                treePrediction.MostProbableClassName = unclassified;
            return treePrediction;
        }
        public Dictionary<string, int> ClassifyCollection(string inputPath, ClassifiactionOutputType outputType = ClassifiactionOutputType.PrintClasses)
        {
            const string unclassified = "unclassified";
            const string special = "__special";
            var classHistogram = new Dictionary<string, int>();
            var classStreams = new Dictionary<string, JSONDecriptionWriter>();
            if (outputType != ClassifiactionOutputType.Histogram)
            {
                for (int i = 0; i < OutputClasses.Length; i++)
                {
                    classStreams.Add(OutputClasses[i], new JSONDecriptionWriter(new StreamWriter(inputPath + "_" + OutputClasses[i] + ".json")));
                }
                classStreams.Add(unclassified, new JSONDecriptionWriter(new StreamWriter(inputPath + "_" + unclassified + ".json")));
            }
            JSONDecriptionWriter specialsWriter = null;
            if (outputType != ClassifiactionOutputType.PrintClassesAndSpecials)
            {
                specialsWriter = new JSONDecriptionWriter(new StreamWriter(inputPath + special + ".json"));
            }
            for (int i = 0; i < OutputClasses.Length; i++)
            {
                classHistogram.Add(OutputClasses[i], 0);
            }

            classHistogram.Add(unclassified, 0);
            StreamReader inputStream = new StreamReader(inputPath);
            var jsonStream = new JsonTextReader(inputStream);
            NNInputProcessor preprocessor = new NNInputProcessor();
            long processedCount = 0;
            var _inputVector = preprocessor.ReadWholeJsonToVector(jsonStream, ValidFields, processedCount, out string _wholeRecord);   
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < inputStream.BaseStream.Length)
            {
                var inputVector = preprocessor.ReadWholeJsonToVector(jsonStream, ValidFields, processedCount, out string wholeRecord);
                var predictedClass = Classify(inputVector).MostProbableClassName;
                if (outputType != ClassifiactionOutputType.Histogram)
                    classStreams[predictedClass].Write(wholeRecord);
                classHistogram[predictedClass]++;
                processedCount++;
                if (predictedClass == unclassified && outputType == ClassifiactionOutputType.PrintClassesAndSpecials)
                    CheckSpecialClusters(inputVector, wholeRecord, specialsWriter, processedCount);
            }
            specialsWriter.Close();
            if (outputType != ClassifiactionOutputType.Histogram)
            {
                foreach (var outputStreamPair in classStreams)
                {
                    outputStreamPair.Value.Close();
                }
            }
            return classHistogram;
        }
        
        public virtual void CheckSpecialClusters(Dictionary<ClusterAttribute, double> inputPairs, string wholeRecord, JSONDecriptionWriter writer, long processedCount)
        {
            const int lowestPixCount = 100;
            const int lowestBranchCount = 5;
            if((int)inputPairs[ClusterAttribute.PixelCount] > lowestPixCount)
                if((int)inputPairs[ClusterAttribute.BranchCount] > lowestBranchCount)
                {
                    var attributePairs = new Dictionary<ClusterAttribute, object>();
                    IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
                    writer.Write(wholeRecord);
                }
           
        }

    }
    public enum ClassifiactionOutputType
    { 
        Histogram, 
        PrintClasses,
        PrintClassesAndSpecials
     
    }
    class Program
    {
        //using the default classifier 
        static void Main(string[] args)
        {
            args = new string[] { "program", "D:/source/repos/Celko 2020 Example data/trained_models/bestClassifier.csf",
                "D:/source/repos/Celko2020/ClusterDescriptionGen/bin/Debug/etstNoBackSlash.json", "--classes" };
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            const string distrOption = "--distr";     
            const string printClassesOption = "--classes";
            const string printClassesAndSpecialsOption = "--specials";
            MultiLayeredClassifier classifier = new MultiLayeredClassifier();


             if (args.Contains(distrOption))
            {
                classifier.LoadFromFile(args[1]);
                var histo = classifier.ClassifyCollection(args[2], ClassifiactionOutputType.Histogram);
                foreach (var pair in histo)
                {
                    Console.WriteLine(pair.Key + ":" + pair.Value);
                }
            }
            else 
            {
                classifier.LoadFromFile(args[1]);
                var histo = classifier.ClassifyCollection(args[2], 
                    args.Contains(printClassesAndSpecialsOption) ? ClassifiactionOutputType.PrintClassesAndSpecials
                       : ClassifiactionOutputType.PrintClasses);
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


