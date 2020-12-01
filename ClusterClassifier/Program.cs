using System;

namespace ClusterClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            uint[] layerSizes = { 2, 2 };
            var myNN = new MLP(layerSizes);
            myNN.SetInput();
            myNN.Process();
        }
    }
}

