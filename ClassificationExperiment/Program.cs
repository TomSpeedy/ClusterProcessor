using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassifierForClusters;
using ClusterCalculator;
using Accord.Neuro;
using System.Globalization;
using System.Threading;
using System.IO;
namespace ClassificationExperiment
{
    interface ITest
    {
        void PrepareData();
        void Test();
    }
    class Program
    {
        

        static void Main(string[] args)
        {


            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            if (args.Length != 2)
            {
                //Console.WriteLine("Invalid argument count - the only argument should be the data directory path");
                //return;
            }
            args = new string[] { "", "D:\\source\\repos\\Example_data" };
            TestData.SetWorkingDirectory(args[1].Replace('\\', '/'));
            MultiLayeredClassifier classifier = new MultiLayeredClassifier();

            /*Console.WriteLine("*************** TESTING THE BEST CLASSIFIER *****************");
            classifier.LoadFromFile(TestData.workingDir + "trained_models/bestClassifier.csf");
            classifier.TestModel(TestData.dataTest);

            Console.WriteLine("*************** START OF THE FIRST TEST (DIFFERENT PARAMETERS) *****************");
            var testParameters = new TestDifferentParameters(10);
            testParameters.PrepareData();
            testParameters.Test();
            Console.WriteLine("*************** END OF THE FIRST TEST (press any key) *****************");
            Console.ReadLine();*/
            Console.WriteLine("*************** START OF THE SECOND TEST (SIMPLE vs MULTI) *****************");
            var testSimpleVsMulti = new TestSimpleVsMulti(10);
            testSimpleVsMulti.PrepareData();
            testSimpleVsMulti.Test();
            Console.WriteLine("*************** END OF THE SECOND TEST (press any key) *****************");
            Console.ReadLine();
            Console.WriteLine("*************** START OF THE THIRD TEST (CROSSVALIDATION)*****************");
            var testKFold = new TestCrossValidateSimple(6, 1);
            testKFold.PrepareData();
            testKFold.Test();
            Console.WriteLine("*************** END OF THE THIRD TEST (press any key) *****************");
            Console.ReadLine();


        }      
    }
    class TestDifferentParameters
    {
        NNClassifier[][] SimpleClassifiers { get; set; }
        const int classifierCount = 6;
        int IterationsCount { get; }
        public TestDifferentParameters(int iterationsCount)
        {
            SimpleClassifiers = new NNClassifier[classifierCount][];
            IterationsCount = iterationsCount;
        }
        public void PrepareData()
        {
            for (int i = 0; i < IterationsCount; i++)
            {              
                for (int j = 0; j < SimpleClassifiers.Length; j++)
                {
                    if(i == 0)
                    {
                        SimpleClassifiers[j] = new NNClassifier[IterationsCount];
                    }
                    SimpleClassifiers[j][i] = new NNClassifier();
                }

                SimpleClassifiers[0][i].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
                SimpleClassifiers[0][i].SetTeacher("backProp", 0.1, 0.1);

                SimpleClassifiers[1][i].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
                SimpleClassifiers[1][i].SetTeacher("backProp", 0.5, 0.5);

                SimpleClassifiers[2][i].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
                SimpleClassifiers[2][i].SetTeacher("backProp", 1, 1);

                SimpleClassifiers[3][i].ConfigureParams(new string[] { TestData.fragOneLayerConfig, TestData.dataLearnFrag });
                SimpleClassifiers[4][i].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
                
                SimpleClassifiers[5][i].ConfigureParams(new string[] { TestData.fragThreeLayerConfig, TestData.dataLearnFrag });

                bool stopCondition = false;
                for (int j = 0; j < SimpleClassifiers.Length; j++)
                {
                    SimpleClassifiers[j][i].TrainProportion = 0.2;
                    SimpleClassifiers[j][i].Learn( TestData.dataLearnFrag, 1, ref stopCondition, eval : false);
                }
            }
        }
        public void Test()
        {
            
                double[][] simpleResults = new double[SimpleClassifiers.Length][];
            for (int i = 0; i < SimpleClassifiers.Length; i++)
            {
                simpleResults[i] = new double[IterationsCount];
                for (int j = 0; j < SimpleClassifiers[i].Length; j++)
                {
                    Console.WriteLine("single classifier confusion matrix:");
                    simpleResults[i][j] = SimpleClassifiers[i][j].TestModel(TestData.fragTest);
                }
                for (int j = 0; j < SimpleClassifiers[i].Length; j++)
                {
                    Console.WriteLine("simple classifier " + i.ToString() + " accuracy (" + j.ToString()  + "):" + simpleResults[i][j].ToString());
                }
            }
            for (int i = 0; i < SimpleClassifiers.Length; i++)
            {
                Console.WriteLine("simple classifier mean: " + simpleResults[i].Mean());
            }      
        }
    }

