using UnityEngine;

namespace CXUtils.CodeUtils
{
    ///<summary> A simple noise helper class </summary>
    public class CXNoiseUtils
    {
        #region Perlin noise
        
        /// <summary> Procedural noise generation, Perlin noise (scale cannot be 0)
        /// <para>Note: seed will be default to 0</para></summary>
        public static float PerlinNoise(float x, float y, float scale, float? seed = null)
        {
            float currentSeed = seed ?? 0;
            return Mathf.PerlinNoise(x / scale + currentSeed, y / scale + currentSeed);
        }

        /// <summary> Procedural noise generation, Perlin noise (scale cannot be 0)
        /// <para>Note: seed will be default to 0</para></summary>
        public static float PerlinNoise(Vector2 position, float scale, float? seed = null) =>
            PerlinNoise(position.x, position.y, scale, seed);

        /// <summary> Generates a boolean value that the threshHold gives.
        /// <para>(clamps threshHold value between 0 ~ 1)</para>
        /// <para>Note: seed will be default to 0</para></summary>
        public static bool PerlinNoise_FlipCoin(float x, float y, float scale, float threshHold = .5f,
         float? seed = null)
        {
            threshHold = Mathf.Clamp01(threshHold);
            return PerlinNoise(x, y, scale, seed) > threshHold;
        }

        /// <summary> Generates a boolean value that the threshHold gives.
        /// <para>(clamps threshHold value between 0 ~ 1)</para>
        /// <para>Note: seed will be default to 0</para></summary>
        public static bool PerlinNoise_FlipCoin(Vector2 position, float scale, float threshHold = .5f,
         float? seed = null) =>
            PerlinNoise_FlipCoin(position.x, position.y, scale, threshHold, seed);

        #endregion
    }

}
