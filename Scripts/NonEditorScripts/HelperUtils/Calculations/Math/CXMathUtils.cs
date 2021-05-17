using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary> Options flags for checking range </summary>
    public enum RangeOptions : byte
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

    ///<summary> Cx's Math Function Class </summary>
    public struct MathUtils
    {
        /// <summary> PI's Brother TAU: Represents PI * 2 </summary>
        public const float TAU = 6.28318530717958f;

        public const float E = 2.71828182845905f;

        #region Range Manipulation

        ///<summary> Returns if the float is in the given range </summary>
        public static bool InRange( float x, float min, float max, RangeOptions checkRangeMode = RangeOptions.IncBoth )
        {
            switch ( checkRangeMode )
            {
                case RangeOptions.IncMax:
                return ( x > min && x <= max );
                case RangeOptions.IncMin:
                return ( x >= min && x < max );
                case RangeOptions.IncBoth:
                return ( x >= min && x <= max );
                default:
                return ( x > min && x < max );
            }
        }

        ///<summary>
        ///Returns if the double is in the given range
        ///</summary>
        public static bool InRange( double x, double min, double max, RangeOptions checkRangeMode = RangeOptions.IncBoth )
        {
            switch ( checkRangeMode )
            {
                case RangeOptions.IncMax:
                return ( x > min && x <= max );
                case RangeOptions.IncMin:
                return ( x >= min && x < max );
                case RangeOptions.IncBoth:
                return ( x >= min && x <= max );
                default:
                return ( x > min && x < max );
            }
        }

        ///<summary> Maps the given value from the given range to the another given range (no Safety Checks) </summary>
        public static float Map( float val, float inMin, float inMax, float outMin, float outMax ) =>
            ( val - inMin ) * ( outMax - outMin ) / ( inMax - inMin ) + outMin;

        #endregion

        #region Lines

        ///<summary> Returns if the two lines will collide with each other </summary>
        public static bool LineIntersection2D( float x1, float x2, float x3, float x4,
        float y1, float y2, float y3, float y4, out float t, out float u )
        {
            //write the line intersection
            float tUp = ( x1 - x3 ) * ( y3 - y4 ) - ( y1 - y3 ) * ( x3 - x4 );
            float uUp = -( ( x1 - x2 ) * ( y1 - y3 ) - ( y1 - y2 ) * ( x1 - x3 ) );
            float den = ( x1 - x2 ) * ( y3 - y4 ) - ( y1 - y2 ) * ( x3 - x4 );

            //calculate
            (t, u) = (tUp / den, uUp / den);

            //make boolean and check
            bool tBool, uBool;
            (tBool, uBool) = (InRange( t, 0f, 1f ), InRange( u, 0f, 1f ));

            //making the bool to the things
            return tBool && uBool;
        }

        ///<summary> Checks if the two lines in 2D will collide with each other </summary>
        public static bool LineIntersection2D( float x1, float x2, float x3, float x4,
        float y1, float y2, float y3, float y4 ) =>
            LineIntersection2D( x1, x2, x3, x4, y1, y2, y3, y4, out _, out _ );

        #endregion

        #region Sigmoid

        ///<summary> This will map the whole real Number line into the range of 0 - 1
        /// <para>using calculation 1f / (Math.Pow(Math.E, -x)); </para> </summary>
        public static float Sigmoid_1( float x ) => 1f / Mathf.Pow( E, -x );

        ///<summary> This will map the whole real Number line into the range of 0 - 1
        /// <para>using calculation Math.Pow(Math.E, x) / (Math.Pow(Math.E, x) + 1f);</para> </summary>
        public static float Sigmoid_2( float x ) => Mathf.Pow( E, x ) / ( Mathf.Pow( E, x ) + 1f );

        #endregion

        #region Angles

        #region Angle Conversion

        ///<summary> Convert's a given degree angle into radians </summary>
        ///<param name="deg"> The converting Degrees </param>
        public static float DegToRad( float deg ) => deg * Mathf.Deg2Rad;

        ///<summary> Convert's a given radiant angle to degree </summary>
        ///<param name="rad"> The converting Radians </param>
        public static float RadToDeg( float rad ) => rad * Mathf.Rad2Deg;

        #endregion

        #endregion

        #region Other useful methods

        /// <summary>
        /// I've written this because C#'s % isn't really modulo arithmetic</br>
        /// this wraps the number back to the target one
        /// </summary>
        public static float Modulo( float a, float b )
        {
            //my own wrapping algorithm for C#
            return ( b + a % b ) % b;
        }

        /// <inheritdoc cref="Modulo(float, float)"/>
        public static int Modulo( int a, int b )
        {
            return ( b + a % b ) % b;
        }

        public static int RoundOnStep( int value, int step ) =>
            (int)Mathf.Round( (float)value / step ) * step;

        public static float RoundOnStep( float value, float step ) =>
            Mathf.Round( value / step ) * step;

        /// <summary> The summifying function </summary>
        public static int SumI( int startI, int endI, Func<int, int> function )
        {
            int ans = 0;
            for ( int i = startI; i <= endI; i++ )
                ans += function( i );
            return ans;
        }

        /// <inheritdoc cref="SumI"/>
        public static double SumD( int startI, int endI, Func<int, double> function )
        {
            double ans = 0;
            for ( int i = startI; i <= endI; i++ )
                ans += function( i );
            return ans;
        }

        /// <inheritdoc cref="SumI"/>
        public static float SumF( int startI, int endI, Func<int, float> function )
        {
            float ans = 0;
            for ( int i = startI; i <= endI; i++ )
                ans += function( i );
            return ans;
        }

        #endregion
    }

    /// <summary>
    /// Cx's Math Extensions
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// Checks if two floats are approximately equal
        /// </summary>
        /// <returns>If two float values are approximately equal together</returns>
        public static bool IsApproximately( this float value, float other ) =>
            Mathf.Approximately( value, other );
    }
}