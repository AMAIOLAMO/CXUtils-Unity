using CXUtils.CodeUtils;

namespace CXUtils.Types
{
    /// <summary>
    ///     Represents a single line in 2D
    /// </summary>
    public readonly struct LineFloat2
    {
        public readonly Float2 positionA, positionB;
        public LineFloat2( Float2 positionA, Float2 positionB ) => ( this.positionA, this.positionB ) = ( positionA, positionB );

        /// <summary>
        ///     Samples a point in between the two points using <paramref name="t" />
        /// </summary>
        public Float2 Sample( float t ) => TweenUtils.Lerp( positionA, positionB, t );

        public static explicit operator LineFloat2( LineFloat3 value ) => new LineFloat2( (Float2)value.positionA, (Float2)value.positionB );
    }

    /// <summary>
    ///     Represents a single line in 3D
    /// </summary>
    public readonly struct LineFloat3
    {
        public readonly Float3 positionA, positionB;
        public LineFloat3( Float3 positionA, Float3 positionB ) => ( this.positionA, this.positionB ) = ( positionA, positionB );

        /// <summary>
        ///     Samples a point in between the two points using <paramref name="t" />
        /// </summary>
        public Float3 Sample( float t ) => TweenUtils.Lerp( positionA, positionB, t );
        
        public static implicit operator LineFloat3( LineFloat2 value ) => new LineFloat3( value.positionA, value.positionB );
    }
}
