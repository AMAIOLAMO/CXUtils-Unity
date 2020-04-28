using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using CXUtils.CodeUtils;

using Random = UnityEngine.Random;

namespace CXUtils.Evolutions.NeuralNetworks
{
    /// <summary> A neural network from Cx Utils (using Math Dot Net) </summary>
    public struct CXNeuralNetwork
    {
        #region Fields

        #region Counts

        public int InputCount { get; private set; }

        public int HiddenLayerCount { get; private set; }

        public int HiddenNeuronCount { get; private set; }

        public int OutputCount { get; private set; }

        #endregion

        #region NNet Main

        public Matrix<float> inputLayer;

        public Matrix<float> outputLayer;

        public List<Matrix<float>> hiddenLayers;

        public List<Matrix<float>> weights;

        public List<float> biases;

        public float fitness;

        #endregion

        #endregion

        public CXNeuralNetwork(int inputCount, int hiddenLayerCount, int hiddenNeuronCount, int outputCount)
        {
            InputCount = inputCount;
            HiddenLayerCount = hiddenLayerCount;
            HiddenNeuronCount = hiddenNeuronCount;
            OutputCount = outputCount;

            inputLayer = Matrix<float>.Build.Dense(1, InputCount);

            hiddenLayers = new List<Matrix<float>>();

            outputLayer = Matrix<float>.Build.Dense(1, OutputCount);

            weights = new List<Matrix<float>>();

            biases = new List<float>();

            fitness = 0;
        }

        #region Setting NNet

        //Resets all the fields
        private void ResetFields()
        {
            inputLayer = Matrix<float>.Build.Dense(1, InputCount);

            hiddenLayers = new List<Matrix<float>>();

            outputLayer = Matrix<float>.Build.Dense(1, OutputCount);

            weights = new List<Matrix<float>>();

            biases = new List<float>();

            fitness = 0;
        }

        /// <summary> Overrides the current Neural network to a new neural network </summary>
        public void SetNeuralNetwork(int inputCount, int hiddenLayerCount, int hiddenNeuronCount, int outputCount)
        {
            InputCount = inputCount;
            HiddenLayerCount = hiddenLayerCount;
            HiddenNeuronCount = hiddenNeuronCount;
            OutputCount = outputCount;

            ResetFields();
        }
        #endregion

        #region Running Network

        /// <summary> Runs this network </summary>
        public float[] RunNetwork(Func<Matrix<float>, Matrix<float>> ActivationFunction = null, params float[] inputs)
        {
            if (inputs.Length != inputLayer.ColumnCount)
                return null;

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

        #endregion

        #region Script Utils

        /// <summary> Initializes the Neural network </summary>
        public void Initialize()
        {
            ClearAllData();

            for (int i = 0; i < HiddenLayerCount + 1; i++)
            {
                Matrix<float> f = Matrix<float>.Build.Dense(1, HiddenNeuronCount);

                hiddenLayers.Add(f);

                biases.Add(Random.Range(-1f, 1f));

                //WEIGHTS
                if (i == 0)
                {
                    Matrix<float> inputToH1 = Matrix<float>.Build.Dense(InputCount, HiddenNeuronCount);
                    weights.Add(inputToH1);
                }

                Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(HiddenNeuronCount, HiddenNeuronCount);
                weights.Add(HiddenToHidden);
            }

            Matrix<float> outputWeight = Matrix<float>.Build.Dense(HiddenNeuronCount, OutputCount);

            weights.Add(outputWeight);

            biases.Add(Random.Range(-1f, 1f));
        }

        /// <summary> Clears all the data </summary>
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
        public void RandomizeWeights() =>
            MapWeights(() => Random.Range(-1f, 1f));

        #region Map manipulation
        /// <summary> Iterates through all the weights and map them </summary>
        public void MapWeights(Func<CXNeuralNetwork, int, int, int, float> IteratingFunc)
        {
            for (int i = 0; i < weights.Count; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        weights[i][x, y] = IteratingFunc(this, i, x, y);
        }

        /// <summary> Iterates through all the weights and map them </summary>
        public void MapWeights(Func<int, int, int, float> IteratingFunc)
        {
            for (int i = 0; i < weights.Count; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        weights[i][x, y] = IteratingFunc(i, x, y);
        }

        /// <summary> Iterates through all the weights and map them </summary>
        public void MapWeights(Func<float> IteratingFunc)
        {
            for (int i = 0; i < weights.Count; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        weights[i][x, y] = IteratingFunc();
        }

        /// <summary> Iterates through all the weights </summary>
        public void IterateWeights(Action<float, int, int, int> IterateAction)
        {
            for (int i = 0; i < weights.Count; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        IterateAction(weights[i][x, y], i, x, y);
        }

        /// <summary> Randomize mutates the Neural Network </summary>
        /// <param name="mutateRate"> 0 ~ 1 mutating rate </param>
        public void Mutate(float mutateRate = .2f) =>
            MapWeights((origin, i, x, y) => MathUtils.FlipCoin(Random.Range(-1f, 1f), origin.weights[i][x, y], mutateRate));

        /// <summary> Crosses any two matching Neural networks </summary>
        public bool TryCrossOver(CXNeuralNetwork other, out CXNeuralNetwork OUT, float rate = .5f)
        {
            if (AllowedCrossOver(other))
            {
                OUT = new CXNeuralNetwork(InputCount, HiddenLayerCount, HiddenNeuronCount, OutputCount);

                CXNeuralNetwork This = this;

                OUT.MapWeights((NNet, i, x, y) =>
                    MathUtils.FlipCoin(rate) ? This.weights[i][x, y] : other.weights[i][x, y]);

                return true;
            }

            OUT = new CXNeuralNetwork();
            return false;
        }

        /// <summary> Crosses any two matching Neural networks </summary>
        public CXNeuralNetwork CrossOver(CXNeuralNetwork other, float rate = .5f)
        {
            CXNeuralNetwork This = this;
            This.TryCrossOver(other, out CXNeuralNetwork child, rate);
            return child;
        }

        #region Other helper utils
        /// <summary> Check's if this Neural network with other neural network matches and could cross over </summary>
        public bool AllowedCrossOver(CXNeuralNetwork other) =>
            InputCount.Equals(other.InputCount) && HiddenLayerCount.Equals(other.HiddenLayerCount) &&
            HiddenNeuronCount.Equals(other.HiddenNeuronCount) && OutputCount.Equals(other.OutputCount);
        #endregion

        #endregion

        #endregion
    }
}

