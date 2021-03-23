using System;
using System.Collections.Generic;
using Accord.Neuro.Learning;
using Accord.Neuro;
using Accord.Statistics;
namespace ClusterClassifier
{
    class Program
    {
        static Random rand = new Random();
        static double[] SwitchRandomVal(double[] input)
        {
            double[] result = new double[input.Length];
            input.CopyTo(result, 0);
            int swapIndex = rand.Next(0, input.Length - 1);
            result[swapIndex] = result[swapIndex] == 1d ? 0 : 1;
            return result;
        }

        static void Main(string[] args)
        {
            double[] one = new double[] { 1, 0, 0, 1, 0, 0, 1, 0, 0 };
            double[] one2 = new double[] { 0, 1, 0, 0, 1, 0, 0, 1, 0 };
            double[] two = new double[] { 1, 1, 1, 0, 1, 0, 1, 1, 1 };
            double[] four = new double[] { 0, 1, 0, 0, 1, 1, 0, 0, 1 };
            uint[] layerSizes = { 16, 16, 16 };
            // initialize input and output values
            double[][] input = new double[6][];
            input[0] = rand.NextDouble() < 0.3 ? one2 : one;
            input[1] = rand.NextDouble() < 0.1 ? SwitchRandomVal(two) : two;
            input[2] = rand.NextDouble() < 0.1 ? SwitchRandomVal(four) : four;
            input[3] = rand.NextDouble() < 0.3 ? one2 : one;
            input[4] = rand.NextDouble() < 0.1 ? SwitchRandomVal(two) : two;
            input[5] = rand.NextDouble() < 0.1 ? SwitchRandomVal(four) : four;
        
        double[][] output = new double[6][] {
   new double[] { 1, 0, 0 }, new double[] { 0, 1, 0 },
    new double[] { 0, 0, 1 }, new double[] { 1, 0, 0 }, new double[] { 0, 1, 0 }, new double[] { 0, 0, 1 } };
    
            // create neural network
            
            ActivationNetwork network = new ActivationNetwork(
                new RectifiedLinearFunction(),
                9, // two inputs in the network
                16, // two neurons in the first layer
                16,
                3); // one neuron in the second layer
                    // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            teacher.LearningRate = 1;
            teacher.Momentum = 1;
            // loop
            int i = 0; 
            while (i < 500000)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output);
                // check error value to see if we need to stop
                // ...
                i++;
                input[0] = rand.NextDouble() < 0.3 ? one2 : one;
                input[1] = rand.NextDouble() < 0.1 ? SwitchRandomVal(two) : two;
                input[2] = rand.NextDouble() < 0.1 ? SwitchRandomVal(four) : four;
                input[3] = rand.NextDouble() < 0.3 ? one2 : one;
                input[4] = rand.NextDouble() < 0.1 ? SwitchRandomVal(two) : two;
                input[5] = rand.NextDouble() < 0.1 ? SwitchRandomVal(four) : four;
            }
            double err = teacher.RunEpoch(input, output);
            var result = network.Compute(one);
            result = network.Compute(two);
            result = network.Compute(SwitchRandomVal(four));
            result = network.Compute(SwitchRandomVal(SwitchRandomVal(four)));
            /*
            const int testSetSize = 6;
            ICostFunction costFunction = new SquareDiffCostFunction();
            var myNN = new MLP(layerSizes, costFunction);
            double[] one = new double[] { 1, 0, 0, 1, 0, 0, 1, 0, 0 };
            double[] one2 = new double[] { 0, 1, 0, 0, 1, 0, 0, 1, 0 };
            double[] two = new double[] { 1, 1, 1, 0, 1, 0, 1, 1, 1 };
            double[] four = new double[] { 0, 1, 0, 0, 1, 1, 0, 0, 1 };
            myNN.CreateInput(new double[] { 1, 0, 0, 1, 0, 0, 1, 0, 0 });
            myNN.CreateOutput();
            Gradient gradient = new Gradient();
            for (int j = 0; j < 15000; j++)
            {
                if(j==14999)
                { }
                List<double[]> inputTrainSet = new List<double[]>();
                List<double[]> outputTrainSet = new List<double[]>();
                for (int i = 0; i < testSetSize; i++)
                {                
                    double[] input = new double[myNN.inputLayer.GetSize()];
                    double[] output = new double[myNN.outputLayer.GetSize()];
                    if (i % 3 == 0)
                    {
                        if (rand.NextDouble() < 0.3)
                            input = one2;
                         else
                          input = one;
                        
                        output = new double[] { 1, 0, 0 };
                    }
                    else if (i % 3 == 1)
                    {
                        if (rand.NextDouble() < 0.1)
                            myNN.SetInput(SwitchRandomVal(two));
                        else
                        input = two;
                        output = new double[] { 0, 1, 0 };
                    }
                    else
                    {

                        if (rand.NextDouble() < 0.1)
                            input = SwitchRandomVal(four);
                        else
                            input = four;
                            output = new double[] { 0, 0, 1 };
                    }
                    inputTrainSet.Add(input);
                    outputTrainSet.Add(output);

                }
                myNN.ProcessTrainingSet(inputTrainSet, outputTrainSet);

            }
            myNN.SetInput(two);
            myNN.Process();
            myNN.SetInput(one2);
            myNN.Process();
            myNN.SetInput(one);
            myNN.Process();
            myNN.SetInput(SwitchRandomVal(SwitchRandomVal(four)));
            myNN.Process();*/
        }
        }
}

