using System.Collections.Generic;
using UnityEngine;
using SysRandom = System.Random;

namespace CXUtils.CodeUtils
{
    /// <summary> A helper class that helps you randomize things </summary>
    public class RandomUtils
    {
        /// <summary> Randomly returns a float between 0 ~ 1 </summary>
        public static float RandFloat()
        {
            return Random.Range(0f, 1f);
        }

        /// <summary> Randomly returns a double between 0 ~ 1 </summary>
        public static double RandDouble()
        {
            return RandFloat();
        }

        /// <summary> Randomly decides and returns a boolean </summary>
        public static bool FlipCoin(float threshold = .5f)
        {
            return Random.Range(0f, 1f) >= threshold;
        }

        /// <summary> Randomly decides between two items </summary>
        public static T FlipCoin<T>(T t1, T t2, float threshold = .5f)
        {
            return FlipCoin(threshold) ? t1 : t2;
        }

        /// <summary>
        ///     Randomly decides between items <br />
        ///     QUICK NOTE: Possibilities of items are the same
        /// </summary>
        public T FlipCoin<T>(params T[] items)
        {
            return items[Random.Range(0, items.Length - 1)]; //in here, the random that comes from unity will return the upper bound, so -1
        }
    }

    /// <summary> A Random class that helps to randomize object </summary>
    public class CxRandom : SysRandom
    {
        public CxRandom() { }
        public CxRandom(int seed) : base(seed) { }

        /// <summary>
        ///     Randomly decides and returns a boolean <br/>
        ///     QUICK NOTE: excludes threshold (value > threshold)
        /// </summary>
        public bool FlipCoin(double threshold = .5f)
        {
            return NextDouble() > threshold;
        }

        /// <summary>
        ///     Randomly decides between two items <br/>
        ///     QUICK NOTE: excludes threshold (value > threshold)
        /// </summary>
        public T FlipCoin<T>(T t1, T t2, double threshold = .5d)
        {
            return FlipCoin(threshold) ? t1 : t2;
        }

        /// <summary>
        ///     Randomly decides between these items <br/>
        ///     QUICK NOTE: possibilities are the same
        /// </summary>
        public T FlipCoin<T>(params T[] items)
        {
            return items[Next(0, items.Length)]; //in here, the random value will never return the upper bound, so don't worry
        }

        /// <summary>
        ///     Randomly decides between items with the given sorted pair <br/>
        ///     QUICK NOTE: <paramref name="probabilityItemPair" /> needs to be sorted from lowest to highest
        /// </summary>
        public bool TryFlipCoin<T>(out T item, params KeyValuePair<int, T>[] probabilityItemPair)
        {
            int i, total = 0;

            //get total
            for (i = 0; i < probabilityItemPair.Length; i++)
                total += probabilityItemPair[i].Key;

            var lastMin = 0;
            var rand = Next(0, total) + 1; // this will go from 0 to tot (since next doesn't include upper bound, so we increment it)

            //get probability
            for (i = 0; i < probabilityItemPair.Length; i++)
                //if in range
                if (rand > lastMin && rand <= total)
                {
                    item = probabilityItemPair[i].Value;
                    return true;
                }

            item = default;
            return false;
        }
    }
}
