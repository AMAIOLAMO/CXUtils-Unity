using System;
using UnityEngine;
using CXUtils.CodeUtils;

namespace CXUtils.PlaneSystem
{
    /// <summary> Options for Plane Dimentions </summary>
    public enum PlaneDimentionOptions { XY, XZ, YZ }

    /// <summary> A Plane class from CXUtils 
    /// <para> Used as a base class for other plane based classes </para> </summary>
    public struct Plane : IEquatable<Plane>
    {
        public Plane(PlaneDimentionOptions planeDimention) => PlaneDimention = planeDimention;

        public PlaneDimentionOptions PlaneDimention { get; set; }

        /// <inheritdoc cref="GetNormal(PlaneDimentionOptions)"/>
        public Vector3 GetNormal() => GetNormal(PlaneDimention);

        /// <summary>
        /// Get's the normal that the plane is facing
        /// </summary>
        public static Vector3 GetNormal(PlaneDimentionOptions planeDimentionOptions)
        {
            switch (planeDimentionOptions)
            {
                case PlaneDimentionOptions.XY: return -Vector3.forward;

                case PlaneDimentionOptions.XZ: return Vector3.up;

                case PlaneDimentionOptions.YZ: return Vector3.right;

                default: throw ExceptionUtils.GetException(ErrorType.NotAccessible);
            }
        }

        public bool Equals(Plane other) => other.PlaneDimention.Equals(PlaneDimention);
    }
}
