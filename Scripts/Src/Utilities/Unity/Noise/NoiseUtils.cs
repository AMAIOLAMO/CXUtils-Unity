using UnityEngine;

namespace CXUtils.Common
{
    ///<summary> A simple noise helper class </summary>
    public struct NoiseUtils
    {
        #region Perlin noise

        /// <summary>
        ///     Procedural noise generation, Perlin noise (scale cannot be 0)
        ///     <para>QUICK NOTE:seed will be default to 0</para>
        /// </summary>
        public static float PerlinNoise( int x, int y, float scale, float? offset = null )
        {
            float currentSeed = offset ?? 0;
            return Mathf.PerlinNoise( ( x + currentSeed ) * scale, ( y + currentSeed ) * scale );
        }

        /// <summary>
        ///     Procedural noise generation, Perlin noise (scale cannot be 0)
        ///     <para>QUICK NOTE:seed will be default to 0</para>
        /// </summary>
        public static float PerlinNoise( Vector2Int position, float scale, float? offset = null )
        {
            return PerlinNoise( position.x, position.y, scale, offset );
        }

        /// <summary>
        ///     Generates a boolean value that the threshHold gives.
        ///     QUICK NOTEs: <br />
        ///     clamps threshHold value between 0 ~ 1 <br />
        ///     seed will be default to 0
        /// </summary>
        public static bool PerlinNoise_FlipCoin( int x, int y, float scale, float threshHold = .5f, float? offset = null )
        {
            return PerlinNoise( x, y, scale, offset ) > Mathf.Clamp01( threshHold );
        }

        /// <summary>
        ///     Generates a boolean value that the threshHold gives.
        ///     QUICK NOTEs: <br />
        ///     clamps threshHold value between 0 ~ 1 <br />
        ///     seed will be default to 0
        /// </summary>
        public static bool PerlinNoise_FlipCoin( Vector2Int position, float scale, float threshHold = .5f, float? offset = null )
        {
            return PerlinNoise_FlipCoin( position.x, position.y, scale, threshHold, offset );
        }

        #endregion
    }

}
