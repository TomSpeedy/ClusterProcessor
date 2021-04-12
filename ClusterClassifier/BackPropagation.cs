using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ClassifierForClusters
{
    class Gradient
    {
        public Dictionary<Edges, double[][]> Edges = new Dictionary<Edges, double[][]>();
        public Dictionary<INNLayer, double[]> Biases = new Dictionary<INNLayer, double[]>();
        public Dictionary<INNLayer, double[]> Values = new Dictionary<INNLayer, double[]>();
        public double SquaredSize { get; set; } = 0;
        public double GetSize() => Math.Sqrt(SquaredSize);
        public static Gradient operator +(Gradient left, Gradient right)
        {
            Gradient newGrad = new Gradient();
            newGrad.Edges = new Dictionary<Edges, double[][]>();
            newGrad.Biases = new Dictionary<INNLayer, double[]>();
            newGrad.Values = new Dictionary<INNLayer, double[]>();
            newGrad.SquaredSize = 0;
            foreach (var leftE in left.Edges)
            {
                var leftMatrix = leftE.Value;
                var rightMatrix = right.Edges[leftE.Key];
                double[][] newMatrix = new double[leftMatrix.Length][];
                for (int i = 0; i < leftE.Value.Length; ++i)
                    newMatrix[i] = leftMatrix[i].Zip(rightMatrix[i], (a, b) => { newGrad.SquaredSize += (a+b) * (a+b); return a + b; }).ToArray();
                newGrad.Edges.Add(leftE.Key, newMatrix);
            }
            foreach (var leftB in left.Biases)
            {
                var rightB = right.Biases[leftB.Key];
                newGrad.Biases.Add(leftB.Key, leftB.Value.Zip(rightB, (a, b) => { newGrad.SquaredSize += (a + b) * (a + b); return a + b; }).ToArray());
            }
            foreach (var leftV in left.Values)
            {
                var rightV = right.Values[leftV.Key];
                newGrad.Values.Add(leftV.Key, leftV.Value.Zip(rightV, (a, b) => { newGrad.SquaredSize += (a + b) * (a + b); return a + b; }).ToArray());
            }
            return newGrad;

        }
        public void Normalize()
        {
            const double newNorm = 50d;
            var currentNorm = (1/newNorm)*GetSize();
            foreach (var (layer,edges) in Edges)
            {
                for (int i = 0; i < edges.Length; i++)
                {

                    for (int j = 0; j < edges[i].Length; j++)
                    {
                        edges[i][j] /= currentNorm;
                    }

                }
            }
            foreach (var (layer, biases) in Biases)
            {
                for (int j = 0; j < biases.Length; j++)
                {
                    biases[j] /= currentNorm;
                }
            }
            foreach (var (layer, values) in Values)
            {
                for (int j = 0; j < values.Length; j++)
                {
                    values[j] /= currentNorm;
                }
            }
            SquaredSize = newNorm * newNorm;
        }
    }
    class BackPropagation 
    {
        private ISqueezeFunction SqueezeFunction;
        private double TrainingSetSize = 1; //set in Calculate Gradient Size
        public BackPropagation(ISqueezeFunction squeezeFunction)
        {
            SqueezeFunction = squeezeFunction;
        }
        public Gradient Gradient = new Gradient();
        public void CalculateGradient(MLP network, double[] expectedResult, double trainingSetSize = 1)
        {
            TrainingSetSize = trainingSetSize;
            Gradient = new Gradient();
            ICostFunction costFunction = new SquareDiffCostFunction();
            
            Gradient.Values.Add(network.outputLayer, network.outputLayer.Neurons.Select((neuron) => costFunction.CalcDerivative(neuron, expectedResult[neuron.Id])).ToArray());

            Gradient.Biases.Add(network.outputLayer, CalculateBiasDerivative(network.outputLayer));

            Gradient.Edges.Add(network.outputLayer.LeftEdges,
                CalculateEdgeDerivative(network.outputLayer.LeftEdges, network.outputLayer.Previous, network.outputLayer));
            for (int i = network.NNLayers.Count - 2; i > 0; i--)
            {
                var currentLayer = (IFullyConnectableLayer)network.NNLayers[i];
                var previousLayer = (IRightConnectableLayer)network.NNLayers[i - 1];
                Gradient.Values.Add(currentLayer, CalculateValueDerivative(currentLayer));
                Gradient.Biases.Add(currentLayer, CalculateBiasDerivative(currentLayer));

                Gradient.Edges.Add(currentLayer.LeftEdges,
                    CalculateEdgeDerivative(currentLayer.LeftEdges, previousLayer, currentLayer));

            }
            Gradient.Normalize();
        }

        //calculates derivatives of the edges
        double[][] CalculateEdgeDerivative(Edges edges, IRightConnectableLayer left, ILeftConnectableLayer right)
        {
            var result = new double[right.GetSize()][];
            for (int i = 0; i < right.Neurons.Length; i++)
            {
                result[i] = new double[left.GetSize()];
                for (int j = 0; j < left.Neurons.Length; j++)
                {
                    result[i][j] = left.Neurons[j].Value *  SqueezeFunction.CalcDerivative(right.Neurons[i].Value) * Gradient.Values[right][i];
                    result[i][j] /= TrainingSetSize;
                    Gradient.SquaredSize += result[i][j] * result[i][j];
                }

            }
            return result;
        }
        //calculates derivative of the biases
        double[] CalculateBiasDerivative(INNLayer nnLayer)
        {
            var result = new double[nnLayer.GetSize()];
            for (int i = 0; i < nnLayer.Neurons.Length; i++)
            {
                result[i] = SqueezeFunction.CalcDerivative(nnLayer.Neurons[i].Value)  * Gradient.Values[nnLayer][i];
                result[i] /= TrainingSetSize;
                Gradient.SquaredSize += result[i] * result[i];
            }
            return result;
        }
        //calculate derivative of the values (activation values)
        double[] CalculateValueDerivative(IRightConnectableLayer nnLayer)
        {
            var result = new double[nnLayer.GetSize()];
            for (int i = 0; i < nnLayer.Neurons.Length; i++)
            {
                for (int j = 0; j < nnLayer.Next.Neurons.Length; j++)
                {
                    result[i] += nnLayer.RightEdges[j][i] * SqueezeFunction.CalcDerivative(nnLayer.Next.Neurons[j].Value) * Gradient.Values[nnLayer.Next][j];
                    result[i] /= TrainingSetSize;
                    Gradient.SquaredSize += result[i] * result[i];
                }
            }
            return result;
        }
    }
}
