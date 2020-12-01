using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterClassifier
{
    class Edges
    {
        private double[][] Matrix { get; set; }
        public Edges(uint leftNCount, uint rightNCount, bool randomize = true)
        {

            Random random = new Random();
            Matrix = new double[rightNCount][];
            for (int i = 0; i < Matrix.Length; i++)
            {
                Matrix[i] = new double[leftNCount];
                for (int j = 0; j < Matrix[i].Length; j++)
                {
                    Matrix[i][j] = random.NextDouble();
                }
            }

        }
        public double[] this[int index]
        {
            get
            {
                return Matrix[index];
            }
            set 
            {
                Matrix[index] = value;
            }
        }
    }
}
