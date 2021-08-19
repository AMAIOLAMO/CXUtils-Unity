using System;
using System.Collections.Generic;

namespace CXUtils.Common
{
    /// <summary>
    ///     An extension for the system random
    /// </summary>
    public static class CxRandomExtension
    {
        /// <summary>
        ///     Gives a random number between <see cref="float.MinValue"/> ~ <see cref="float.MaxValue">
        /// </summary>
        public static float NextFloat(this Random random)
        {
            double mantissa = random.NextDouble() * 2d - 1d;

            double exponent = Math.Pow(2d, random.Next(-126, 128));
            return (float)(mantissa * exponent);
        }

        /// <summary>
        ///     Gives a random number between <paramref name="min"/> ~ <paramref name="max"/>
        /// </summary>
        public static float NextFloat(this Random random, float min, float max) => (float)random.NextDouble() * (max - min) + min;

        /// <summary>
        ///     Randomly decides and returns a boolean <br />
        ///     QUICK NOTE: excludes threshold (value > threshold)
        /// </summary>
        public static bool FlipCoin(this Random random, double threshold = .5f) => random.NextDouble() > threshold;

        /// <summary>
        ///     Randomly decides between two items <br />
        ///     QUICK NOTE: excludes threshold (value > threshold)
        /// </summary>
        public static T FlipCoin<T>(this Random random, T t1, T t2, double threshold = .5d) => random.FlipCoin(threshold) ? t1 : t2;

        /// <summary>
        ///     Randomly decides between these items <br />
        ///     QUICK NOTE: possibilities are the same
        /// </summary>
        public static T Choose<T>(this Random random, params T[] items) => items[random.Next(0, items.Length)];

        /// <summary>
        ///     Randomly decides between items with the given sorted pair <br />
        ///     QUICK NOTE: <paramref name="probabilityItemPair" /> needs to be sorted from lowest to highest
        /// </summary>
        public static bool TryChoose<T>(this Random random, out T item, params KeyValuePair<int, T>[] probabilityItemPair)
        {
            int i, total = 0;

            //get total
            for ( i = 0; i < probabilityItemPair.Length; ++i )
                total += probabilityItemPair[i].Key;

            const int lastMin = 0;
            int rand = random.Next(0, total + 1); // this will go from 0 to tot (since next doesn't include upper bound, so we increment it)

            //get probability
            for ( i = 0; i < probabilityItemPair.Length; ++i )
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