    class TestSimpleVsMulti : ITest
    {
        public double VarianceExpected { get; set; }
        public double AccuracyExpected { get; set; }
        MultiLayeredClassifier[] MultiClassifiers { get; set; }
        NNClassifier[] SimpleClassifiers { get; set; }
     
        bool Stopped = false;
        public TestSimpleVsMulti(int testClassifierCount)
        {
            MultiClassifiers = new MultiLayeredClassifier[testClassifierCount];
            SimpleClassifiers = new NNClassifier[testClassifierCount];
        }
        public void PrepareData()
        {
            for (int i = 0; i < SimpleClassifiers.Length; i++)
            {
                Console.WriteLine($"******** Creating Simple Classifier {i} ***********");
                SimpleClassifiers[i] = new NNClassifier();
                SimpleClassifiers[i].Train(TestData.configAllPath, TestData.trainAllPath, ref Stopped, minimumAccuracy: 1, seed: i);
                for (int j = 0; j < i; j++)
                {                  
                    SimpleClassifiers[i].Learn( TestData.trainAllPath, successThreshold:1, ref Stopped,  seed: i, eval:true);
                }
                SimpleClassifiers[i].Learn(TestData.trainAllPath, successThreshold: 0, ref Stopped, seed: i, eval: true);
                MultiClassifiers[i] = new MultiLayeredClassifier();
                Console.WriteLine($"******** Creating Multi Classifier {i} ***********");
                MultiClassifiers[i].FromDefault(TestData.workingDir,seed: i);
            }

        }
        public void Test()
        {
            double[] simpleResults = new double[SimpleClassifiers.Length];
            double[] multiResults = new double[MultiClassifiers.Length];
            for (int i = 0; i < SimpleClassifiers.Length; i++)
            {
                Console.WriteLine("single classifier confusion matrix:");
                simpleResults[i] = SimpleClassifiers[i].TestModel(TestData.dataTest);
                Console.WriteLine("multi classifier confusion matrix:");
                multiResults[i] = MultiClassifiers[i].TestModel(TestData.dataTest);
                    
            }
            for (int i = 0; i < SimpleClassifiers.Length; i++)
            {
                Console.WriteLine("simple classifier accuracy" + i + ":" + simpleResults[i]);
                Console.WriteLine("multi classifier accuracy" + i + ":" + multiResults[i]);
            }
            Console.WriteLine("simple classifier mean" + ":" + simpleResults.Mean());
            Console.WriteLine("simple classifier variance:" + simpleResults.Variance());
            Console.WriteLine("multi classifier mean:" + multiResults.Mean());
            Console.WriteLine("multi classifier variance" + multiResults.Variance());
        }
    }
    class TestCrossValidateSimple : ITest
    {
        int KFoldValue { get; set; }
        NNClassifier LeadClassifier { get; set; }
        NNClassifier FragHeFeClassifier { get; set; }
        NNClassifier PrLeClassifier { get; set; }
        NNClassifier ElMuPiClassifier { get; set; }
        int RepetitionCount { get; }
        public TestCrossValidateSimple(int kFoldValue, int repetitionsCount)
        {
            RepetitionCount = repetitionsCount;
            KFoldValue = kFoldValue;

        }
        public void PrepareData()
        {

                
                LeadClassifier = new NNClassifier();

                FragHeFeClassifier = new NNClassifier();

                PrLeClassifier = new NNClassifier();

                ElMuPiClassifier = new NNClassifier();
        }
        public void Test()
        {
            double[][] accuraciesLead = new double[RepetitionCount][];
            double[][] accuraciesFragHeFe = new double[RepetitionCount][];
            double[][] accuraciesPrLe = new double[RepetitionCount][];
            double[][] accuraciesElMuPi = new double[RepetitionCount][];
            for (int i = 0; i < RepetitionCount; i++)
            {
                int learnIterationsCount = 1;
                accuraciesLead[i] = LeadClassifier.CrossValidate(KFoldValue, TestData.dataLearnLead, TestData.leadConfig, i,  learnIterationsCount);


                learnIterationsCount = 2;
                accuraciesFragHeFe[i] = FragHeFeClassifier.CrossValidate(KFoldValue, TestData.dataLearnFrag, TestData.fragHeFeConfig, i, learnIterationsCount);

                learnIterationsCount = 1;
                accuraciesPrLe[i] = PrLeClassifier.CrossValidate(KFoldValue, TestData.dataLearnPrLe, TestData.prLeConfig, i, learnIterationsCount);

                learnIterationsCount = 1;
                accuraciesElMuPi[i] = ElMuPiClassifier.CrossValidate(KFoldValue, TestData.dataLearnElMuPi, TestData.elMuPiConfig, i, learnIterationsCount);

            }
            Console.WriteLine($"********* RESULTS ***********");
            for (int i = 0; i < KFoldValue; i++)
            {
                Console.WriteLine($"{TestData.lead} accuracy : {accuraciesLead[0][i]}");
            }
            Console.WriteLine($"{TestData.lead} variance : {accuraciesLead.Variance()}");

            for (int i = 0; i < KFoldValue; i++)
            {
                Console.WriteLine($"{TestData.fragHeFe} accuracy : {accuraciesFragHeFe[0][i]}");
            }
            Console.WriteLine($"{TestData.fragHeFe} variance : {accuraciesFragHeFe.Variance()}");

            for (int i = 0; i < KFoldValue; i++)
            {
                Console.WriteLine($"{TestData.prLe} accuracy : {accuraciesPrLe[0][i]}");
            }
            Console.WriteLine($"{TestData.prLe} variance : {accuraciesPrLe.Variance()}");


            for (int i = 0; i < KFoldValue; i++)
            {
                Console.WriteLine($"{TestData.elMuPi} accuracy : {accuraciesElMuPi[0][i]}");
            }
            Console.WriteLine($"{TestData.elMuPi} variance : {accuraciesElMuPi.Variance()}");

        }
    }
    static class TestData
    {
        public static string lead = "lead";
        public static string fragHeFe = "fragHeFe";
        public static string prLe = "prLe";
        public static string elMuPi = "elMuPi";
        public static string workingDir = "";
        public static string dataLearnLead = "train_data/trainLeadNew.json";
        public static string dataLearnFrag = "train_data/trainFragHeFeNew.json";
        public static string dataLearnPrLe = "train_data/trainPrLe_ElMuPi.json";
        public static string dataLearnElMuPi = "train_data/trainElMuPi.json";
        public static string leadConfig = "train_data/LeadNetworkConfig.json";
        public static string fragHeFeConfig = "train_data/FragHeFeNetworkConfig.json";
        public static string prLeConfig = "train_data/PrLeNetworkConfig.json";
        public static string elMuPiConfig = "train_data/ElMuPiNetworkConfig.json";
        public static string dataTest = "test_data/testCollection.json";
        public static string fragTest = "test_data/testFrag.json";

