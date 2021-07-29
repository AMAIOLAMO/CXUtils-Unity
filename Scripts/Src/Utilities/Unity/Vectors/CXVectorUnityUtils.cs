using UnityEngine;

namespace CXUtils.Types.UnityUtils
{
    public static class CxVectorUnityUtils
    {
        public static Vector2 ToUnity( this Float2 value ) => new Vector2( value.x, value.y );
        public static Vector3 ToUnity( this Float3 value ) => new Vector3( value.x, value.y, value.z );
        public static Vector4 ToUnity( this Float4 value ) => new Vector4( value.x, value.y, value.z, value.w );

        public static Vector2Int ToUnity( this Int2 value ) => new Vector2Int( value.x, value.y );
        public static Vector3Int ToUnity( this Int3 value ) => new Vector3Int( value.x, value.y, value.z );

        public static Float2 ToCxType( this Vector2 value ) => new Float2( value.x, value.y );
        public static Float3 ToCxType( this Vector3 value ) => new Float3( value.x, value.y, value.z );
        public static Float4 ToCxType( this Vector4 value ) => new Float4( value.x, value.y, value.z, value.w );

        public static Int2 ToCxType( this Vector2Int value ) => new Int2( value.x, value.y );
        public static Int3 ToCxType( this Vector3Int value ) => new Int3( value.x, value.y, value.z );
        public static Int4 ToCxTypeInt4( this Vector4 value ) => new Int4( (int)value.x, (int)value.y, (int)value.z, (int)value.w );
    }
}
