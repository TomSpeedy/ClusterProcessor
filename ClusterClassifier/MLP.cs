using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterClassifier
{
    class MLP
    {
        public IRightConnectableLayer inputLayer;
        public ILeftConnectableLayer outputLayer;
        private double StepSize = 0.5;
        ICostFunction CostFunction;
        private void ConnectTwoLayers(IRightConnectableLayer leftLayer, ILeftConnectableLayer rightLayer)
        {
            var edges = new Edges(leftLayer.GetSize(), rightLayer.GetSize(), randomize: true);
            leftLayer.RightEdges = edges;
            rightLayer.LeftEdges = edges;
            leftLayer.Next = rightLayer;
            rightLayer.Previous = leftLayer;
        }
        public List<INNLayer> NNLayers = new List<INNLayer>();
        public MLP(IList<uint> layerSizes, ICostFunction costFunction)
        {
            for (int i = 0; i < layerSizes.Count; i++)
            {
                var newLayer = new HiddenLayer(layerSizes[i]);
                NNLayers.Add(newLayer);
            }
            for (int i = 0; i < layerSizes.Count - 1; i++)
            {
                ConnectTwoLayers((IRightConnectableLayer)NNLayers[i], (ILeftConnectableLayer)NNLayers[i + 1]);
            }
            CostFunction = costFunction;
        }
        public void SetInput(double[] input)
        {
            if (input.Length > inputLayer.Neurons.Length)
                Console.WriteLine("Input is too large for created input layer");
            for (int i = 0; i < input.Length; ++i)
            {
                inputLayer.Neurons[i].Value = input[i];
            }
        }
        public void CreateInput(double [] input)
        {
            //double[] input = { 1, 2};
            inputLayer = new InputLayer(input);
            if (NNLayers[0].GetType() == typeof(InputLayer))
                NNLayers.RemoveAt(0);
            NNLayers.Insert(0, inputLayer); //should be called only once ever
            ConnectTwoLayers(inputLayer, (ILeftConnectableLayer)NNLayers[1]);
        }
        public void CreateOutput(uint outputClassesCount = 3)
        {
            outputLayer = new OutputLayer(outputClassesCount);
            NNLayers.Add(outputLayer);
            ConnectTwoLayers((IRightConnectableLayer)NNLayers[NNLayers.Count - 2], outputLayer);
        }

        public Gradient CalculateGradient(double[] expectedResult, double testSetSize)
        {
            ISqueezeFunction sqFunction = new ReluSqeezeFunction();
            BackPropagation backPropagation = new BackPropagation(sqFunction);
            backPropagation.CalculateGradient(this, expectedResult, testSetSize);
            return backPropagation.Gradient;
        }
        public void ProcessTrainingSet(IList<double[]> trainingSetInputs, IList<double[]> trainingSetOutputs)
        {
            Gradient avgGradient = new Gradient();
            double avgCostFunc = 0;
            for (int i = 0; i < trainingSetInputs.Count; ++i)
            {
                SetInput(trainingSetInputs[i]);
                Process();              
                if (i == 0)
                    avgGradient = CalculateGradient(trainingSetOutputs[i], trainingSetInputs.Count);
                else
                    avgGradient += CalculateGradient(trainingSetOutputs[i], trainingSetInputs.Count);
                avgCostFunc += CostFunction.CalcValue(outputLayer.Neurons, trainingSetOutputs[i]) / trainingSetInputs.Count;
            }
            avgGradient.Normalize();
            MakeStep(avgGradient, avgCostFunc, trainingSetInputs, trainingSetOutputs);

        }
        private void DescentValues(Gradient gradient, double stepSize = 0.05)
        {
            for (int i = 1; i < NNLayers.Count; ++i)
            {
                for (int j = 0; j < NNLayers[i].Neurons.Length; ++j)
                {
                    NNLayers[i].Neurons[j].Bias -= stepSize * gradient.Biases[NNLayers[i]][j];
                }
            }
            for (int i = 1; i < NNLayers.Count; ++i)
            {
                var edges = ((ILeftConnectableLayer)NNLayers[i]).LeftEdges;
                for (int j = 0; j < edges.Matrix.Length; ++j)
                {
                    for (int k = 0; k < edges.Matrix[j].Length; ++k)
                    {
                        edges.Matrix[j][k] -= stepSize * gradient.Edges[edges][j][k];
                    }
                }
            }
        }

        public void MakeStep(Gradient avgGradient, double avgCostFuncValue, IList<double[]> trainingSetInputs, IList<double[]> trainingSetOutputs)
        {
            const double epsilon = 0.0005;
            //DescentValues(avgGradient);
            
            while (!TryStep(avgGradient, avgCostFuncValue, trainingSetInputs, trainingSetOutputs) && StepSize > epsilon)
                StepSize /= 2d;
            //if we succeeded increase step size for the next step
            StepSize *= 1.05;
        }
        private bool TryStep(Gradient avgGradient, double avgCostFuncValue, IList<double[]> trainingSetInputs, IList<double[]> trainingSetOutputs)
        {
            //TODO optimize performance by creating new NN and set its biases and weights (no restore needed)
            const double steepness = 0.000001d;
            DescentValues(avgGradient, StepSize);
            //calculate avg( F(a-stepsize*gradient) ) = newAVgCostVal
            double newAvgCostFuncVal = 0;
            for (int i = 0; i < trainingSetInputs.Count; ++i)
            {
                SetInput(trainingSetInputs[i]);
                Process();
                newAvgCostFuncVal += CostFunction.CalcValue(outputLayer.Neurons, trainingSetOutputs[i]) / trainingSetInputs.Count;
            }
            bool isValidPoint = newAvgCostFuncVal < avgCostFuncValue; //- steepness * StepSize * avgGradient.GetSize();
            //restore gradient if step size is too large
            if (!isValidPoint)
                DescentValues(avgGradient, -StepSize);
            return isValidPoint;
        }
        public void Process()
        {
            ISqueezeFunction sqFunction = new ReluSqeezeFunction();
            for (int i = 1; i < NNLayers.Count; i++)
            {
                ((ILeftConnectableLayer)NNLayers[i]).UpdateLayer(sqFunction);
            }
        }

    }
}
