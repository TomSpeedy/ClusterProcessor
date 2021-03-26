using System;
using System.Collections.Generic;
using System.Text;
using Accord.Neuro.Learning;
using Accord.Neuro;
using Accord.Statistics;
namespace ClusterClassifier
{
    interface ISqueezeFunction
    {
        double Squeeze(double value);
        double CalcDerivative(double squeezedValue);
        
    }
    class LogSigmSqeezeFunction : ISqueezeFunction
    {
        public double Squeeze(double value)
        {
            return (1.0 / (1.0 + Math.Pow(Math.E, -value)));
        }
        public double CalcDerivative(double squeezedValue)
        {
            return squeezedValue * (1 - squeezedValue);
        }
    }
    class ReluSqeezeFunction : ISqueezeFunction, IActivationFunction
    {
        const double leakConstant = 0.01;
        public double Squeeze(double value)
        {
            if (value >= 0)
            {
                return value;
            }
            return leakConstant * value;
        }
        public double CalcDerivative(double squeezedValue)
        {
            if (squeezedValue > 0)
                return 1;
            if (squeezedValue == 0)
                return 0;
            return leakConstant;
        }
        public double Function(double value) => Squeeze(value);
        public double Derivative(double value) => CalcDerivative(value);
        public double Derivative2(double value) => 0;
    }
}
