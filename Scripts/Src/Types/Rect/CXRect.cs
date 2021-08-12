namespace CXUtils.Types
{
    /// <summary>
    ///     Represents a rectangle using float
    /// </summary>
    public readonly struct RectFloat
    {
        public RectFloat(Float2 min, Float2 max) => (this.min, this.max) = (min, max);

        public readonly Float2 min, max;

        public Float2 Size => max - min;

        public Float2 Center => min + Size.Halve;

        public static RectFloat Create(Float2 min, Float2 size) => new RectFloat(min, min + size);

        public static implicit operator RectFloat(RectInt value) => new RectFloat((Float2)value.min, (Float2)value.max);
    }

    /// <summary>
    ///     Represents a rectangle using integer
    /// </summary>
    public readonly struct RectInt
    {
        public RectInt(Int2 min, Int2 max) => (this.min, this.max) = (min, max);

        public readonly Int2 min, max;

        public Int2 Size => max - min;

        public static RectInt Create(Int2 min, Int2 size) => new RectInt(min, min + size);
    }
}