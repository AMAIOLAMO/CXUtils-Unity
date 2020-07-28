using UnityEngine;
using System.Collections.Generic;

using SysRandom = System.Random;

namespace CXUtils.CodeUtils
{
    /// <summary> A helper class that helps you randomize things </summary>
    public class RandomUtils
    {
        /// <summary> Randomly returns a float between 0 ~ 1 </summary>
        public static float RandomFloat() => Random.Range(0f, 1f);

        /// <summary> Randomly returns a double between 0 ~ 1 </summary>
        public static double RandomDouble() => RandomFloat();

        /// <summary> Randomly decides and returns a boolean </summary>
        public static bool FlipCoin(float threshold = .5f) => Random.Range(0f, 1f) > threshold;

        /// <summary> Randomly decides between two items </summary>
        public static T FlipCoin<T>(T t1, T t2, float threshold = .5f) => FlipCoin(threshold) ? t1 : t2;

        /// <summary> Randomly decides between items
        /// <para>QUICK NOTE: Possibilities of items are the same</para> </summary>
        public T FlipCoin<T>(params T[] items) => items[Random.Range(0, items.Length - 1)]; //in here, the random that comes from unity will return the upper bound, so -1
    }

    /// <summary> A Random class that helps to randomize object </summary>
    public class CXRandom : SysRandom
    {
        public CXRandom() : base() { }
        public CXRandom(int seed) : base(seed) { }

        /// <summary> Randomly decides and returns a boolean
        /// <para>QUICK NOTE: excludes theashold (value > threashold)</para></summary>
        public bool FlipCoin(double threashold = .5f) => NextDouble() > threashold;

        /// <summary> Randomly decides between two items
        /// <para>QUICK NOTE: excludes theashold (value > threashold)</para></summary>
        public T FlipCoin<T>(T t1, T t2, double threshold = .5d) => FlipCoin(threshold) ? t1 : t2;

        /// <summary> Randomly decides between these items 
        /// <para>QUICK NOTE: possibilities are the same </para></summary>
        public T FlipCoin<T>(params T[] items) => items[Next(0, items.Length)]; //in here, the random value will never return the upper bound, so don't worry

        /// <summary>
        /// Randomly decides between items with the given sorted pair
        /// <para> QUICK NOTE: <paramref name="probabilityItemPair"/> needs to be sorted from lowest to highest </para>
        /// </summary>
        public bool TryFlipCoin<T>(out T item, params KeyValuePair<int, T>[] probabilityItemPair)
        {
            int i, tot = 0;

            //get total
            for (i = 0; i < probabilityItemPair.Length; i++)
                tot += probabilityItemPair[i].Key;

            int lastMin = 0;
            int rand = Next(0, tot) + 1; // this will go from 0 to tot (since next doesn't include upper bound, so we increment it)

            //get probability
            for (i = 0; i < probabilityItemPair.Length; i++)
            {
                //if in range
                if (MathUtils.ValueInRange(rand, lastMin, tot, RangeOptions.IncMax))
                {
                    item = probabilityItemPair[i].Value;
                    return true;
                }
            }

            item = default;
            return false;
        }
    }
}
