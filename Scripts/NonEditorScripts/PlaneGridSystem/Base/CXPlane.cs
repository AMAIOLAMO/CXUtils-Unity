using System;
using UnityEngine;
using CXUtils.CodeUtils;

namespace CXUtils.PlaneSystem
{
    /// <summary> Options for Plane Dimentions </summary>
    public enum PlaneDimensionOptions : byte
    {
        XY = 0,
        XZ = 1,
        YZ = 2
    }

    /// <summary>
    /// A Place data structure from CXUtils <br/>
    /// Used for determining what plane you are using in a 3Dimetional space
    /// </summary>
    public struct CXPlane : IEquatable<CXPlane>
    {
        public CXPlane(PlaneDimensionOptions planeDimension) => PlaneDimension = planeDimension;

        public PlaneDimensionOptions PlaneDimension { get; set; }

        /// <inheritdoc cref="GetNormal(PlaneDimensionOptions)"/>
        public Vector3 GetNormal() => GetNormal(PlaneDimension);

        /// <summary>
        /// Get's the normal that the plane is facing
        /// </summary>
        public static Vector3 GetNormal(PlaneDimensionOptions planeDimensionOptions)
        {
            switch ( planeDimensionOptions )
            {
                case PlaneDimensionOptions.XY:
                return -Vector3.forward;

                case PlaneDimensionOptions.XZ:
                return Vector3.up;

                case PlaneDimensionOptions.YZ:
                return Vector3.right;

                default:
                throw ExceptionUtils.Error.NotAccessible;
            }
        }

        public bool Equals(CXPlane other) => other.PlaneDimension.Equals(PlaneDimension);
    }
}
