using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ClusterClassifier
{
    
    class BackPropagation 
    {
        private ISqueezeFunction SqueezeFunction;
        public BackPropagation(ISqueezeFunction squeezeFunction)
        {
            SqueezeFunction = squeezeFunction;
        }
       
        public Dictionary<Edges , double[][]> GradientEdges = new Dictionary<Edges, double[][]>();
        public Dictionary<INNLayer, double[]> GradientBiases = new Dictionary<INNLayer, double[]>();
        private Dictionary<INNLayer, double[]> GradientValues = new Dictionary<INNLayer, double[]>();
        public void CalculateGradient(MLP network, double[] expectedResult)
        {
            ICostFunction costFunction = new SquareDiffCostFunction(expectedResult);
            GradientValues.Add(network.outputLayer, network.outputLayer.Neurons.Select((neuron) => costFunction.calcDerivative(neuron)).ToArray());

            GradientBiases.Add(network.outputLayer, CalculateBiasDerivative(network.outputLayer));

            GradientEdges.Add(network.outputLayer.LeftEdges,
                CalculateEdgeDerivative(network.outputLayer.LeftEdges, network.outputLayer.Previous, network.outputLayer));
            for (int i = network.NNLayers.Count - 2; i > 0; i--)
            {
                var currentLayer = (IFullyConnectableLayer)network.NNLayers[i];
                var previousLayer = (IRightConnectableLayer)network.NNLayers[i - 1];
                GradientValues.Add(currentLayer, CalculateValueDerivative(currentLayer));
                GradientBiases.Add(currentLayer, CalculateBiasDerivative(currentLayer));

                GradientEdges.Add(currentLayer.LeftEdges,
                    CalculateEdgeDerivative(currentLayer.LeftEdges, previousLayer, currentLayer));

            }
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
                    result[i][j] = left.Neurons[j].Value * SqueezeFunction.CalcDerivative(right.Neurons[j].NonSqueezedValue) * GradientValues[right][i];
                    
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
                result[i] = SqueezeFunction.CalcDerivative(nnLayer.Neurons[i].NonSqueezedValue)  * GradientValues[nnLayer][i];
                
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
                    result[i] += nnLayer.RightEdges[j][i] * SqueezeFunction.CalcDerivative(nnLayer.Neurons[j].NonSqueezedValue) * GradientValues[nnLayer.Next][j];
                }
            }
            return result;
        }
    }
}
