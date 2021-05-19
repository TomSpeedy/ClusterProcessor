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
            /*var testParameters = new TestDifferentParameters();
            testParameters.PrepareData();
            testParameters.Test();
            */
            /*var testSimpleVsMulti = new TestSimpleVsMulti(10);
            testSimpleVsMulti.PrepareData();
            testSimpleVsMulti.Test();*/

            /*var testKFold = new TestCrossValidateSimple(6, 1);
            testKFold.PrepareData();
            testKFold.Test();*/

            MultiLayeredClassifier classifier = new MultiLayeredClassifier();
            classifier.LoadFromFile("../../trained_models/bestClassifier.csf");
            classifier.TestModel(TestData.dataTest);

        }      
    }
    class TestDifferentParameters
    {
        NNClassifier[] SimpleClassifiers { get; set; }
        const int classifierCount = 10;
        public TestDifferentParameters()
        {
            SimpleClassifiers = new NNClassifier[classifierCount];
        }
        public void PrepareData()
        {
            for (int i = 0; i < SimpleClassifiers.Length; i++)
            {
                SimpleClassifiers[i] = new NNClassifier();
            }
            SimpleClassifiers[0].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
            SimpleClassifiers[0].EpochSize = 1;            

            SimpleClassifiers[1].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
            SimpleClassifiers[1].EpochSize = 8;

            SimpleClassifiers[2].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
            SimpleClassifiers[2].EpochSize = 16;

            SimpleClassifiers[3].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
            SimpleClassifiers[3].SetTeacher("backProp", 0.1, 0.1);

            SimpleClassifiers[4].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
            SimpleClassifiers[4].SetTeacher("backProp", 0.5, 0.5);

            SimpleClassifiers[5].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
            SimpleClassifiers[5].SetTeacher("backProp", 0.9, 0.9);

            SimpleClassifiers[6].ConfigureParams(new string[] { TestData.fragHeFeConfig, TestData.dataLearnFrag });
            SimpleClassifiers[7].ConfigureParams(new string[] { TestData.fragOneLayerConfig, TestData.dataLearnFrag });
            SimpleClassifiers[8].ConfigureParams(new string[] { TestData.fragTwoBiggerConfig, TestData.dataLearnFrag });
            SimpleClassifiers[9].ConfigureParams(new string[] { TestData.fragThreeLayerConfig, TestData.dataLearnFrag });

            bool stopCondition = false;
            for (int i = 0; i < SimpleClassifiers.Length; i++)
            {
                SimpleClassifiers[i].TrainProportion = 0.5;
                SimpleClassifiers[i].Learn( TestData.dataLearnFrag, 1, ref stopCondition);
            }
        }
        public void Test()
        {
            
                double[] simpleResults = new double[SimpleClassifiers.Length];
                for (int i = 0; i < SimpleClassifiers.Length; i++)
                {
                    Console.WriteLine("single classifier confusion matrix:");
                    simpleResults[i] = SimpleClassifiers[i].TestModel(TestData.fragTest);
                }
                for (int i = 0; i < SimpleClassifiers.Length; i++)
                {
                    Console.WriteLine("simple classifier accuracy" + i + ":" + simpleResults[i]);
                }
                Console.WriteLine("simple classifier mean: " + simpleResults.Mean());
                Console.WriteLine("simple classifier variance: " + simpleResults.Variance());           
        }
    }

    class Test { }
    class TestSimpleVsMulti : ITest
    {
        public double VarianceExpected { get; set; }
        public double AccuracyExpected { get; set; }
        MultiLayeredClassifier[] MultiClassifiers { get; set; }
        NNClassifier[] SimpleClassifiers { get; set; }
        const string trainAllPath = "../../train_data/trainAll.json";
        const string configAllPath = "../../train_data/AllConfig.json";
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
                SimpleClassifiers[i].Train(configAllPath, trainAllPath, ref Stopped, minimumAccuracy: 1, seed: i);
                for (int j = 0; j < i; j++)
                {                  
                    SimpleClassifiers[i].Learn( trainAllPath, successThreshold:1, ref Stopped,  seed: i, eval:true);
                }
                SimpleClassifiers[i].Learn(trainAllPath, successThreshold: 0, ref Stopped, seed: i, eval: true);
                MultiClassifiers[i] = new MultiLayeredClassifier();
                Console.WriteLine($"******** Creating Multi Classifier {i} ***********");
                MultiClassifiers[i].FromDefault(seed: i);
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
        public Dictionary<string, double[][]> CVErrors { get; private set; } = new Dictionary<string, double[][]>();
        public Dictionary<string, double> Variances { get; private set; } = new Dictionary<string, double>();
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
            CVErrors.Add(TestData.lead, accuraciesLead);
            Variances.Add(TestData.lead, accuraciesLead.Variance());
            CVErrors.Add(TestData.fragHeFe, accuraciesFragHeFe);
            Variances.Add(TestData.fragHeFe, accuraciesFragHeFe.Variance());
            CVErrors.Add(TestData.prLe, accuraciesPrLe);
            Variances.Add(TestData.prLe, accuraciesPrLe.Variance());
            CVErrors.Add(TestData.elMuPi, accuraciesElMuPi);
            Variances.Add(TestData.elMuPi, accuraciesElMuPi.Variance());

        }
    }
    static class TestData
    {
        public const string lead = "lead";
        public const string fragHeFe = "fragHeFe";
        public const string prLe = "prLe";
        public const string elMuPi = "elMuPi";
        public const string dataLearnLead = "../../train_data/trainLeadNew.json";
        public const string dataLearnFrag = "../../train_data/trainFragHeFeNew.json";
        public const string dataLearnPrLe = "../../train_data/trainPrLe_ElMuPi.json";
        public const string dataLearnElMuPi = "../../train_data/trainElMuPi.json";
        public const string leadConfig = "../../train_data/LeadNetworkConfig.json";
        public const string fragHeFeConfig = "../../train_data/FragHeFeNetworkConfig.json";
        public const string prLeConfig = "../../train_data/PrLeNetworkConfig.json";
        public const string elMuPiConfig = "../../train_data/ElMuPiNetworkConfig.json";
        public const string dataTest = "../../test_data/testCollection.json";
        public const string fragTest = "../../test_data/testFrag.json";

        public const string fragOneLayerConfig = "../../train_data/FragHeFeNetworkConfig1.json";
        public const string fragTwoBiggerConfig = "../../train_data/FragHeFeNetworkConfig2.json";
        public const string fragThreeLayerConfig = "../../train_data/FragHeFeNetworkConfig3.json";
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
