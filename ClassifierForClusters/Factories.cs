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
    /// <summary>
    /// creates activation functions for MLP
    /// </summary>
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
    /// <summary>
    /// creates a trainable classifier based on a string
    /// </summary>
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
    /// <summary>
    /// creates learning algorithm for a given activation network
    /// </summary>
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

}
