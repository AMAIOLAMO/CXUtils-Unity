using System;
using CXUtils.CodeUtils;
using CXUtils.UsefulTypes;

namespace CXUtils.PlaneSystem
{
    /// <summary>
    ///     Options for Plane Dimensions
    /// </summary>
    public enum PlaneDimensionOptions { XY, XZ, YZ }

    /// <summary>
    ///     A data structure for converting plane axis to 3D axis
    /// </summary>
    public readonly struct Plane : IEquatable<Plane>, IFormattable
    {
        public Plane( PlaneDimensionOptions dimension ) => this.dimension = dimension;

        public readonly PlaneDimensionOptions dimension;

        /// <summary>
        /// Samples a plane position and converts to a world position according to the plane
        /// </summary>
        public Float3 Sample( Float2 planePosition )
        {
            switch ( dimension )
            {
                case PlaneDimensionOptions.XY: return new Float3( planePosition.x, planePosition.y, 0 );
                case PlaneDimensionOptions.XZ: return new Float3( planePosition.x, 0, planePosition.y );
                case PlaneDimensionOptions.YZ: return new Float3( 0f, planePosition.x, planePosition.y );
                default: throw ExceptionUtils.Error.NotAccessible;
            }
        }

        public bool Equals( Plane other ) => other.dimension.Equals( dimension );
        public string ToString( string format, IFormatProvider formatProvider ) => "dimension: " + dimension;
    }
}
