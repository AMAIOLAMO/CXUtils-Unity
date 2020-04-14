using System;
using CXUtils.CXMath;

namespace EasyAsset.CXNeuralNetwork
{

    ///<summary> CX's Perceptron Class </summary>
    public class Perceptron
    {
        #region Variables
        public float[] weights;
        public float biasWeight;
        public float LearningOvershootDamp = 0.01f;
        public Func<float, float> currentActivationFunction = (x) =>
            1f / (1f + (float)Math.Pow(Math.E, -x));
        private readonly Random random = new Random();
        #endregion

        ///<summary> Creating a new perceptron with all initialized in 0 </summary>
        public Perceptron(int weightLength)
        {
            weights = new float[weightLength];
            biasWeight = 0;

            for (int i = 0; i < weightLength; i++)
                weights[i] = 0;
        }

        ///<summary> Creating a new perceptron with random values </summary>
        public Perceptron(int weightLength, float biasWeight, float LearningOvershootDamp, int randomRange)
        {
            weights = new float[weightLength];
            this.biasWeight = biasWeight;
            this.LearningOvershootDamp = LearningOvershootDamp;

            randomRange = Math.Abs(randomRange);

            for (int i = 0; i < weightLength; i++)
                weights[i] = random.Next(-randomRange, randomRange + 1);
        }

        ///<summary> Creating a new perceptron with given weight </summary>
        public Perceptron(float[] weights, float biasWeight, float LearningOvershootDamp)
        {
            this.weights = weights;
            this.biasWeight = biasWeight;
            this.LearningOvershootDamp = LearningOvershootDamp;
        }

        ///<summary> Creating a new perceptron with initialized one weight </summary>
        public Perceptron(int weightLength, float init_Weight, float biasWeight, float LearningOvershootDamp)
        {
            weights = new float[weightLength];
            this.biasWeight = biasWeight;
            this.LearningOvershootDamp = LearningOvershootDamp;

            for (int i = 0; i < weightLength; i++)
                weights[i] = init_Weight;
        }


        //method and functions
        //setting

        ///<summary> Sets the activation functions using other functions </summary>
        public void SetActivationFunction(Func<float, float> activationFunction) =>
            currentActivationFunction = activationFunction;

        ///<summary> Uses the function to all the weight </summary>
        public void MapAllWeightUsingFunction(Func<float, float> func)
        {
            for (int i = 0; i < weights.Length; i++)
                weights[i] = func(weights[i]);
        }

        //activational methods----------------------------------

        ///<summary> Returns the activation value (overridable) </summary>
        public virtual float ActivationFunction(float x) =>
            currentActivationFunction(x);


        //guessing----------------------------------

        ///<summary> This will let the perceptron guess the answer without activation </summary>
        public float Guess_NoActivation(float[] inputs)
        {
            float totWeight = 0f;
            for (int i = 0; i < weights.Length; i++)
                totWeight += weights[i] * inputs[i];

            return totWeight;
        }

        ///<summary> This will let the perceptron guess the answer with activation </summary>
        public float Guess_WithActivation(float[] inputs) =>
            ActivationFunction(Guess_NoActivation(inputs) + biasWeight);



        //training----------------------------------
        //training error for un overshooting

        ///<summary> Returns the training error with no activation </summary>
        public float GetTrainingError_NoActivation(float[] inputs, float target) =>
            (target - Guess_NoActivation(inputs));

        ///<summary> Returns the training error with activation </summary>
        public float GetTrainingError_WithActivation(float[] inputs, float target) =>
            (target - Guess_WithActivation(inputs));

        ///<summary> Train the perceptron using inputs and the target </summary>
        public void Train(float[] inputs, float target)
        {
            float error = GetTrainingError_WithActivation(inputs, target);

            for (int i = 0; i < weights.Length; i++)
                weights[i] += error * inputs[i] * LearningOvershootDamp;
        }
    }

    ///<summary> CX's neural network (beta) </summary>
    public class NeuralNetwork
    {
        #region Variables
        public Matrix weights_InputToHiddenMatrix;
        public Matrix weights_HiddenToOutputMatrix;
        public Matrix biasWeights;

        public int InputCount { get; private set; }
        public int HiddenNodeCount { get; private set; }
        public int OutPutCount { get; private set; }
        #endregion

        public NeuralNetwork(int inputCount = 2, int hiddenNodeCount = 1, int outputCount = 2)
        {
            (InputCount, HiddenNodeCount, OutPutCount) = (inputCount, hiddenNodeCount,outputCount);

            weights_InputToHiddenMatrix = new Matrix(hiddenNodeCount, inputCount);
            weights_HiddenToOutputMatrix = new Matrix(outputCount, hiddenNodeCount);
        }

        #region MainMethods
        public Matrix FeedForward(Matrix Inputs)
        {
            Matrix result = weights_InputToHiddenMatrix * Inputs;
            result.Map(MathFunc.Sigmoid_1);

            result = weights_HiddenToOutputMatrix * result;
            result.Map(MathFunc.Sigmoid_1);

            return result;
        }

        public void Train(Matrix Inputs, Matrix Targets)
        {
            throw new NotImplementedException();
            //Matrix guessedAns = FeedForward(Inputs);
            //Matrix gottedError = Targets - guessedAns;
            //still not finish
            //this thing sucks lol
        }
        #endregion

        #region HelperMethods
        public void RandomizeWeights()
        {
            weights_InputToHiddenMatrix.Randomize();
            weights_HiddenToOutputMatrix.Randomize();
        }
        #endregion
    }
}