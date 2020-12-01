using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterClassifier
{

    class Neuron
    {
        public double Bias { get; set; }
        public double Value { get; set; }
        public int Id { get; }
        public double NonSqueezedValue { get; set; } = 0;
        public Neuron(int id, bool randomize)
        {
            if (randomize)
            {
                Random random = new Random();
                Bias = random.NextDouble() / 5;
                Value = 0;
            }
            else
            {
                Bias = 0D;
                Value = 0D;
            }
            Id = id;
        }
        public Neuron(int id, double value, double bias = 0)
        {
            Id = id;
            Value = value;
            Bias = bias;          
        }
        
    }
}
