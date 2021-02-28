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
        ///<summary> Include both Min and Max </sumary>
        IncBoth,
        ///<summary> Exclude Both Min and Max </summary>
        ExcBoth
    }

    ///<summary> Cx's Math Function Class </summary>
    public struct MathUtils
    {
        /// <summary> Represents PI * 2 </summary>
        public const float TAU = 6.28318530717958f;

        #region Range Manipulation

        ///<summary> Returns if the float is in the given range </summary>
        public static bool ValueInRange(float x, float Min, float Max, RangeOptions checkRangeMode = RangeOptions.IncBoth)
        {
            switch ( checkRangeMode )
            {
                case RangeOptions.IncMax:
                return ( x > Min && x <= Max );
                case RangeOptions.IncMin:
                return ( x >= Min && x < Max );
                case RangeOptions.IncBoth:
                return ( x >= Min && x <= Max );
                default:
                return ( x > Min && x < Max );
            }
        }

        ///<summary>
        ///Returns if the double is in the given range
        ///</summary>
        public static bool ValueInRange(double x, double Min, double Max, RangeOptions checkRangeMode = RangeOptions.IncBoth)
        {
            switch ( checkRangeMode )
            {
                case RangeOptions.IncMax:
                return ( x > Min && x <= Max );
                case RangeOptions.IncMin:
                return ( x >= Min && x < Max );
                case RangeOptions.IncBoth:
                return ( x >= Min && x <= Max );
                default:
                return ( x > Min && x < Max );
            }
        }

        ///<summary> Maps the given value from the given range to the another given range (no Safety Checks) </summary>
        public static float Map(float val, float in_min, float in_max, float out_min, float out_max) =>
            ( val - in_min ) * ( out_max - out_min ) / ( in_max - in_min ) + out_min;

        #endregion

        #region Lines

        ///<summary> Returns if the two lines will collide with each other </summary>
        public static bool LineIntersection2D(float x1, float x2, float x3, float x4,
        float y1, float y2, float y3, float y4, out float t, out float u)
        {
            //write the line intersection
            float t_up = ( x1 - x3 ) * ( y3 - y4 ) - ( y1 - y3 ) * ( x3 - x4 );
            float u_up = -( ( x1 - x2 ) * ( y1 - y3 ) - ( y1 - y2 ) * ( x1 - x3 ) );
            float den = ( x1 - x2 ) * ( y3 - y4 ) - ( y1 - y2 ) * ( x3 - x4 );

            //calculate
            (t, u) = (t_up / den, u_up / den);

            //make boolean and check
            bool t_Bool, u_Bool;
            (t_Bool, u_Bool) = (ValueInRange(t, 0f, 1f), ValueInRange(u, 0f, 1f));

            //making the bool to the things
            return t_Bool && u_Bool;
        }

        ///<summary> Checks if the two lines in 2D will collide with each other </summary>
        public static bool LineIntersection2D(float x1, float x2, float x3, float x4,
        float y1, float y2, float y3, float y4) =>
            LineIntersection2D(x1, x2, x3, x4, y1, y2, y3, y4, out _, out _);

        #endregion

        #region Sigmoid

        ///<summary> This will map the whole real Number line into the range of 0 - 1
        /// <para>using calculation 1f / (Math.Pow(Math.E, -x)); </para> </summary>
        public static float Sigmoid_1(float x) => 1f / ( (float)Math.Pow(Math.E, -x) );

        ///<summary> This will map the whole real Number line into the range of 0 - 1
        /// <para>using calculation Math.Pow(Math.E, x) / (Math.Pow(Math.E, x) + 1f);</para> </summary>
        public static float Sigmoid_2(float x) => (float)Math.Pow(Math.E, x) / ( (float)Math.Pow(Math.E, x) + 1f );

        #endregion

        #region Angles

        #region Angle Conversion

        ///<summary> Convert's a given degree angle into radiants </summary>
        ///<param name="deg"> The converting Degrees </param>
        public static float DegToRad(float deg) => deg * UnityEngine.Mathf.Deg2Rad;

        ///<summary> Convert's a given radiant angle to degree </summary>
        ///<param name="rad"> The converting Radiants </param>
        public static float RadToDeg(float rad) => rad * UnityEngine.Mathf.Rad2Deg;

        #endregion

        #endregion

        #region Other useful methods

        /// <summary> The summifying function </summary>
        public static int SumInt(int start_i, int end_i, Func<int, int> function)
        {
            int ans = 0;
            for ( int i = start_i; i <= end_i; i++ )
                ans += function(i);
            return ans;
        }

        /// <inheritdoc cref="SumInt(int, int, Func{int, int})"/>
        public static double SumDouble(int start_i, int end_i, Func<int, double> function)
        {
            double ans = 0;
            for ( int i = start_i; i <= end_i; i++ )
                ans += function(i);
            return ans;
        }

        /// <inheritdoc cref="SumInt(int, int, Func{int, int})"/>
        public static float SumFloat(int start_i, int end_i, Func<int, float> function)
        {
            float ans = 0;
            for ( int i = start_i; i <= end_i; i++ )
                ans += function(i);
            return ans;
        }

        #endregion
    }

    /// <summary>
    /// Cx's Float Extensions
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Checks if two floats are approximately equal
        /// </summary>
        /// <returns>If two float values are approximately equal together</returns>
        public static bool IsApproximately(this float value, float other) =>
            Mathf.Approximately(value, other);
    }
}