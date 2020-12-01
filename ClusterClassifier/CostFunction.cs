using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterClassifier
{
    interface ICostFunction
    {
        double CalcValue(Neuron[] actual);
        double calcDerivative(Neuron neuron);
    }
    class SquareDiffCostFunction : ICostFunction
    {
        private  double[] Expected { get; }
        public SquareDiffCostFunction(double[] expected)
        {
            Expected = expected;
        }
        public double CalcValue(Neuron[] actual)
        {
            double finalCost = 0;
            for (int i = 0; i < Expected.Length; i++)
            {
                finalCost += (actual[i].Value - Expected[i]) * (Expected[i] - actual[i].Value);
            }
            return finalCost;
        }
        public double calcDerivative(Neuron neuron)
        {
            return 2 * (neuron.Value - Expected[neuron.Id]);
        }
    }

}
