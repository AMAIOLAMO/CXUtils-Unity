using CXUtils.Types;
using UnityEngine;

namespace CXUtils.Unity
{
    /// <summary>
    ///     Vector extension conversion methods
    /// </summary>
    public static class CXVectorConversions
    {
        #region Unity Conversion

        public static Vector2 ToUnity( this Float2 vector ) => new Vector2( vector.x, vector.y );
        public static Vector3 ToUnity( this Float3 vector ) => new Vector3( vector.x, vector.y, vector.z );
        public static Vector4 ToUnity( this Float4 vector ) => new Vector4( vector.x, vector.y, vector.z, vector.w );

        public static Vector2Int ToUnity( this Int2 vector ) => new Vector2Int( vector.x, vector.y );
        public static Vector3Int ToUnity( this Int3 vector ) => new Vector3Int( vector.x, vector.y, vector.z );
        public static Vector4 ToUnity( this Int4 vector ) => new Vector4( vector.x, vector.y, vector.z, vector.w );

        #endregion

        #region CXUtils Conversion

        public static Float2 ToCxType( this Vector2 vector ) => new Float2( vector.x, vector.y );
        public static Float3 ToCxType( this Vector3 vector ) => new Float3( vector.x, vector.y, vector.z );
        public static Float4 ToCxType( this Vector4 vector ) => new Float4( vector.x, vector.y, vector.z, vector.w );

        public static Int2 ToCxType( this Vector2Int vector ) => new Int2( vector.x, vector.y );
        public static Int3 ToCxType( this Vector3Int vector ) => new Int3( vector.x, vector.y, vector.z );

        #endregion
    }
}
