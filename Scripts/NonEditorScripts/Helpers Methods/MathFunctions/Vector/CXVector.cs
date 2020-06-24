using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    ///<summary> CX's Vector class </summary>
    public struct VectorUtils
    {
        #region Floor & Ceil

        public static Vector2 FloorVec2(Vector2 vec2) =>
            Map_Vec2(vec2, (val) => Mathf.Floor(val));

        public static Vector3 FloorVec3(Vector3 vec3) =>
            Map_Vec3(vec3, (val) => Mathf.Floor(val));

        public static Vector4 FloorVec4(Vector4 vec4) =>
            Map_Vec4(vec4, (val) => Mathf.Floor(val));


        public static Vector2 CeilVec2(Vector2 vec2) =>
            Map_Vec2(vec2, (val) => Mathf.Ceil(val));

        public static Vector3 CeilVec3(Vector3 vec3) =>
            Map_Vec3(vec3, (val) => Mathf.Ceil(val));

        public static Vector4 CeilVec4(Vector4 vec4) =>
            Map_Vec4(vec4, (val) => Mathf.Ceil(val));

        #endregion

        #region Map

        /// <summary>
        /// Maps a function to every axis of the vector 2
        /// </summary>
        public static Vector2 Map_Vec2(Vector2 vec2, Func<float, float> mapFunc)
        {
            vec2.x = mapFunc(vec2.x);
            vec2.y = mapFunc(vec2.y);

            return vec2;
        }

        /// <summary>
        /// Maps a function to every axis of the vector 3
        /// </summary>
        public static Vector3 Map_Vec3(Vector3 vec3, Func<float, float> mapFunc)
        {
            vec3.x = mapFunc(vec3.x);
            vec3.y = mapFunc(vec3.y);
            vec3.z = mapFunc(vec3.z);

            return vec3;
        }

        /// <summary>
        /// Maps a function to every axis of the vector 4
        /// </summary>
        public static Vector4 Map_Vec4(Vector4 vec4, Func<float, float> mapFunc)
        {
            vec4.x = mapFunc(vec4.x);
            vec4.y = mapFunc(vec4.y);
            vec4.z = mapFunc(vec4.z);
            vec4.w = mapFunc(vec4.w);

            return vec4;
        }

        #endregion

        #region Rounding

        ///<summary> Round the Given to Int </summary>
        public static Vector2Int RoundVector2ToInt(Vector2 vec2)
        {
            vec2 = Map_Vec2(vec2, (val) => Mathf.Round(val));
            return new Vector2Int((int)vec2.x, (int)vec2.y);
        }

        ///<summary> Round the Given to Int </summary>
        public static Vector3Int RoundVector3ToInt(Vector3 vec3)
        {
            vec3 = Map_Vec3(vec3, (val) => Mathf.Round(val));
            return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
        }

        ///<summary> Round the Given to Int </summary>
        public static Vector4 RoundVector4ToInt(Vector4 vec4)
        {
            vec4 = Map_Vec4(vec4, (val) => Mathf.Round(val));
            return new Vector4((int)vec4.x, (int)vec4.y, (int)vec4.z, (int)vec4.w);
        }

        #endregion

        #region Math
        
        ///<summary> Get's the dot product of the given two vectors </summary>
        public static float Dot(Vector3 vect1, Vector3 vect2) =>
            vect1.x * vect2.x + vect1.y * vect2.y + vect1.z * vect2.z;
        
        ///<summary> Get's the dot product of the given two vectors </summary>
        public static float Dot(Vector2 vect1, Vector2 vect2) =>
            vect1.x * vect2.x + vect1.y * vect2.y;
        
        #endregion
    }
}