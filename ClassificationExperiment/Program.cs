using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassifierForClusters;
using ClusterCalculator;
using Accord.Neuro;
namespace ClassificationExperiment
{
    interface ITest
    {
        void PrepareData();
        void Test(string inputTestFile = null);
    }
    class Program
    {


        static void Main(string[] args)
        {
            /*
            const string dataTestAll = "../../test_data/testCollection.json";
            var testDefault = new TestManyDefaultClassifiers(5);
            testDefault.PrepareData();
            testDefault.Test(dataTestAll);
            */
            var testKFold = new TestCrossValidateSimple(6, 1);
            testKFold.PrepareData();
            testKFold.Test();

        }      
    }

    class TestManyDefaultClassifiers : ITest
    {
        public double VarianceExpected { get; set; }
        public double AccuracyExpected { get; set; }
        int TestClassifierCount { get; }     
        MultiLayeredClassifier[] Classifiers { get; set; }
        const string dataLearnLead = "../../train_data/trainLeadMulti.json";
        const string dataLearnFrag = "../../train_data/trainFragHeFeNew.json";
        const string dataLearnPrLe = "../../train_data/trainPrLe_ElMuPi.json";
        const string dataLearnElMuPi = "../../train_data/trainElMuPi.json";      
        ClusterAttribute[] ValidFields { get; } = new MultiLayeredClassifier().ValidFields;
            
