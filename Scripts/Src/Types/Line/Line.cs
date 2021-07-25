using CXUtils.CodeUtils;

namespace CXUtils.Types
{
    /// <summary>
    ///     Represents a single line in 2D
    /// </summary>
    public readonly struct LineFloat2
    {
        public readonly Float2 position1, position2;
        public LineFloat2( Float2 position1, Float2 position2 ) => ( this.position1, this.position2 ) = ( position1, position2 );

        /// <summary>
        ///     Samples a point in between the two points using <paramref name="t" />
        /// </summary>
        public Float2 Sample( float t ) => TweenUtils.Lerp( position1, position2, t );

        public static explicit operator LineFloat2( LineFloat3 value ) => new LineFloat2( (Float2)value.position1, (Float2)value.position2 );
    }

    /// <summary>
    ///     Represents a single line in 3D
    /// </summary>
    public readonly struct LineFloat3
    {
        public readonly Float3 position1, position2;
        public LineFloat3( Float3 position1, Float3 position2 ) => ( this.position1, this.position2 ) = ( position1, position2 );

        /// <summary>
        ///     Samples a point in between the two points using <paramref name="t" />
        /// </summary>
        public Float3 Sample( float t ) => TweenUtils.Lerp( position1, position2, t );

        public static implicit operator LineFloat3( LineFloat2 value ) => new LineFloat3( value.position1, value.position2 );
    }
}
