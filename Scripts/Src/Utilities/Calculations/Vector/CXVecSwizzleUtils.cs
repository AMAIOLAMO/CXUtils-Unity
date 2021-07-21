using UnityEngine;

namespace CXUtils.CodeUtils
{
    public static class CXVecSwizzleUtils
    {
        #region XYZ

            public static Vector2 XY(this Vector3 vec) =>
                new Vector2(vec.x, vec.y);
            
            public static Vector2 YX(this Vector3 vec) =>
                new Vector2(vec.y, vec.x);

            public static Vector2 XZ(this Vector3 vec) =>
                new Vector2(vec.x, vec.z);
            
            public static Vector2 ZX(this Vector3 vec) =>
                new Vector2(vec.z, vec.x);
            
            public static Vector2 YZ(this Vector3 vec) =>
                new Vector2(vec.y, vec.z);
            
            public static Vector2 ZY(this Vector3 vec) =>
                new Vector2(vec.z, vec.y);

            public static Vector2 XX(this Vector3 vec) =>
                new Vector2(vec.x, vec.x);

            public static Vector2 YY(this Vector3 vec) =>
                new Vector2(vec.y, vec.y);
            
            public static Vector2 ZZ(this Vector3 vec) =>
                new Vector2(vec.z, vec.z);

        #endregion
    }
}
