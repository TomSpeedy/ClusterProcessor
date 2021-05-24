using Accord.Math;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using ClusterCalculator;

namespace ClassifierForClusters
{
    /// <summary>
    /// represents a prediction of the classifier about a single cluster
    /// </summary>
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

        }
        public virtual double CalcConfidence()
        {
            if (MostProbableClassName == "unclassified")
                return 0;
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
}
