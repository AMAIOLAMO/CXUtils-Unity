using System;
using CXUtils.CodeUtils;

namespace CXUtils.UsefulTypes
{
    /// <summary> A base class for making ranging classes </summary>
    public abstract class RangeValueBase<T> : CXRangeBase<T>
    {
        #region Vars

        public T Value { get; set; }

        #endregion

        public RangeValueBase(T value, T min, T max) : base(min, max)
        {
            if (!InRange(value))
                ExceptionUtils.ThrowException<Exception>($"[{nameof(RangeValueBase<T>)}] Constructor Error: Value out of range!");

            Value = value;
        }

        #region Methods

        /// <summary> Tries to set the value </summary>
        public virtual bool TrySetValue(T value)
        {
            if (!InRange(value))
                return false;

            Value = value;
            return true;
        }

        #endregion
    }

    public class RangeInt : RangeValueBase<int>
    {
        public RangeInt(int value, int min, int max) : base(value, min, max) { }

        public override bool InRange(int value) =>
            MathUtils.ValueInRange(value, Min, Max, RangeOptions);
    }

    public class RangeFloat : RangeValueBase<float>
    {
        public RangeFloat(float value, float min, float max) : base(value, min, max) { }

        public override bool InRange(float value) =>
            MathUtils.ValueInRange(value, Min, Max, RangeOptions);
    }

    public class RangeDouble : RangeValueBase<double>
    {
        public RangeDouble(double value, double min, double max) : base(value, min, max) { }

        public override bool InRange(double value) =>
            MathUtils.ValueInRange(value, Min, Max, RangeOptions);
    }
}