using CXUtils.CodeUtils;

namespace CXUtils.UsefulTypes
{
    /// <summary> A class that implements ranges </summary>
    public abstract class CXRangeBase<T>
    {
        public T Min { get; private set; }
        public T Max { get; private set; }

        /// <summary> A simple range options for checking ranges </summary>
        public RangeOptions RangeOptions { get; set; }

        public CXRangeBase(T min, T max, RangeOptions rangeOptions = RangeOptions.IncBoth) =>
            (Min, Max, RangeOptions) = (min, max, rangeOptions);

        /// <summary> Checks the value in range </summary>
        public abstract bool InRange(T value);
    }

}