        public static string fragOneLayerConfig = "train_data/FragHeFeNetworkConfig1.json";
        public static string fragTwoBiggerConfig = "train_data/FragHeFeNetworkConfig2.json";
        public static string fragThreeLayerConfig = "train_data/FragHeFeNetworkConfig3.json";

        public static string trainAllPath = "train_data/trainAll.json";
        public static string configAllPath = "train_data/AllConfig.json";
        public static void SetWorkingDirectory(string dataDir)
        {
            if (dataDir[dataDir.Length - 1] == '/')
                workingDir = dataDir;
            else
                workingDir = dataDir + "/";
            dataLearnElMuPi = workingDir + dataLearnElMuPi;
            dataLearnFrag = workingDir + dataLearnFrag;
            dataLearnPrLe = workingDir + dataLearnPrLe;
            dataLearnLead = workingDir + dataLearnLead;
            leadConfig = workingDir + leadConfig;
            fragHeFeConfig = workingDir + fragHeFeConfig;
            prLeConfig = workingDir + prLeConfig;
            elMuPiConfig = workingDir + elMuPiConfig;
            dataTest = workingDir + dataTest;
            fragTest = workingDir + fragTest;
            fragOneLayerConfig = workingDir + fragOneLayerConfig;
            fragTwoBiggerConfig = workingDir + fragTwoBiggerConfig;
            fragThreeLayerConfig = workingDir + fragThreeLayerConfig;
            trainAllPath = workingDir + trainAllPath;
            configAllPath = workingDir + configAllPath; 

        }
    }
    public static class TestUtilExtensions
    {
        public static double Variance(this double[] input)
        {
            var mean = input.Mean();
            double variance = 0;
            for (int i = 0; i < input.Length; i++)
            {
                variance += (input[i] - mean) * (input[i] - mean);
            }
            return variance / (input.Length - 1);
        }
        public static double Mean(this double[] input)
        {
            return input.Sum() / input.Length;          
        }
        public static double Mean(this double[][] input)
        {
            double result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                result += input[i].Sum() / (double)input[i].Length;
            }
            return result / (double)input.Length;

        }
        public static double Variance(this double[][] input)
        {
            var mean = input.Mean();
            double variance = 0;
            for (int i = 0; i < input.Length; i++)
                input[i].Select(value => { return variance += (value - mean)*(value-mean); }).ToArray();
            return variance / (double)(input.Length * input[0].Length - 1);
        }
    }
          
    
}
