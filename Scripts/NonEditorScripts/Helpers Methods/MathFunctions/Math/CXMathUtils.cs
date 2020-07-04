using System;
using UnityEngine;

using Random = UnityEngine.Random;

namespace CXUtils.CodeUtils
{
    /// <summary> Options flags for checking range </summary>
    public enum CheckRangeOptions
    { valueLessEq, valueGreatEq, valueBothEq, valueNotBoth }

    ///<summary> Cx's Math Function Class </summary>
    public struct MathUtils
    {
        // #region Test
        
        // private void Test()
        // {
            
        // }
        
        // #endregion

        #region Range Manipulation

        ///<summary> Returns if the float is in the given range </summary>
        public static bool CheckFloatInRange(float x, float Min, float Max,
        CheckRangeOptions checkRangeMode = CheckRangeOptions.valueBothEq)
        {
            switch (checkRangeMode)
            {
                case CheckRangeOptions.valueLessEq:
                    return (x > Min && x <= Max);

                case CheckRangeOptions.valueGreatEq:
                    return (x >= Min && x < Max);

                case CheckRangeOptions.valueBothEq:
                    return (x >= Min && x <= Max);
                    
                default:
                    return (x > Min && x < Max);
            }
        }

        ///<summary> Maps the given value from the given range to the another given range (no Safety Checks) </summary>
        /*
            Process:
            value - in_Min -- Making the range to map from 0
            * (out_Max - out_Min) -- multiply by the size of the out Range
            / (in_Max - in_Min) -- divide the size of the in_range (got the ans from range 0)
            + out_Min -- and offset the value back to the original range
        */
        public static float Map(float val, float in_min, float in_max, float out_min, float out_max) =>
            ((val - in_min) * (out_max - out_min)) / (in_max - in_min) + out_min;
        #endregion

        #region Lines

        ///<summary> Returns if the two lines will collide with each other </summary>
        public static bool LineIntersection2D(float x1, float x2, float x3, float x4,
        float y1, float y2, float y3, float y4, out float t, out float u)
        {
            //write the line intersection
            float t_up = (x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4);
            float u_up = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3));
            float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            //calculate
            (t, u) = (t_up / den, u_up / den);

            //make boolean and check
            bool t_Bool, u_Bool;
            (t_Bool, u_Bool) = (CheckFloatInRange(t, 0f, 1f), CheckFloatInRange(u, 0f, 1f));
            //making the bool to the things
            return (t_Bool && u_Bool);
        }

        ///<summary> Checks if the two lines in 2D will collide with each other </summary>
        public static bool LineIntersection2D(float x1, float x2, float x3, float x4,
        float y1, float y2, float y3, float y4) =>
            LineIntersection2D(x1, x2, x3, x4, y1, y2, y3, y4, out _, out _);

        #endregion

        #region Sigmoid

        ///<summary> This will map the whole real Number line into the range of 0 - 1
        /// <para>using calculation 1f / (Math.Pow(Math.E, -x)); </para> </summary>
        public static float Sigmoid_1(float x) =>
            1f / ((float)Math.Pow(Math.E, -x));

        ///<summary> This will map the whole real Number line into the range of 0 - 1
        /// <para>using calculation Math.Pow(Math.E, x) / (Math.Pow(Math.E, x) + 1f);</para> </summary>
        public static float Sigmoid_2(float x) =>
            (float)Math.Pow(Math.E, x) / ((float)Math.Pow(Math.E, x) + 1f);

        #endregion

        #region Angles

        #region Angle Conversion

        ///<summary> Convert's a given degree angle into radiants </summary>
        ///<param name="deg"> The converting Degrees </param>
        public static float DegreeToRadiants(float deg) =>
            deg * UnityEngine.Mathf.Deg2Rad;

        ///<summary> Convert's a given radiant angle to degree </summary>
        ///<param name="rad"> The converting Radiants </param>
        public static float RadiantsToDegree(float rad) =>
            rad * UnityEngine.Mathf.Rad2Deg;

        #endregion

        ///<summary> Get's the angle between two vectors </summary>
        ///<param name="from"> The vector start with </param>
        ///<param name="to"> The vector end with </param>
        public static float AngleBetweenVects2D(Vector2 from, Vector2 to)
        {
            float angle = Vector2.Angle(from, to);
            Vector2 diffBetweenFromAndTo = to - from;

            if (diffBetweenFromAndTo.y < 0)
                angle = -angle;

            return angle;
        }

        #endregion

        #region Random

        /// <summary> Randomly decides and returns a boolean </summary>
        public static bool FlipCoin(float threshHold = .5f) =>
            Random.Range(0f, 1f) > threshHold;

        /// <summary> Randomly decides between two items </summary>
        public static T FlipCoin<T>(T t1, T t2, float threshHold = .5f) =>
            FlipCoin(threshHold) ? t1 : t2;

        /// <summary> Randomly returns a float between 0 ~ 1 </summary>
        public static float RandomFloat() =>
            Random.Range(0f, 1f);

        #endregion

        #region Other useful methods

        /// <summary> Swaps two objects </summary>
        public static void Swap<T>(ref T T1, ref T T2) =>
            (T1, T2) = (T2, T1);

        /// <summary> The summification function Zigma </summary>
        public static float Zigma(int start_i, int end_i, Func<float, float> function)
        {
            float ans = 0;
            for (int i = start_i; i <= end_i; i++)
                ans += function(i);
            return ans;
        }

        #endregion
    }

}