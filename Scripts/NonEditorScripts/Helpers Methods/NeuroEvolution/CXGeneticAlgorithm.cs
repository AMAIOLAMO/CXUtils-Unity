using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXUtils.NeuroEvolution
{
    /// <summary>
    /// Genetic algorithm for neuroEvolutions
    /// </summary>
    public struct CXGeneticAlgorithm
    {
        public CXNeuralNetwork[] population;
        private CXNeuralNetwork[] newPop;
        public int generationCount;

        public CXNeuralNetwork current_bestNNet;
        public float current_bestFitness;
        public int populationCount;
        public float mutationRate;

        public CXGeneticAlgorithm(int count, float mutateRate = .5f,
            int inputCount = 1, int hiddenLayerCount = 1,
            int hiddenNeuronCount = 5, int outputCount = 1)
        {
            populationCount = count;
            mutationRate = mutateRate;
            current_bestNNet = null;
            generationCount = 1;
            current_bestFitness = 0;

            population = new CXNeuralNetwork[count];
            newPop = new CXNeuralNetwork[count];

            for (int i = 0; i < count; i++)
                population[i] = new CXNeuralNetwork(inputCount, hiddenLayerCount, hiddenNeuronCount, outputCount);
        }

        public CXGeneticAlgorithm(int count, Func<int, CXNeuralNetwork> CreateNNet, float mutateRate = .5f)
        {
            populationCount = count;
            mutationRate = mutateRate;
            current_bestNNet = null;
            generationCount = 1;
            current_bestFitness = 0;

            population = new CXNeuralNetwork[count];
            newPop = new CXNeuralNetwork[count];

            for (int i = 0; i < count; i++)
                population[i] = CreateNNet(i);
        }

        public void InitializeGeneration()
        {
            for (int i = 0; i < populationCount; i++)
                population[i].Initialize();
        }

        public void RandomizeGeneration()
        {
            for (int i = 0; i < populationCount; i++)
                population[i].RandomInitialize();
        }

        public void NextGeneration()
        {
            CalculateFitness();

            for (int i = 0; i < population.Length; i++)
            {
                CXNeuralNetwork parent1 = ChooseParent();
                CXNeuralNetwork parent2 = ChooseParent();

                CXNeuralNetwork child = parent1.CrossOver(parent2);
                child.FullMutate(mutationRate);

                newPop[i] = child;
            }

            population = newPop;

            generationCount++;
        }

        private void CalculateFitness()
        {
            current_bestNNet = population[0];

            for (int i = 0; i < populationCount; i++)
            {
                if (population[i].fitness > current_bestNNet.fitness)
                    current_bestNNet = population[i];
            }

            current_bestFitness = current_bestNNet.fitness;
        }

        private CXNeuralNetwork ChooseParent()
        {
            return current_bestNNet;
        }

        public void Map(Func<CXNeuralNetwork, CXNeuralNetwork> mapFunc)
        {
            for (int i = 0; i < populationCount; i++)
                population[i] = mapFunc(population[i]);
        }
    }
}