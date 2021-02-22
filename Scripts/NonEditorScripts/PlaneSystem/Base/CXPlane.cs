using System;
using UnityEngine;
using CXUtils.CodeUtils;

namespace CXUtils.PlaneSystem
{
    /// <summary> Options for Plane Dimentions </summary>
    public enum PlaneDimentionOptions : byte
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
        public CXPlane(PlaneDimentionOptions planeDimention) => PlaneDimention = planeDimention;

        public PlaneDimentionOptions PlaneDimention { get; set; }

        /// <inheritdoc cref="GetNormal(PlaneDimentionOptions)"/>
        public Vector3 GetNormal() => GetNormal(PlaneDimention);

        /// <summary>
        /// Get's the normal that the plane is facing
        /// </summary>
        public static Vector3 GetNormal(PlaneDimentionOptions planeDimentionOptions)
        {
            switch ( planeDimentionOptions )
            {
                case PlaneDimentionOptions.XY:
                return -Vector3.forward;

                case PlaneDimentionOptions.XZ:
                return Vector3.up;

                case PlaneDimentionOptions.YZ:
                return Vector3.right;

                default:
                throw ExceptionUtils.GetException(ErrorType.NotAccessible);
            }
        }

        public bool Equals(CXPlane other) => other.PlaneDimention.Equals(PlaneDimention);
    }
}
