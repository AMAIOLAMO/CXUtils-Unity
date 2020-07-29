using System;
using CXUtils.CodeUtils;
using UnityEngine;

namespace CXUtils.UsefulTypes
{
    /// <summary>
    /// A class that checks for ranges
    /// </summary>
    public abstract class CXRangeBase<T>
    {
        /// <summary>
        /// The min value of this range
        /// </summary>
        public T Min { get; private set; }

        /// <summary>
        /// The max value of this range
        /// </summary>
        public T Max { get; private set; }

        /// <summary> A simple range options for checking ranges </summary>
        public RangeOptions RangeOptions { get; set; }

        public CXRangeBase(T min, T max, RangeOptions rangeOptions = RangeOptions.IncBoth) => (Min, Max, RangeOptions) = (min, max, rangeOptions);

        /// <summary> Checks if <paramref name="value"/> is in range </summary>
        public abstract bool InRange(T value);
    }

    /// <summary>
    /// Range checking for int
    /// </summary>
    public class RangeInt : CXRangeBase<int>, ICloneable
    {
        public RangeInt(int min, int max, RangeOptions rangeOptions = RangeOptions.IncBoth) : base(min, max, rangeOptions) 
        {
            if (min > max) throw new ArgumentOutOfRangeException($"{nameof(min)} is bigger than {nameof(max)} and that does not make sense!");
        }

        public object Clone() => new RangeInt(Min, Max, RangeOptions);

        public override bool InRange(int value) => MathUtils.ValueInRange(value, Min, Max, RangeOptions);
    }

    /// <summary>
    /// Range checking for float
    /// </summary>
    public class RangeFloat : CXRangeBase<float>, ICloneable
    {
        public RangeFloat(float min, float max, RangeOptions rangeOptions = RangeOptions.IncBoth) : base(min, max, rangeOptions) 
        {
            if (min > max) throw new ArgumentOutOfRangeException($"{nameof(min)} is bigger than {nameof(max)} and that does not make sense!");
        }

        public object Clone() => new RangeFloat(Min, Max, RangeOptions);

        public override bool InRange(float value) => MathUtils.ValueInRange(value, Min, Max, RangeOptions);
    }

    /// <summary>
    /// Range checking for double
    /// </summary>
    public class RangeDouble : CXRangeBase<double>, ICloneable
    {
        public RangeDouble(double min, double max, RangeOptions rangeOptions = RangeOptions.IncBoth) : base(min, max, rangeOptions)
        {
            if (min > max) throw new ArgumentOutOfRangeException($"{nameof(min)} is bigger than {nameof(max)} and that does not make sense!");
        }

        public object Clone() => new RangeDouble(Min, Max, RangeOptions);

        public override bool InRange(double value) => MathUtils.ValueInRange(value, Min, Max, RangeOptions);
    }

    //Just extensions for ranges
    public static class CXRangeExtensions
    {
        public static Vector2Int ToVector2Int(this RangeInt range) => new Vector2Int(range.Min, range.Max);
        public static Vector2 ToVector2(this RangeFloat range) => new Vector2(range.Min, range.Max);
        public static Vector2 ToVector2(this RangeDouble range) => new Vector2((float)range.Min, (float)range.Max);
    }
}
