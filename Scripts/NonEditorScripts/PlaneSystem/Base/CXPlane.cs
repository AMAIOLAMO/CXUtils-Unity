using UnityEngine;

namespace CXUtils.PlaneSystem
{
    /// <summary> Options for Plane Dimentions </summary>
    public enum PlaneDimentionOptions
    {
        XY, XZ, YZ
    }

    /// <summary> A Plane class from CXUtils 
    /// <para> Used as a base class for other plane based classes </para> </summary>
    public class Plane
    {
        /// <summary> Get's the normal that the plane is facing </summary>
        public Vector3 GetFacingNormal(PlaneDimentionOptions planeDimentionOptions)
        {
            switch (planeDimentionOptions)
            {
                case PlaneDimentionOptions.XY:
                return new Vector3(0, 0, -1);

                case PlaneDimentionOptions.XZ:
                return Vector3.up;

                default:
                return Vector3.right;
            }
        }
    }
}
