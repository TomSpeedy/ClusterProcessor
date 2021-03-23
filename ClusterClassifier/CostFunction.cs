using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterClassifier
{
    interface ICostFunction
    {
        double CalcValue(Neuron[] actual, double[] expected);
        double CalcDerivative(Neuron neuron, double expected);
    }
    class SquareDiffCostFunction : ICostFunction
    {

        public double CalcValue(Neuron[] actual, double[] expected)
        {
            double finalCost = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                finalCost += (actual[i].Value - expected[i]) * (actual[i].Value - expected[i]);
            }
            return finalCost;
        }
        public double CalcDerivative(Neuron neuron, double expected)
        {
            return 2 * (neuron.Value - expected);
        }
    }

}
