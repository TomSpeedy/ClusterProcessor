using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterClassifier
{
    class MLP
    {
        public IRightConnectableLayer inputLayer;
        public ILeftConnectableLayer outputLayer;
        private void ConnectTwoLayers(IRightConnectableLayer leftLayer, ILeftConnectableLayer rightLayer)
        {
            var edges = new Edges(leftLayer.GetSize(), rightLayer.GetSize(), randomize: true);
            leftLayer.RightEdges = edges;
            rightLayer.LeftEdges = edges;
            leftLayer.Next = rightLayer;
            rightLayer.Previous = leftLayer;
        }
        public List<INNLayer> NNLayers = new List<INNLayer>();
        public MLP(IList<uint> layerSizes)
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
        }
        //TODO : reformat setting input/output
        public void SetInput()
        {
            double[] input = { 1, 2};
            inputLayer = new InputLayer(input);
            NNLayers.Insert(0, inputLayer); //should be called only once ever
            ConnectTwoLayers(inputLayer, (ILeftConnectableLayer)NNLayers[1]);
        }
        public void SetOutput()
        {
            uint outputSize = 3;
            outputLayer = new OutputLayer(outputSize);
            NNLayers.Add(outputLayer);
            ConnectTwoLayers((IRightConnectableLayer)NNLayers[NNLayers.Count - 2], outputLayer);
        }

        public void BackPropagate()
        { 
            
        }
        public void Process()
        {
            ISqueezeFunction sqFunction = new LogSigmSqeezeFunction();
            for (int i = 1; i < NNLayers.Count; i++)
            {
                ((ILeftConnectableLayer)NNLayers[i]).UpdateLayer(sqFunction);
            }
        }

    }
}
