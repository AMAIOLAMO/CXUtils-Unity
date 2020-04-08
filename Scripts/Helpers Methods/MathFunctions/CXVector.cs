using UnityEngine;

namespace CXUtils.CXVectors
{
    ///<summary> CX's Vector class </summary>
    public class CXVector
    {

        ///<summary> Round the Given to Int </summary>
        public static Vector2Int RoundVector2ToInt(Vector2 vec2)
        {
            return new Vector2Int(Mathf.RoundToInt(vec2.x), Mathf.RoundToInt(vec2.y));
        }

        ///<summary> Round the Given to Int </summary>
        public static Vector3Int RoundVector3ToInt(Vector3 vec3)
        {
            return new Vector3Int(Mathf.RoundToInt(vec3.x), Mathf.RoundToInt(vec3.y), Mathf.RoundToInt(vec3.z));
        }

        public static Vector4 RoundVector4ToInt(Vector4 vec4)
        {
            return new Vector4(Mathf.RoundToInt(vec4.x), Mathf.RoundToInt(vec4.y), Mathf.RoundToInt(vec4.z), Mathf.RoundToInt(vec4.w));
        }
    }
}