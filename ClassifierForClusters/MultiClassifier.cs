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

    public interface IClassifier
    {
        void LoadFromFile(string filePath);
        ClassPrediction Classify(Dictionary<ClusterAttribute, double> inputPairs);
        Dictionary<string, int> ClassifyCollection(string inputFile, ClassificationOutputType processingType = ClassificationOutputType.SplitClasses, 
            ClassificationOutputFileCount fileCount = ClassificationOutputFileCount.Multiple);
        ClusterAttribute[] ValidFields { get; }
        string[] OutputClasses { get; }
    }
    /// <summary>
    /// Multi level, non trainable classifier
    /// </summary>
    public class MultiLevelClassifier : IClassifier
    {
        public virtual ClusterAttribute[] ValidFields { get; set; }
        public virtual string[] OutputClasses { get; set; }
        public ClassifierRoot<IClassifier> ClassifierTree { get; set; }
        /// <summary>
        /// Build multi classifier by composing multiple classifiers linearly
        /// </summary>
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
                    if (!parent.Model.OutputClasses.ToList().Contains(splitClasses[i]))
                    {
                        Console.WriteLine($"Classifiers cannot be merged because classifier {i} does not contain required split class: \'{splitClasses[i]}\'");
                        throw new ArgumentException();
                    }
                    parent.Descendants.Add(splitClasses[i], node);
                    node = parent;
                }               
                ClassifierTree = new ClassifierRoot<IClassifier>(topDownClassifiers[0]);
                if (!ClassifierTree.Model.OutputClasses.ToList().Contains(splitClasses[0]))
                {
                    Console.WriteLine($"Classifiers cannot be merged because classifier {0} does not contain required split class: \'{splitClasses[0]}\'");
                    throw new ArgumentException();
                }
                ClassifierTree.Descendants.Add(splitClasses[0], node);
            }
            OutputClasses = ClassifierTree.GetAllOuputClasses();
            ValidFields = ClassifierTree.GetAllUsedAttributes();

        }
        public void SetRoot(ClassifierRoot<IClassifier> classifierRoot)
        {
            ClassifierTree = classifierRoot;
            OutputClasses = ClassifierTree.GetAllOuputClasses();
            ValidFields = ClassifierTree.GetAllUsedAttributes();
        }
        /// <summary>
        /// Load or train the default classifier structure
        /// </summary>
        public void FromDefault(string dataDir, int? seed = null)
        {
            string modelLead = dataDir +"trained_models/trainLeadNew.json_trained_0.994.csf";
            string modelFrag = dataDir + "trained_models/trainFragHeFeNew.json_trained_0.961.csf";
            string modelPrLe = dataDir + "trained_models/trainPrLe_ElMuPi.json_trained_0.966.csf";
            string modelElMuPi = dataDir + "trained_models/trainElMuPi.json_trained_0.801.csf";

            var leadNN = new NNClassifier();
            var fragHeFeNN = new NNClassifier();
            var prLeNN = new NNClassifier();
            var elMuPiNN = new NNClassifier();
            bool stopped = false;
            if (!seed.HasValue)
            {
                //loading existing models
                leadNN.LoadFromFile(modelLead);
                fragHeFeNN.LoadFromFile(modelFrag);
                prLeNN.LoadFromFile(modelPrLe);
                elMuPiNN.LoadFromFile(modelElMuPi);
            }
            else
            {
                //training new models
                string dataLead = dataDir + "train_data/trainLeadNew.json";
                string dataFrag = dataDir + "train_data/trainFragHeFeNew.json";
                string dataPrLe = dataDir + "train_data/trainPrLe_ElMuPi.json";
                string dataElMuPi = dataDir + "train_data/trainElMuPi.json";
                string leadConfig = dataDir + "train_data/LeadNetworkConfig.json";
                string fragHeFeConfig = dataDir + "train_data/FragHeFeNetworkConfig.json";
                string prLeConfig = dataDir + "train_data/PrLeNetworkConfig.json";
                string elMuPiConfig = dataDir + "train_data/ElMuPiNetworkConfig.json";
                leadNN.Train(leadConfig, dataLead, ref stopped, 1, seed.Value);
                fragHeFeNN.Train(fragHeFeConfig, dataFrag, ref stopped, 1, seed.Value);
                for (int i = 0; i < 2; i++)
                    fragHeFeNN.Learn(dataFrag, 1, ref stopped, seed: seed.Value);
                prLeNN.Train(prLeConfig, dataPrLe, ref stopped, 1, seed.Value);
                elMuPiNN.Train(elMuPiConfig, dataElMuPi, ref stopped, 1, seed.Value);


            }
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
            SetRoot(leadNode);
        }
        public virtual void LoadFromFile(string trainedModelsPath)
        {
            using (StreamReader mainDeserializer = new StreamReader(trainedModelsPath))
            {
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                MultiLevelClassifier classifier = JsonConvert.DeserializeObject<MultiLevelClassifier>(mainDeserializer.ReadToEnd(), settings);
                this.ClassifierTree = classifier.ClassifierTree;
                this.OutputClasses = classifier.OutputClasses;
                this.ValidFields = classifier.ValidFields;


                FileStream NNDeserializer = new FileStream(trainedModelsPath + "_support", FileMode.Open);
                ClassifierTree.DeserializeNN(NNDeserializer);
            }

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
            return PrintConfMatrix(ConfusionMatrix);
        }
        public double PrintConfMatrix(int[][] confusionMatrix, bool normalized = true)
        {
            const string unclassified = "unclassified";
            Console.Write("Tru\\Pre");
            var rowSums = confusionMatrix.Select(x => x.Sum()).ToArray();
            double totalSum = 0;
            double diagSum = 0;
            for (int j = 0; j < confusionMatrix.Length; j++)
            {
                for (int k = 0; k < confusionMatrix.Length; k++)
                {
                    double value = 0;
                    if (normalized)
                        value = confusionMatrix[j][k] / (double)rowSums[j];
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
            for (int i = 0; i < confusionMatrix.Length; i++)
            {
                Console.Write("\t");

                if (i < confusionMatrix.Length - 1)
                {
                    string className = OutputClasses[i];
                    Console.Write(className.Substring(0, Math.Min(4, className.Length)));
                }
                else
                    Console.WriteLine(unclassified.Substring(0, 4));
            }
            for (int i = 0; i < confusionMatrix.Length; i++)
            {
                if (i < confusionMatrix.Length - 1)
                {
                    string className = OutputClasses[i];
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
        /// <summary>
        /// classifies a single example of the cluster
        /// </summary>
        /// <param name="inputVector"></param>
        /// <returns></returns>
        public virtual ClassPrediction Classify(Dictionary<ClusterAttribute, double> inputVector)
        {
            const string unclassified = "unclassified";
            const double epsilonConfidence = 0.05;

            var treePrediction = ClassifierTree.Classify(inputVector);

            if (treePrediction.CalcConfidence() < epsilonConfidence)
                treePrediction.MostProbableClassName = unclassified;
            return treePrediction;
        }
        /// <summary>
        /// Classifies the whole set of cluster in a single call
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        public Dictionary<string, int> ClassifyCollection(string inputPath, ClassificationOutputType outputType = ClassificationOutputType.SplitClasses, 
            ClassificationOutputFileCount outFileCount = ClassificationOutputFileCount.Multiple)
        {
            const string unclassified = "unclassified";
            const string special = "__special";
            const string output = "classified";
            var classHistogram = new Dictionary<string, int>();
            var classStreams = new Dictionary<string, JSONDecriptionWriter>();
            if (outputType != ClassificationOutputType.Histogram)
            {
                if(outFileCount == ClassificationOutputFileCount.Multiple)
                {
                    for (int i = 0; i < OutputClasses.Length; i++)
                    {
                        classStreams.Add(OutputClasses[i], new JSONDecriptionWriter(new StreamWriter(inputPath.Substring(0, inputPath.LastIndexOf('.')) + "_" + OutputClasses[i] + ".json")));
                    }
                    classStreams.Add(unclassified, new JSONDecriptionWriter(new StreamWriter(inputPath.Substring(0, inputPath.LastIndexOf('.')) + "_" + unclassified + ".json")));
                }
                else
                {
                    classStreams.Add(output, new JSONDecriptionWriter(new StreamWriter(inputPath.Substring(0,inputPath.LastIndexOf('.')) + "_" + output + ".json")));
                }
            }
            JSONDecriptionWriter specialsWriter = null;
            if (outputType == ClassificationOutputType.SplitClassesAndSpecials)
            {
                if (outFileCount == ClassificationOutputFileCount.Multiple)
                    specialsWriter = new JSONDecriptionWriter(new StreamWriter(inputPath.Substring(0, inputPath.LastIndexOf('.')) + special + ".json"));
                else
                    specialsWriter = classStreams[output];
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
                {                    
                    wholeRecord = AddClassToRecord(wholeRecord, predictedClass);
                    bool isSpecial = false;
                    if (predictedClass == unclassified && outputType == ClassificationOutputType.SplitClassesAndSpecials)
                        isSpecial = CheckSpecialClusters(inputVector, wholeRecord, specialsWriter, processedCount);
                    if(!isSpecial)
                    {
                        if (outFileCount == ClassificationOutputFileCount.Multiple)
                        {
                            classStreams[predictedClass].Write(wholeRecord);
                        }
                        else
                        {                      
                            classStreams[output].Write(wholeRecord);
                        }
                    }
                }
                classHistogram[predictedClass]++;
                processedCount++;
            }
            if(specialsWriter != null)
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
        string AddClassToRecord(string wholeJsonRecord, string predictedClass)
        {
            const string classKey = "Class";
            StringReader inputString = new StringReader(wholeJsonRecord);
            var jsonString = new JsonTextReader(inputString);
            jsonString.Read();
            JObject jsonCluster = JObject.Load(jsonString);
            if (jsonCluster.ContainsKey(classKey))
                jsonCluster[classKey] = predictedClass;
            else
                jsonCluster.Add(classKey, predictedClass);
            return jsonCluster.ToString();

        }
        /// <summary>
        /// checks if a cluster can be considered special
        /// </summary>
        public virtual bool CheckSpecialClusters(Dictionary<ClusterAttribute, double> inputPairs, string wholeRecord, JSONDecriptionWriter writer, long processedCount)
        {
            const int lowestPixCount = 150;
            const int lowestBranchCount = 3;
            if ((int)inputPairs[ClusterAttribute.PixelCount] > lowestPixCount)
                if ((int)inputPairs[ClusterAttribute.BranchCount] > lowestBranchCount)
                {
                    wholeRecord = AddClassToRecord(wholeRecord, "special");
                    writer.Write(wholeRecord);
                    return true;
                }
            return false;
        }

    }
    /// <summary>
    /// wrapper around a simple classifier so we can coonnect them into a tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        public ClassifierRoot(IClassifier rootClassifier) : base(rootClassifier)
        {
        }

    }

    [Serializable]
    public enum ClassificationOutputType
    {
        Histogram,
        SplitClasses,
        SplitClassesAndSpecials
    }
    [Serializable]
    public enum ClassificationOutputFileCount
    {
        Single,
        Multiple
    }

}
