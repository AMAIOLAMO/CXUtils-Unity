using UnityEngine;
using CXUtils.CodeUtils;
using System;

namespace CXUtils.Evolutions.GeneticAlgorithm
{
    /// <summary> A DNA for the genetic algorithm </summary>
    public class CXDNA<T>
    {
        #region Fields

        public T[] Genes { get; set; }
        public float Fitness { get; set; }

        public Func<T> GetRandomGene { get; private set; }
        public Func<int, float> FitnessFunction { get; private set; }

        #endregion

        public CXDNA(int size, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool initializeGenes = true)
        {
            Genes = new T[size];
            Fitness = 0;

            FitnessFunction = fitnessFunction;
            GetRandomGene = getRandomGene;

            if (initializeGenes)
            {
                for (int i = 0; i < Genes.Length; i++)
                    Genes[i] = GetRandomGene();
            }
        }

        #region Script Utils

        public float CalculateFitness(int index)
        {
            Fitness = FitnessFunction(index);
            return Fitness;
        }

        public CXDNA<T> CrossOver(CXDNA<T> other)
        {
            CXDNA<T> child = new CXDNA<T>(Genes.Length, GetRandomGene, FitnessFunction, initializeGenes: false);

            for (int i = 0; i < Genes.Length; i++)
                child.Genes[i] = MathUtils.FlipCoin() ? Genes[i] : other.Genes[i];

            return child;
        }

        public void Mutate(float mutationRate)
        {
            mutationRate = Mathf.Clamp01(mutationRate);

            for (int i = 0; i < Genes.Length; i++)
            {
                if (MathUtils.RandomFloat() > mutationRate)
                {
                    Genes[i] = GetRandomGene();
                }
            }
        }

        #endregion
    }
}