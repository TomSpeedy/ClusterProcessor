using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterClassifier
{
    interface ISqueezeFunction
    {
        double Squeeze(double value);
        double CalcDerivative(double value);
        
    }
    class LogSigmSqeezeFunction : ISqueezeFunction
    {
        public double Squeeze(double value)
        {
            return (1.0 / (1.0 + Math.Pow(Math.E, -value)));
        }
        public double CalcDerivative(double value)
        {
            return Squeeze(value) * (1 - Squeeze(value));
        }
    }
    class ReluSqeezeFunction : ISqueezeFunction
    {
        public double Squeeze(double value)
        {
            return Math.Max(0d, value);
        }
        public double CalcDerivative(double value)
        {
            if (value > 0)
                return 1;
            return 0;
        }
    }
}
