using System;
using System.IO;
using SimpleJSON;
using CXUtils.CodeUtils;
using Random = UnityEngine.Random;
using MathNet.Numerics.LinearAlgebra;

namespace CXUtils.Evolutions.NeuralNetworks
{
    /// <summary> A neural network from Cx Utils (using Math Dot Net) </summary>
    public class CXNeuralNetwork
    {
        #region Fields

        #region Counts

        public int InputCount { get; private set; }

        public int HiddenLayerCount { get; private set; }

        public int HiddenNeuronCount { get; private set; }

        public int OutputCount { get; private set; }

        #endregion

        #region NNet Main

        private Matrix<float> inputLayer;

        private Matrix<float> outputLayer;

        private Matrix<float>[] hiddenLayers;

        public Matrix<float>[] weights;

        public float[] biases;

        #endregion

        #endregion

        public CXNeuralNetwork(int inputCount, int hiddenLayerCount, int hiddenNeuronCount, int outputCount)
        {
            InputCount = inputCount;
            HiddenLayerCount = hiddenLayerCount;
            HiddenNeuronCount = hiddenNeuronCount;
            OutputCount = outputCount;

            ResetFields();

            Initialize();
        }

        #region Setting NNet

        //Resets all the fields
        private void ResetFields()
        {
            inputLayer = Matrix<float>.Build.Dense(1, InputCount);

            hiddenLayers = new Matrix<float>[HiddenLayerCount];

            outputLayer = Matrix<float>.Build.Dense(1, OutputCount);

            //weights = new List<Matrix<float>>();
            weights = new Matrix<float>[HiddenLayerCount + 1]; // hiddenlayer + 2(input and output) - 1 (cuz I want)

            biases = new float[HiddenLayerCount + 2];
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

            for (int i = 1; i < hiddenLayers.Length; i++)
                hiddenLayers[i] = ActivationFunction((hiddenLayers[i - 1] * weights[i]) + biases[i]);

            outputLayer = ActivationFunction((hiddenLayers[hiddenLayers.Length - 1] * weights[weights.Length - 1]) + biases[biases.Length - 1]);

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

        /// <summary> Initializes the Neural network with no initial weights and biases as 0 </summary>
        public void Initialize()
        {
            //start
            biases[0] = 0;
            Matrix<float> inputToH1 = Matrix<float>.Build.Dense(InputCount, HiddenNeuronCount);
            weights[0] = inputToH1;

            for (int i = 1; i < HiddenLayerCount + 1; i++)
            {
                Matrix<float> f = Matrix<float>.Build.Dense(1, HiddenNeuronCount);

                hiddenLayers[i - 1] = f;

                biases[i] = 0;

                Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(HiddenNeuronCount, HiddenNeuronCount);
                weights[i] = HiddenToHidden;
            }

            Matrix<float> outputWeight = Matrix<float>.Build.Dense(HiddenNeuronCount, OutputCount);

            weights[HiddenLayerCount] = outputWeight;
            //adds the last one to 0
            biases[HiddenLayerCount + 1] = 0;
        }

        /// <summary> Initializes the Neural network and randomizes the biases and weights </summary>
        public void RandomInitialize()
        {
            Initialize();

            RandomizeAll();
        }

        public void ClearAllData()
        {
            inputLayer.Clear();
            hiddenLayers = new Matrix<float>[HiddenLayerCount];
            outputLayer.Clear();
            weights = new Matrix<float>[HiddenLayerCount + 1];
            biases = new float[HiddenLayerCount + 2];
        }

        #region Randomization

        public void RandomizeAll()
        {
            RandomizeWeights();
            RandomizeBiases();
        }

        public void RandomizeWeights() =>
            MapWeights(() => Random.Range(-1f, 1f));

        public void RandomizeBiases()
        {
            for (int i = 0; i < HiddenLayerCount + 2; i++)
                biases[i] = Random.Range(-1f, 1f);
        }

        #endregion

        #region Map manipulation and iteration

        /// <summary> Iterates through all the weights and map them </summary>
        public void MapWeights(Func<int, int, int, float> IteratingFunc)
        {
            for (int i = 0; i < weights.Length; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        weights[i][x, y] = IteratingFunc(i, x, y);
        }

        /// <summary> Iterates through all the weights and map them </summary>
        public void MapWeights(Func<float> IteratingFunc)
        {
            for (int i = 0; i < weights.Length; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        weights[i][x, y] = IteratingFunc();
        }

        /// <summary> Iterates through all the weights </summary>
        public void IterateWeights(Action<float, int, int, int> IterateAction)
        {
            for (int i = 0; i < weights.Length; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        IterateAction(weights[i][x, y], i, x, y);
        }

        /// <summary> Iterates through all the weights </summary>
        public void IterateWeights(Action<float> IterateAction)
        {
            for (int i = 0; i < weights.Length; i++)
                for (int x = 0; x < weights[i].RowCount; x++)
                    for (int y = 0; y < weights[i].ColumnCount; y++)
                        IterateAction(weights[i][x, y]);
        }

        #endregion

        #region Json

        /// <summary> Converts this Neural network's weights into json </summary>
        public string WeightsToJson()
        {
            JSONArray newJsonWeights = new JSONArray();
            UnityEngine.Debug.Log(weights.Length);

            //calculates and adds each of the weights one to one
            for (int i = 0; i < weights.Length; i++)
            {
                JSONArray newJsonWeight = new JSONArray();

                for (int y = 0; y < weights[i].ColumnCount; y++)
                {
                    JSONArray newJsonWeight_y = new JSONArray();

                    for (int x = 0; x < weights[i].RowCount; x++)
                        newJsonWeight_y.Add(weights[i][x, y]);

                    newJsonWeight.Add(newJsonWeight_y);
                }

                newJsonWeights.Add(newJsonWeight);
            }

            return newJsonWeights.ToString();
        }

        /// <summary> Parses the weights json file and gets the weight matrix from it </summary>
        public static Matrix<float>[] WeightsFromJson(string json)
        {
            Matrix<float>[] newWeights;

            //if parse correctly
            JSONArray newJsonArrayWeights = JSON.Parse(json).AsArray;

            newWeights = new Matrix<float>[newJsonArrayWeights.Count];

            UnityEngine.Debug.Log(newWeights.Length);
            UnityEngine.Debug.Log(newJsonArrayWeights.Count);

            for (int i = 0; i < newWeights.Length; i++)
            {
                newWeights[i] = Matrix<float>.Build.Dense(newJsonArrayWeights[i][0].Count, newJsonArrayWeights[i].Count);

                for (int y = 0; y < newJsonArrayWeights[i].Count; y++)
                    for (int x = 0; x < newJsonArrayWeights[i][y].Count; x++)
                        newWeights[i][x, y] = newJsonArrayWeights[i][y][x];
            }

            return newWeights;
        }

        /// <summary> Parses the weights json file and gets the neural network from it </summary>
        public static CXNeuralNetwork NetFromWeightsJson(string json)
        {
            Matrix<float>[] weights = WeightsFromJson(json);

            CXNeuralNetwork newNet = new CXNeuralNetwork(
                weights[0].RowCount,
                weights.Length - 1,
                weights[1].RowCount,
                weights[weights.Length - 1].ColumnCount
                );

            newNet.weights = weights;

            return newNet;
        }

        /// <summary> Saves the weights to a path </summary>
        public void SaveWeightsJsonToPath(string path)
        {
            string json = WeightsToJson();
            File.WriteAllText(path, json);
        }
        #endregion

        #region Other helper utils

        /// <summary> Randomize mutates the Neural Network </summary>
        /// <param name="mutateRate"> 0 ~ 1 mutating rate </param>
        public void Mutate(float mutateRate = .2f) =>
            MapWeights((i, x, y) => MathUtils.FlipCoin(Random.Range(-1f, 1f), weights[i][x, y], mutateRate));

        /// <summary> Crosses any two matching Neural networks </summary>
        public bool TryCrossOver(CXNeuralNetwork other, out CXNeuralNetwork OUT, float rate = .5f)
        {
            if (AllowedCrossOver(other))
            {
                OUT = new CXNeuralNetwork(InputCount, HiddenLayerCount, HiddenNeuronCount, OutputCount);

                OUT.MapWeights((i, x, y) =>
                    MathUtils.FlipCoin(rate) ? weights[i][x, y] : other.weights[i][x, y]);

                return true;
            }

            OUT = null;
            return false;
        }

        /// <summary> Crosses any two matching Neural networks </summary>
        public CXNeuralNetwork CrossOver(CXNeuralNetwork other, float rate = .5f)
        {
            TryCrossOver(other, out CXNeuralNetwork child, rate);
            return child;
        }

        /// <summary> Check's if this Neural network with other neural network matches and could cross over </summary>
        public bool AllowedCrossOver(CXNeuralNetwork other) =>
            InputCount.Equals(other.InputCount) && HiddenLayerCount.Equals(other.HiddenLayerCount) &&
            HiddenNeuronCount.Equals(other.HiddenNeuronCount) && OutputCount.Equals(other.OutputCount);

        #endregion

        #endregion
    }
}