    public TestManyDefaultClassifiers(int testClassifierCount)
        {
            TestClassifierCount = testClassifierCount;
            Classifiers = new MultiLayeredClassifier[testClassifierCount];
        }
        public void PrepareData()
        {
            for (int i = 0; i < TestClassifierCount; i++)
            {
                AddNewClasifier(i);
            }

        }
        public void Test(string inputData)
        {
            double[] accuracies = new double[Classifiers.Length];
            for(int i = 0; i < Classifiers.Length; i++)
            {
                accuracies[i] = Classifiers [i].TestModel(inputData);
            }
            AccuracyExpected = accuracies.Average();
            accuracies.ToList().ForEach(accuracy => { VarianceExpected += Math.Pow(accuracy - AccuracyExpected, 2) / (accuracies.Length - 1); });

        }
        void AddNewClasifier(int indexToAdd)
        {           
            var classifiers = new List<NNClassifier>();
            classifiers.Add(DefaultLearner.LearnLead().classifier);
            classifiers.Add(DefaultLearner.LearnFragHeFe().classifier);
            classifiers.Add(DefaultLearner.LearnPrLe().classifier);
            classifiers.Add(DefaultLearner.LearnElMuPi().classifier);
            MultiLayeredClassifier multiClassifier = null;// new MultiLayeredClassifier(classifiers);
            Classifiers[indexToAdd] = multiClassifier;
        }

    }
    class TestCrossValidateSimple :ITest
    {
        Dictionary<string, double[]> ExpectedValues { get; set; } = new Dictionary<string, double[]>();
        Dictionary<string, double[]> Variances { get; set; }
        int KFoldValue { get; set; }
        NNClassifier[] LeadClassifiers { get; set; }
        NNClassifier[] FragHeFeClassifiers { get; set; }
        NNClassifier[] PrLeClassifiers { get; set; }
        NNClassifier[] ElMuPiClassifiers { get; set; }
        const string dataLearnLead = "../../train_data/trainLeadMulti.json";
        const string dataLearnFragHeFe = "../../train_data/trainFragHeFeNew.json";
        const string dataLearnPrLe = "../../train_data/trainPrLe_ElMuPi.json";
        const string dataLearnElMuPi = "../../train_data/trainElMuPi.json";
        public TestCrossValidateSimple(int kFoldValue, int repetitionsCount)
        {
            KFoldValue = kFoldValue;
            LeadClassifiers = new NNClassifier[repetitionsCount];
            FragHeFeClassifiers = new NNClassifier[repetitionsCount];
            PrLeClassifiers = new NNClassifier[repetitionsCount];
            ElMuPiClassifiers = new NNClassifier[repetitionsCount];

        }
        public void PrepareData()
        {
            NNInputProcessor preprocessor = new NNInputProcessor();
            Interval[] intervalsLead;//= preprocessor.CalculateNormIntervals(dataLearnLead, DefaultLearner.ValidFields);
            Interval[] intervalsFragHeFe ;//= preprocessor.CalculateNormIntervals(dataLearnFragHeFe, DefaultLearner.ValidFields);
            Interval[] intervalsPrLe ;// preprocessor.CalculateNormIntervals(dataLearnPrLe, DefaultLearner.ValidFields);
            Interval[] intervalsElMuPi;//= preprocessor.CalculateNormIntervals(dataLearnElMuPi, DefaultLearner.ValidFields);

            for (int i = 0; i < LeadClassifiers.Length; i++)
            {
                
                string name = "lead";
                string[] outputClasses = DefaultLearner.OutputClasses[name];
                LeadClassifiers[i] =null;//new NNClassifier(DefaultLearner.ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervalsLead, outputClasses, name);
                LeadClassifiers[i].TrainProportion = new Interval(0, 0.8);

                name = "fragHeFe";
                outputClasses = DefaultLearner.OutputClasses[name];
                FragHeFeClassifiers[i] = null;// new NNClassifier(DefaultLearner.ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervalsFragHeFe, outputClasses, name);

                name = "prLe";
                outputClasses = DefaultLearner.OutputClasses[name];
                PrLeClassifiers[i] = null;// new NNClassifier(DefaultLearner.ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervalsPrLe, outputClasses, name);

                name = "elMuPi";
                outputClasses = DefaultLearner.OutputClasses[name];
                ElMuPiClassifiers[i] = null;//new NNClassifier(DefaultLearner.ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervalsElMuPi, outputClasses, name);


            }
        }
        public void Test(string v = null)
        {
            double[] accuraciesLead = new double[LeadClassifiers.Length];
            double[] accuraciesFragHeFe = new double[LeadClassifiers.Length];
            double[] accuraciesPrLe = new double[LeadClassifiers.Length];
            double[] accuraciesElMuPi = new double[LeadClassifiers.Length];
            for (int i = 0; i < LeadClassifiers.Length; i++)
            {
                int epochSize = 32;
                int learnIterationsCount = 1;
                accuraciesLead[i] = LeadClassifiers[i].CrossValidate(KFoldValue, epochSize, dataLearnLead, DefaultLearner.ValidFields, learnIterationsCount).Average();


                epochSize = 8;
                learnIterationsCount = 4;
                accuraciesFragHeFe[i] = FragHeFeClassifiers[i].CrossValidate(KFoldValue, epochSize, dataLearnFragHeFe, DefaultLearner.ValidFields, learnIterationsCount).Average();

                epochSize = 6;
                learnIterationsCount = 1;
                accuraciesPrLe[i] = PrLeClassifiers[i].CrossValidate(KFoldValue, epochSize, dataLearnPrLe, DefaultLearner.ValidFields, learnIterationsCount).Average();

                epochSize = 8;
                learnIterationsCount = 1;
                accuraciesElMuPi[i] = ElMuPiClassifiers[i].CrossValidate(KFoldValue, epochSize, dataLearnElMuPi, DefaultLearner.ValidFields, learnIterationsCount).Average();

            }
            ExpectedValues.Add("lead", accuraciesLead);
            ExpectedValues.Add("fragHeFe", accuraciesFragHeFe);
            ExpectedValues.Add("prLe", accuraciesPrLe);
            ExpectedValues.Add("elMuPi", accuraciesElMuPi);
        }
    }
    //class TestAllWithSingleClassifier
    static class DefaultLearner
    {
        public static ClusterAttribute[] ValidFields { get; } = new MultiLayeredClassifier().ValidFields;
        const string dataLearnLead = "../../train_data/trainLeadMulti.json";
        const string dataLearnFrag = "../../train_data/trainFragHeFeNew.json";
        const string dataLearnPrLe = "../../train_data/trainPrLe_ElMuPi.json";
        const string dataLearnElMuPi = "../../train_data/trainElMuPi.json";
        public static Dictionary<string, string[]> OutputClasses = new Dictionary<string, string[]>();
        static DefaultLearner()
        {
            OutputClasses.Add("lead", new string[] {
                 "lead",
                  "he",
                 "fe",
                 "frag",
                 "other"
             });
            OutputClasses.Add("fragHeFe", new string[] {
                 "frag",
                 "he",
                 "fe",
                 "other"
             });
            OutputClasses.Add("prLe", new string[] {
                 "proton",
                 "elMuPi",
                 "low_electr",
            });
            OutputClasses.Add("elMuPi", new string[] {
                 "muon",
                 "electron",
                 "pion",
                 "elPi0"
            });
        }
        public static (NNClassifier classifier, double success) LearnFragHeFe()
        {
            const string name = "fragHeFe";
            var outputClasses = OutputClasses["name"];
            NNInputProcessor preprocessor = new NNInputProcessor();
            var intervals = preprocessor.CalculateNormIntervals(dataLearnFrag, ValidFields);
                        const int epochSize = 8;
            NNClassifier fragClassifier = null; //= new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervals, outputClasses, name);
            const int learnIterations = 4;
            double success = 0;
            for (int i = 0; i < learnIterations; i++)
            {
                //if(i == learnIterations - 1)
                 //   success = fragClassifier.Learn(epochSize, dataLearnFrag, ValidFields, 0, eval: false);

            };
            return (fragClassifier, success);
        }
        public static (NNClassifier classifier, double success) LearnLead()
        {
            const string name = "lead";
            string[] outputClasses = OutputClasses[name];
                  
            NNInputProcessor preprocessor = new NNInputProcessor();
           // Interval[] intervals = preprocessor.CalculateNormIntervals(dataLearnLead, ValidFields);
            int epochSize = 32;
            NNClassifier nnClassifier = null;// = new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervals, outputClasses, name);
            var success = nnClassifier.Learn("",0);
            return (nnClassifier, success);
        }
        public static (NNClassifier classifier, double success) LearnPrLe()
        {
            const string name = "prLe";
            string[] outputClasses = OutputClasses[name];
            NNInputProcessor preprocessor = new NNInputProcessor();
            //Interval[] intervals = preprocessor.CalculateNormIntervals(dataLearnPrLe, ValidFields);
            int epochSize = 6;
            NNClassifier multiClassifier = null;//new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervals, outputClasses, name);
            var success = multiClassifier.Learn("", 0);
            return (multiClassifier, success);
        }
        public static (NNClassifier classifier, double success) LearnElMuPi()
        {
            const string name = "elMuPi";
            string[] outputClasses = OutputClasses[name];
            int epochSize = 8;
            NNInputProcessor preprocessor = new NNInputProcessor();
            //Interval[] intervals = preprocessor.CalculateNormIntervals(dataLearnElMuPi, ValidFields);
            NNClassifier multiClassifier = null;//new NNClassifier(ValidFields.Length, outputClasses.Length, new int[] { 13, 13 }, new SigmoidFunction(1), intervals, outputClasses, name);
           // multiClassifier.Learn(epochSize, dataLearnElMuPi, ValidFields, 0, eval: false);
            var success = multiClassifier.Learn("", 0);
            return (multiClassifier, success);
        }       
    }
}
