using System;
using System.Collections.Generic;

namespace CXUtils.Common
{
    /// <summary>
    ///     A <see cref="Random" /> Wrapper
    /// </summary>
    public class CxRandom : Random
    {
        public CxRandom() { }
        public CxRandom(int seed) : base(seed) { }

        /// <summary>
        ///     Randomly decides and returns a boolean <br />
        ///     QUICK NOTE: excludes threshold (value > threshold)
        /// </summary>
        public bool FlipCoin(double threshold = .5f) => NextDouble() > threshold;

        /// <summary>
        ///     Randomly decides between two items <br />
        ///     QUICK NOTE: excludes threshold (value > threshold)
        /// </summary>
        public T FlipCoin<T>(T t1, T t2, double threshold = .5d) => FlipCoin(threshold) ? t1 : t2;

        /// <summary>
        ///     Randomly decides between these items <br />
        ///     QUICK NOTE: possibilities are the same
        /// </summary>
        public T Choose<T>(params T[] items) => items[Next(0, items.Length)];

        /// <summary>
        ///     Randomly decides between items with the given sorted pair <br />
        ///     QUICK NOTE: <paramref name="probabilityItemPair" /> needs to be sorted from lowest to highest
        /// </summary>
        public bool TryChoose<T>(out T item, params KeyValuePair<int, T>[] probabilityItemPair)
        {
            int i, total = 0;

            //get total
            for ( i = 0; i < probabilityItemPair.Length; i++ )
                total += probabilityItemPair[i].Key;

            const int lastMin = 0;
            int rand = Next(0, total + 1); // this will go from 0 to tot (since next doesn't include upper bound, so we increment it)

            //get probability
            for ( i = 0; i < probabilityItemPair.Length; i++ )
                //if in range
                if ( rand > lastMin && rand <= total )
                {
                    item = probabilityItemPair[i].Value;
                    return true;
                }

            item = default;
            return false;
        }
    }
}
