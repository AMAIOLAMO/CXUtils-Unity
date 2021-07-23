using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary> Options flags for checking range </summary>
    public enum RangeOptions
    {
        ///<summary> Include Max, exclude Min </summary>
        IncMax,
        ///<summary> Include Min, exclude Max </summary>
        IncMin,
        ///<summary> Include both Min and Max </summary>
        IncBoth,
        ///<summary> Exclude Both Min and Max </summary>
        ExcBoth
    }

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

        /// <summary>
        ///     This will map the whole real Number line into the range of 0 - 1
        ///     <para>using calculation 1f / (Math.Pow(Math.E, -x)); </para>
        /// </summary>
        public static float Sigmoid( float x ) => 1f / Mathf.Pow( E, -x );

        public static bool IsApproximate( this float value, float x, float precision = float.Epsilon ) => Math.Abs( value - x ) < precision;

        #region Range

        /// <summary>
        ///     Maps the given value from the given range to the another given range <br />
        ///     NOTE: there is a probability that you can overflow and cause unexpected behaviour
        /// </summary>
        public static float Map( float val, float inMin, float inMax, float outMin, float outMax ) =>
            ( val - inMin ) * ( outMax - outMin ) / ( inMax - inMin ) + outMin;

        public static float Clamp( float value, float min, float max ) =>
            value < min ? min : value > max ? max : value;

        public static float Clamp01( float value ) => Clamp( value, 0, 1 );

        #endregion

        #region Lines

        ///<summary> Returns if the two lines will collide with each other </summary>
        public static bool LineIntersection2D( float x1, float x2, float x3, float x4,
            float y1, float y2, float y3, float y4, out float t, out float u )
        {
            float x1Mx2 = x1 - x2,
                x1Mx3 = x1 - x3,
                x3Mx4 = x3 - x4,
                y1My2 = y1 - y2,
                y1My3 = y1 - y3,
                y3My4 = y3 - y4;

            //write the line intersection
            float tUp = x1Mx3 * y3My4 - y1My3 * x3Mx4;
            float uUp = -( x1Mx2 * y1My3 - y1My2 * x1Mx3 );
            float den = x1Mx2 * y3My4 - y1My2 * x3Mx4;

            //calculate
            ( t, u ) = ( tUp / den, uUp / den );

            //make boolean and check
            bool tBool, uBool;
            ( tBool, uBool ) = ( t >= 0f && t <= 1f, u >= 0f && u <= 1f );

            return tBool && uBool;
        }

        ///<summary> Checks if the two lines in 2D will collide with each other </summary>
        public static bool LineIntersection2D( float x1, float x2, float x3, float x4,
            float y1, float y2, float y3, float y4 ) => LineIntersection2D( x1, x2, x3, x4, y1, y2, y3, y4, out _, out _ );

        #endregion

        #region Other

        /// <summary>
        ///     I've written this because C#'s % isn't really modulo arithmetic <br />
        ///     this wraps the number back to the target one
        /// </summary>
        public static float Modulo( float a, float b ) =>
            //my own wrapping algorithm for C#
            ( b + a % b ) % b;

        /// <inheritdoc cref="Modulo(float, float)" />
        public static int Modulo( int a, int b ) => ( b + a % b ) % b;

        public static int RoundOnStep( int value, int step ) => (int)Mathf.Round( (float)value / step ) * step;

        public static float RoundOnStep( float value, float step ) => Mathf.Round( value / step ) * step;

        #endregion
    }
}
