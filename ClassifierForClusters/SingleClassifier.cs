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
using ClusterCalculator;

namespace ClassifierForClusters
{
    public interface ITrainableClassifier : IClassifier
    {
        double Train(string configFilePath, string trainDataPath, ref bool stopCondition, double minimumAccuracy = 0, int seed = 42);

    }
    /// <summary>
    /// Default implementation of MLp classifier
    /// </summary>
    [Serializable]  
    public class NNClassifier : ITrainableClassifier
    {
        const string unclassified = "unclassified";
        public DeepBeliefNetwork Network { get; private set; }
        public string Name { get; set; }
        public Dictionary<ClusterAttribute, Interval> IntervalsSqueeze { get; set; }
        public double TrainProportion { get; set; } = 0.9;
        public double TestProportion { get; set; } = 0.1;
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
        public void SetTeacher(string teacherName, double? momentum, double? learningRate)
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
        public NNClassifier(int inputLen, int outputLen, int[] layerSizes, IActivationFunction activationFunction, string[] outputClasses, string name)
        {
            var layerNewSizes = layerSizes.Append(outputLen).ToArray();
            Network = new DeepBeliefNetwork(inputLen, layerNewSizes);
            Network.SetActivationFunction(activationFunction);
            SetDefaultTeacher();
            OutputClasses = outputClasses;
            Name = name;

        }
        public NNClassifier(DeepBeliefNetwork network, IActivationFunction activationFunction, string[] outputClasses, string name)
        {
            Network = network;
            Network.SetActivationFunction(activationFunction);
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
        /// <summary>
        /// Loads parameters of classifier from config file
        /// </summary>
        /// <param name="inputFiles">config file and training data file</param>
        public int ConfigureParams(string[] inputFiles)
        {
            Console.WriteLine("Analyzing the data...");
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
            if (networkParameters.ContainsKey("usedTrainDataSize"))
            {
                if (double.TryParse(networkParameters["usedTrainDataSize"].ToString(), out double usedTrainData))
                {
                    if (usedTrainData <= 0 || usedTrainData > 1)
                        throw new ArgumentException("Error the used train data parameter must have value between 0 and 1");
                    TrainProportion = usedTrainData;
                }
                else
                    throw new ArgumentException("Error - the value of the momentum was not valid");
            }
            if (networkParameters.ContainsKey("printInterval"))
            {
                if (uint.TryParse(networkParameters["printInterval"].ToString(), out uint printIntervalJson))
                    PrintInterval = (int)printIntervalJson;
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
                if (EvaluationClassesMap.Keys.Count != OutputClasses.Length)
                    throw new ArgumentException("Error - Uncategorized class - when evaluationMultiClasses attribute is present, all classes must belong to some multiClass");
            }
            return clusterCount;
        }
        /// <summary>
        /// loads the model from config file and trains according to the parameters
        /// </summary>
        /// <param name="configFilePath">NN config file</param>
        /// <param name="trainDataPath"> train data for NN</param>
        /// <param name="stopCondition"> bool indicating when to stop the process</param>
        /// <param name="minimumAccuracy"></param>
        /// <param name="seed"> seed for splitting the data into train and test set</param>
        /// <returns></returns>
        public double Train(string configFilePath, string trainDataPath, ref bool stopCondition, double minimumAccuracy = 0, int seed = 42)
        {

            int clusterCount = ConfigureParams(new string[] { configFilePath, trainDataPath });
            Random rand = new Random(seed);
            int[] trainIndices = Enumerable.Range(0, clusterCount)
                .OrderBy(index => rand.Next())    //permutating the indices
                .Take((int)(TrainProportion * clusterCount))
                .OrderBy(index => index).ToArray();  //sorting for faster further browsing
            return Learn(trainDataPath, minimumAccuracy, ref stopCondition, eval: true, trainIndices, seed);

        }
        /// <summary>
        /// Learning algorithm of NN
        /// </summary>
        /// <param name="learnJsonPath"> train data source</param>
        /// <param name="successThreshold"> minimal accuracy expected</param>
        /// <param name="stopCondition"> bool for stopping algorithm </param>
        /// <param name="eval"> bool indication if evaluation is needed</param>
        /// <param name="trainIndices"> if we prepared train indices in advance, pass them here</param>
        /// <param name="seed"> seed for dataset splitting </param>
        /// <returns></returns>
        public double Learn(string learnJsonPath, double successThreshold, ref bool stopCondition, bool eval = true, int[] trainIndices = null, int seed = 42)
        {
            if (trainIndices == null)
            {
                NNInputProcessor inputProcesor = new NNInputProcessor();
                int clusterNumber = inputProcesor.CountAllRecords(learnJsonPath);
                Random rand = new Random(seed);
                trainIndices = Enumerable.Range(0, clusterNumber)
                .OrderBy(index => rand.Next())
                .Take((int)(TrainProportion * clusterNumber))
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
            bool done = false;
            int currentEpoch = 0;
            while (inputStream.BaseStream.Position < inputStream.BaseStream.Length && !done && !stopCondition)
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
                if (done)
                    continue;
                double error = Teacher.RunEpoch(input, output) / (double)EpochSize;

                if (currentEpoch % PrintInterval == 0)
                {
                    if (!Silent)
                        Console.WriteLine($"Network Training MSE after {currentEpoch} epochs: {error}");
                }
                currentEpoch++;
                iteration++;

            }
            if (!eval)
                return -1;
            return Eval(learnJsonPath, successThreshold, trainIndices);

        }
        /// <summary>
        /// evaluate the trained network after learning
        /// </summary>
        private double Eval(string learnJsonPath, double successThreshold, int[] trainIndices)
        {
            Console.WriteLine("Evaluation on test data in progress...");

            NNInputProcessor preprocessor = new NNInputProcessor();
            int matrixLength = EvaluationClasses == null ? OutputClasses.Length + 1 : EvaluationClasses.Count() + 1;
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
                if (currentTrainIndex < trainIndices.Length && iteration == trainIndices[currentTrainIndex])
                {
                    iteration++;
                    currentTrainIndex++;
                    continue;
                }

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
                    if (prediction == unclassified)
                    {
                        confusionMatrix[EvaluationClasses.IndexOf(EvaluationClassesMap[OutputClasses[classIndex]])][EvaluationClasses.Count]++;
                    }
                    else
                    {

                        prediction = EvaluationClassesMap[prediction];
                        confusionMatrix[EvaluationClasses.IndexOf(EvaluationClassesMap[OutputClasses[classIndex]])][EvaluationClasses.IndexOf(prediction)]++;
                    }
                }
                else
                {
                    if (prediction == unclassified)
                    {
                        confusionMatrix[classIndex][OutputClasses.Length]++;
                    }
                    else
                    {
                        confusionMatrix[classIndex][OutputClasses.IndexOf(prediction)]++;
                        iteration++;
                    }
                }
            }
            var totalSum = 0;
            var diagSum = 0;
            for (int j = 0; j < confusionMatrix.Length; j++)
            {

                for (int k = 0; k < confusionMatrix.Length; k++)
                {
                    totalSum += confusionMatrix[j][k];
                    if (j == k)
                        diagSum += confusionMatrix[j][k];
                }
            }
            var successRate = diagSum / (double)totalSum;
            if (!Silent)
            {
                PrintConfusionMatrix(confusionMatrix);
            }
            if (successRate >= successThreshold)
            {
                successRate = (Math.Truncate(successRate * 1000)) / 1000d;
                StoreToFile(learnJsonPath + "_trained_" + successRate + "_" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '-') + ".csf");
            }
            return successRate;
        }
        public double PrintConfusionMatrix(int[][] confusionMatrix, bool normalized = true)
        {
            var rowSums = confusionMatrix.Select(x => x.Sum()).ToArray();
            double totalSum = 0;
            double diagSum = 0;
            for (int j = 0; j < confusionMatrix.Length; j++)
            {

                for (int k = 0; k < confusionMatrix.Length; k++)
                {
                    double value = 0;
                    if (normalized)
                    {
                        if (rowSums[j] == 0)
                            value = 0;
                        else
                            value = confusionMatrix[j][k] / (double)rowSums[j];
                    }
                    else
                        value = confusionMatrix[j][k];
                    totalSum += value;
                    if (j == k)
                        diagSum += value;
                }
            }

            Console.Write("Tru\\Pre");
            for (int i = 0; i < confusionMatrix.Length; i++)
            {
                Console.Write("\t");

                if (i < confusionMatrix.Length - 1)
                {
                    string className = EvaluationClasses == null ? OutputClasses[i] : EvaluationClasses[i];
                    Console.Write(className.Substring(0, Math.Min(4, className.Length)));
                }
                else
                    Console.WriteLine(unclassified.Substring(0, 4));
            }
            for (int i = 0; i < confusionMatrix.Length; i++)
            {
                if (i < confusionMatrix.Length - 1)
                {
                    string className = EvaluationClasses == null ? OutputClasses[i] : EvaluationClasses[i];
                    Console.Write(className.Substring(0, Math.Min(4, className.Length)));
                }
                else
                    Console.Write(unclassified.Substring(0, 4));
                for (int j = 0; j < confusionMatrix.Length; j++)
                {
                    Console.Write("\t");
                    if (normalized)
                    {
                        if (rowSums[i] == 0)
                            Console.Write(0);
                        else
                            Console.Write(Math.Truncate(1000d * confusionMatrix[i][j] / (double)rowSums[i]) / 1000d);
                    }
                    else
                        Console.Write(confusionMatrix[i][j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Classifier Accuracy :" + diagSum / (double)totalSum);
            return diagSum / (double)totalSum;
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
        }
        public double[] CrossValidate(int kFoldCount, string learnJsonPath, string configPath, int seed, int learnIterationsCount = 1)
        {
            ConfigureParams(new string[] { configPath, learnJsonPath });
            double[] accuracies = new double[kFoldCount];
            var originInterval = TrainProportion;
            NNInputProcessor preprocessor = new NNInputProcessor();
            int clusterCount = preprocessor.CountAllRecords(learnJsonPath);
            Random rand = new Random();
            int[] trainIndices = Enumerable.Range(0, clusterCount)
                        .OrderBy(index => rand.Next())
                        .ToArray();
            int[][] splitTrainIndices = new int[kFoldCount][];
            for (int i = 0; i < kFoldCount; i++)
            {
                int[] currentTrainIndices = new int[(trainIndices.Length) / kFoldCount];
                Array.Copy(trainIndices, i * (trainIndices.Length / kFoldCount), currentTrainIndices, 0, trainIndices.Length / kFoldCount);
                splitTrainIndices[i] = currentTrainIndices;
            }
            bool done = false;
            for (int i = 0; i < kFoldCount; i++)
            {
                int[] currentTrainIndices = new int[splitTrainIndices[0].Length * (kFoldCount - 1)];
                int diffIndex = 0;
                for (int j = 0; j < kFoldCount; j++)
                {
                    if (j != i)
                    {
                        splitTrainIndices[j].CopyTo(currentTrainIndices, (j - diffIndex) * splitTrainIndices[0].Length);

                    }
                    else
                    {
                        diffIndex = 1;
                    }
                }
                currentTrainIndices.Sort();
                for (int learnIteration = 0; learnIteration < learnIterationsCount; learnIteration++)
                {
                    Learn(learnJsonPath, 1, ref done, eval: false, currentTrainIndices, seed: seed);
                }
                accuracies[i] = Eval(learnJsonPath, 1, splitTrainIndices[i].Sorted());
                Network.Randomize();
            }

            TrainProportion = originInterval;
            return accuracies;
        }
        public void LoadFromFile(string inJsonPath)
        {
            this.Network = DeepBeliefNetwork.Load(inJsonPath);
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
                    Teacher = classifier.Teacher;
                    ValidFields = classifier.ValidFields;
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

            const double epsilonConfidence = 0.05;

            var classPrediction = new ClassPrediction(resultVector, OutputClasses);
            if (classPrediction.CalcConfidence() < epsilonConfidence)
                classPrediction.MostProbableClassName = unclassified;
            return classPrediction;
        }
        public Dictionary<string, int> ClassifyCollection(string inputPath, ClassificationOutputType outputType = ClassificationOutputType.SplitClasses, ClassificationOutputFileCount outFileCount = ClassificationOutputFileCount.Multiple)
        {
            const string unclassified = "unclassified";
            const string special = "__special";
            var classHistogram = new Dictionary<string, int>();
            var classStreams = new Dictionary<string, JSONDecriptionWriter>();
            if (outputType != ClassificationOutputType.Histogram)
            {
                for (int i = 0; i < OutputClasses.Length; i++)
                {
                    classStreams.Add(OutputClasses[i], new JSONDecriptionWriter(new StreamWriter(inputPath + "_" + OutputClasses[i] + ".json")));
                }
                classStreams.Add(unclassified, new JSONDecriptionWriter(new StreamWriter(inputPath + "_" + unclassified + ".json")));
            }
            JSONDecriptionWriter specialsWriter = null;
            if (outputType != ClassificationOutputType.SplitClassesAndSpecials)
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
                if (outputType != ClassificationOutputType.Histogram)
                    classStreams[predictedClass].Write(wholeRecord);
                classHistogram[predictedClass]++;
                processedCount++;
                if (predictedClass == unclassified && outputType == ClassificationOutputType.SplitClassesAndSpecials)
                    CheckSpecialClusters(inputVector, wholeRecord, specialsWriter, processedCount);
            }
            specialsWriter.Close();
            if (outputType != ClassificationOutputType.Histogram)
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
            if ((int)inputPairs[ClusterAttribute.PixelCount] > lowestPixCount)
                if ((int)inputPairs[ClusterAttribute.BranchCount] > lowestBranchCount)
                {
                    var attributePairs = new Dictionary<ClusterAttribute, object>();
                    IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
                    writer.Write(wholeRecord);
                }

        }
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
            while (inputStream.Peek() != -1 && inputStream.BaseStream.Position < 1 * inputStream.BaseStream.Length)
            {
                var inputVector = preprocessor.ReadTrainJsonToVector(jsonStream, ValidFields, OutputClasses, out int classIndex);
                if (inputVector == null)
                    break;
                if (classIndex == 1)
                {
                }
                var resultClass = Classify(inputVector).MostProbableClassName;
                if (true)
                {
                    if (resultClass == "unclassified")
                        ConfusionMatrix[classIndex][OutputClasses.Length]++;
                    else
                        ConfusionMatrix[classIndex][OutputClasses.IndexOf(resultClass)]++;
                    classesCounts[classIndex]++;
                    c++;
                }

            }

            return PrintConfusionMatrix(ConfusionMatrix);
        }

    }
    /// <summary>
    /// represents an object for data normalization
    /// </summary>
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
    /// <summary>
    /// preprocessor of the input for NN
    /// </summary>
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
            foreach (var attribute in usableKeys)
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
}
