using System;

namespace CXUtils.CodeUtils
{
    /// <summary>
    ///     Math Function Class
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        ///     Is half TAU
        /// </summary>
        public const float PI = 3.14159265358979f;

        /// <summary>
        ///     Is double PI
        /// </summary>
        public const float TAU = 6.28318530717958f;

        public const float E = 2.71828182845905f;

        public static float Floor(float value) => value > 0 ? value : value - 1f;
        public static float Ceil(float value) => value < 0 ? value : value + 1f;


        /// <summary>
        ///     This will map the whole real Number line into the range of 0 - 1
        ///     <para>using calculation 1f / (Math.Pow(Math.E, -x)); </para>
        /// </summary>
        public static float Sigmoid(float x) => 1f / (float)Math.Pow(E, -x);

        public static bool IsApproximate(this float value, float x, float precision = float.Epsilon) => Math.Abs(value - x) < precision;

        /// <summary>
        ///     This will loop the <paramref name="value"/> back to 0, when <paramref name="value"/> is an integer
        /// </summary>
        public static float Frac(this float value) => value - Floor(value);
        /// <summary>
        ///     Loops the <paramref name="value"/> back to 0 when <paramref name="value"/> is a multiple of <paramref name="amount"/>
        /// </summary>
        public static float Loop(this float value, float amount) => value - Floor(value / amount) * amount;

        /// <inheritdoc cref="Loop(float, float)"/>
        public static int Loop(this int value, int amount) => value % amount;

        #region Range

        /// <summary>
        ///     Maps the given value from the given range to the another given range <br />
        ///     NOTE: If values overflow, it will cause unexpected behaviour
        /// </summary>
        public static float Map(float val, float inMin, float inMax, float outMin, float outMax) => (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

        public static float Clamp(float value, float min, float max) =>
            value < min ? min : value > max ? max : value;

        public static float Clamp01(float value) => Clamp(value, 0f, 1f);

        #endregion

        #region Other

        public static float RoundOnStep(float value, float step) => (float)Math.Round(value / step) * step;

        #endregion
    }
}
