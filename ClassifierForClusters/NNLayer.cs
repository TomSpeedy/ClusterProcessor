using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace ClassifierForClusters
{
    interface INNLayer
    {
        Neuron[] Neurons { get; set; }
        uint GetSize(); 
    }
    interface ILeftConnectableLayer : INNLayer
    {
        Edges LeftEdges { get; set; }   
        IRightConnectableLayer Previous { get; set; }
        void UpdateLayer(ISqueezeFunction sqFunction);
    }
    interface IRightConnectableLayer : INNLayer
    {
        Edges RightEdges { get; set; }
        ILeftConnectableLayer Next { get; set; }
    }
    interface IFullyConnectableLayer : IRightConnectableLayer, ILeftConnectableLayer
    { 
    
    }
    class OutputLayer : ILeftConnectableLayer
    {
        public Neuron[] Neurons { get; set; }
        public Edges LeftEdges { get; set; }
        public IRightConnectableLayer Previous { get; set; }
        public uint GetSize()
        {
            if (Neurons == null)
                return 0;
            return (uint)Neurons.Length;
        }
        public OutputLayer(uint neuronCount)
        {
            Neurons = new Neuron[neuronCount];
            for (int i = 0; i < neuronCount; i++)
            {
                Neurons[i] = new Neuron(id: i, randomize: true);
            }
        }
        public void UpdateLayer(ISqueezeFunction sqFunction)
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Value = Neurons[i].Bias;
                for (int j = 0; j < Previous.Neurons.Length; j++)
                {
                    Neurons[i].Value += Previous.Neurons[j].Value * LeftEdges[i][j];
                }
                Neurons[i].NonSqueezedValue = Neurons[i].Value;
                Neurons[i].Value = sqFunction.Squeeze(Neurons[i].Value);
            }
        }
    }
    class InputLayer : IRightConnectableLayer
    {
        public Neuron[] Neurons { get; set; }
        public Edges RightEdges { get; set; }
        public IRightConnectableLayer Previous { get; set; }
        public ILeftConnectableLayer Next { get; set; }
        public uint GetSize()
        {
            if (Neurons == null)
                return 0;
            return (uint)Neurons.Length;
        }
        public InputLayer(IList<double> inputVector)
        {
            Neurons = new Neuron[inputVector.Count];
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(id: i, value: inputVector[i]);
            }
        }
    }
    class HiddenLayer : IFullyConnectableLayer
    {
        public Neuron[] Neurons { get; set; }
        public Edges LeftEdges { get; set; }
        public Edges RightEdges { get; set; }
        public IRightConnectableLayer Previous { get; set; }
        public ILeftConnectableLayer Next { get; set; }
        public uint GetSize()
        {
            if (Neurons == null)
                return 0;
            return (uint)Neurons.Length;
        }
        public HiddenLayer(uint neuronCount)
        { 
            Neurons = new Neuron[neuronCount];
            for (int i = 0; i < neuronCount; i++)
            {
                Neurons[i] = new Neuron(id: i, randomize : true);
            }           
        }
        public void UpdateLayer(ISqueezeFunction sqFunction)
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Value = Neurons[i].Bias;
                for (int j = 0; j < Previous.Neurons.Length; j++)
                {
                    Neurons[i].Value += Previous.Neurons[j].Value * LeftEdges[i][j];
                }
                Neurons[i].NonSqueezedValue = Neurons[i].Value;
                Neurons[i].Value = sqFunction.Squeeze(Neurons[i].Value);
            }
        }
    }
}
