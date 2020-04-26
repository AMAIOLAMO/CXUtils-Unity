using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using CXUtils.CodeUtils;

using Random = UnityEngine.Random;

namespace CXUtils.NeuralNetworks
{
    /// <summary> A neural network from Cx Utils (using Math Dot Net) </summary>
    public struct CXNeuralNetwork
    {
        #region Fields

        public Matrix<float> inputLayer;

        public Matrix<float> outputLayer;

        public List<Matrix<float>> hiddenLayers;

        public List<Matrix<float>> weights;

        public List<float> biases;

        public float fitness;

        #endregion

        public CXNeuralNetwork(int inputsCount, int hiddenLayerCount, int hiddenNeuronCount, int outputsCount)
        {
            inputLayer = Matrix<float>.Build.Dense(1, inputsCount);

            hiddenLayers = new List<Matrix<float>>();

            outputLayer = Matrix<float>.Build.Dense(1, outputsCount);

            weights = new List<Matrix<float>>();

            biases = new List<float>();

            fitness = 0;

            for (int i = 0; i < hiddenLayerCount; i++)
            {
                Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);

                hiddenLayers.Add(f);

                biases.Add(Random.Range(-1f, 1f));

                //WEIGHTS

                if (i == 0)
                {
                    Matrix<float> inputToH1 = Matrix<float>.Build.Dense(inputsCount, hiddenNeuronCount);
                    weights.Add(inputToH1);
                }

                Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
                weights.Add(HiddenToHidden);
            }

            Matrix<float> outputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, outputsCount);

            weights.Add(outputWeight);
            biases.Add(Random.Range(-1f, 1f));
        }

        public void ClearAllData()
        {
            inputLayer.Clear();
            hiddenLayers.Clear();
            outputLayer.Clear();
            weights.Clear();
            biases.Clear();
            fitness = 0;
        }

        /// <summary> Randomized weights </summary>
        public void RandomizeWeights()
        {
            for (int i = 0; i < weights.Count; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        weights[i][x, y] = Random.Range(-1f, 1f);
        }

        /// <summary> Runs this network </summary>
        public float[] RunNetwork(Func<Matrix<float>, Matrix<float>> ActivationFunction = null, params float[] inputs)
        {
            if (inputs.Length != inputLayer.ColumnCount)
                return null;

            ClearAllData();

            //recieving input
            for (int i = 0; i < inputLayer.ColumnCount; i++)
                inputLayer[0, i] = inputs[i];

            //maps all the value using tanh
            inputLayer = ActivationFunction(inputLayer);

            hiddenLayers[0] = ActivationFunction((inputLayer * weights[0]) + biases[0]);

            for (int i = 1; i < hiddenLayers.Count; i++)
                hiddenLayers[i] = ActivationFunction((hiddenLayers[i - 1] * weights[i]) + biases[i]);

            outputLayer = ActivationFunction((hiddenLayers[hiddenLayers.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]);

            float[] output = new float[outputLayer.ColumnCount];

            for (int i = 0; i < outputLayer.ColumnCount; i++)
                output[i] = outputLayer[0, i];

            return output;
        }

        /// <summary> Runs this network with Tanh activation function </summary>
        public float[] RunNetworkTanh(params float[] inputs) =>
            RunNetwork((x) => Matrix<float>.Tanh(x), inputs);

        /// <summary> Runs this network with Sigmoid activation function </summary>
        public float[] RunNetworkSigmoid(params float[] inputs)
        {
            return RunNetwork((x) =>
            {
                Matrix<float> result = Matrix<float>.Build.Dense(x.RowCount, x.ColumnCount);
                x.Map(MathUtils.Sigmoid_1, result);
                return result;
            },
            inputs);
        }
    }
}

