using System;
using System.Collections.Generic;
using CXUtils.CodeUtils;
using CXUtils.Evolutions.NeuralNetworks;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

namespace CXUtils.Evolutions.GeneticAlgorithm
{

    public class CXNNetGeneAlgorithm
    {
        #region Fields

        public CXNeuralNetwork[] Population { get; private set; }
        public int Generation { get; private set; }

        public int CrossOverCount { get; private set; }

        public bool CrossAll { get; private set; }

        public float BestFitness { get; private set; }
        public CXNeuralNetwork? BestNeuralNetwork { get; private set; }
        public float MutationRate { get; set; }
        public float TotFitness { get; private set; }

        #endregion

        public CXNNetGeneAlgorithm(int populationSize,
            int inputCount, int hiddenLayerCount, int hiddenNeuronCount, int outputCount, float mutationRate = 0.055f, int? crossOverCount = null)
        {
            Population = new CXNeuralNetwork[populationSize];
            Generation = 1;
            BestFitness = 0;
            BestNeuralNetwork = null;
            MutationRate = mutationRate;
            TotFitness = 0;
            if (crossOverCount != null)
                CrossOverCount = crossOverCount.Value;

            CrossAll = crossOverCount == null;

            for (int i = 0; i < populationSize; i++)
                Population[i] = new CXNeuralNetwork(inputCount, hiddenLayerCount, hiddenNeuronCount, outputCount);

            InitGeneration();
            RandomizeGeneration();
        }

        #region Script Utils

        /// <summary> spawn a new generation </summary>
        public void NewGeneration()
        {
            if (Population.Length == 0)
                return;

            CalculateFitness();

            Population = CrossAndMutateGeneration();

            Generation++;

            InitGeneration();
        }

        private CXNeuralNetwork[] CrossAndMutateGeneration()
        {
            CXNeuralNetwork[] newPop = new CXNeuralNetwork[Population.Length];

            //the count to cross over
            if(CrossAll)
            {
                for (int i = 0; i < newPop.Length; i++)
                {
                    CXNeuralNetwork parent_1 = ChooseParent();
                    CXNeuralNetwork parent_2 = ChooseParent();

                    CXNeuralNetwork newChild = CrossOver(parent_1, parent_2);

                    newPop[i] = Mutate(newChild);
                }
            }
            else
            {
                for (int i = 0; i < CrossOverCount; i++)
                {
                    CXNeuralNetwork parent_1 = ChooseParent();
                    CXNeuralNetwork parent_2 = ChooseParent();

                    CXNeuralNetwork newChild = CrossOver(parent_1, parent_2);

                    newPop[i] = Mutate(newChild);
                }

                for (int i = CrossOverCount; i < newPop.Length; i++)
                    newPop[i].RandomizeWeights();
            }

            return newPop;
        }

        private void InitGeneration()
        {
            for (int i = 0; i < Population.Length; i++)
                Population[i].Initialize();
        }
        private void RandomizeGeneration()
        {
            for (int i = 0; i < Population.Length; i++)
                Population[i].RandomizeWeights();
        }

        /// <summary> Calculates this current population's overall fitness and their own fitness </summary>
        private void CalculateFitness()
        {
            TotFitness = 0;
            CXNeuralNetwork bestNNet = Population[0];

            for (int i = 0; i < Population.Length; i++)
            {
                TotFitness += Population[i].fitness;

                if (Population[i].fitness > bestNNet.fitness)
                    bestNNet = Population[i];
            }

            BestNeuralNetwork = bestNNet;
        }

        public CXNeuralNetwork CrossOver(CXNeuralNetwork a, CXNeuralNetwork b)
        {
            CXNeuralNetwork child = new CXNeuralNetwork(a.InputCount, a.HiddenLayerCount, a.HiddenNeuronCount, a.OutputCount);

            for (int i = 0; i < child.weights.Count; i++)
                for (int x = 0; x < child.weights[i].RowCount; x++)
                    for (int y = 0; y < child.weights[i].ColumnCount; y++)
                    {
                        child.weights[i][x, y] = MathUtils.FlipCoin() ? a.weights[i][x, y] : b.weights[i][x, y];
                        Debug.Log(child.weights[i][x, y]);
                    }

            return child;
        }

        public CXNeuralNetwork ChooseParent()
        {
            float randNum = MathUtils.RandomFloat() * TotFitness;

            for (int i = 0; i < Population.Length; i++)
            {
                if (randNum < Population[i].fitness)
                    return Population[i];
                else
                    randNum -= Population[i].fitness;
            }
            Debug.Log("Selected first pop as parent");
            return Population[0];
        }

        public CXNeuralNetwork Mutate(CXNeuralNetwork nnet)
        {
            for (int i = 0; i < nnet.weights.Count; i++)
                for (int x = 0; x < nnet.weights[i].RowCount; x++)
                    for (int y = 0; y < nnet.weights[i].ColumnCount; y++)
                        if (MathUtils.FlipCoin(MutationRate))
                            nnet.weights[i][x, y] = Random.Range(-1f, 1f);
            return nnet;
        }

        #endregion
    }

    /// <summary> A genetic algorithm class for calculating natural selection </summary>
    public class CXGeneticAlgorithm<T>
    {
        #region Fields

        public List<CXDNA<T>> Population { get; private set; }
        public int Generation { get; private set; }

        public float BestFitness { get; private set; }
        public T[] BestGenes { get; private set; }

        public float MutationRate { get; set; }
        public float TotFitness { get; private set; }

        #endregion

        #region Constructor

        public CXGeneticAlgorithm(int populationSize, int DNASize, Func<T> getRandomGeneFunc, Func<int, float> fitnessFunc,
            float mutationRate = .02f)
        {
            Generation = 1;
            MutationRate = mutationRate;
            Population = new List<CXDNA<T>>();

            BestGenes = new T[DNASize];

            for (int i = 0; i < populationSize; i++)
            {
                Population.Add(new CXDNA<T>(DNASize, getRandomGeneFunc, fitnessFunc));
            }
        }

        #endregion

        #region Script Utils

        public void NewGeneration()
        {
            if (Population.Count == 0)
                return;

            CalculateFitness();

            List<CXDNA<T>> newPop = new List<CXDNA<T>>();

            for (int i = 0; i < Population.Count; i++)
            {
                CXDNA<T> parent1 = ChooseParent();
                CXDNA<T> parent2 = ChooseParent();
                CXDNA<T> child = parent1.CrossOver(parent2);

                child.Mutate(MutationRate);

                newPop.Add(child);
            }

            Population = newPop;

            Generation++;
        }

        public void CalculateFitness()
        {
            TotFitness = 0;
            CXDNA<T> bestDNA = Population[0];

            for (int i = 0; i < Population.Count; i++)
            {
                TotFitness += Population[i].CalculateFitness(i);

                if (Population[i].Fitness > BestFitness)
                    bestDNA = Population[i];
            }

            BestFitness = bestDNA.Fitness;
            bestDNA.Genes.CopyTo(BestGenes, 0);
        }

        private CXDNA<T> ChooseParent()
        {
            float randNum = MathUtils.RandomFloat() * TotFitness;

            for (int i = 0; i < Population.Count; i++)
            {
                if (randNum < Population[i].Fitness)
                    return Population[i];
                else
                    randNum -= Population[i].Fitness;
            }

            return Population[0];
        }

        #endregion
    }
}