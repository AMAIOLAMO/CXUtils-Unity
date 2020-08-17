using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CXUtils.CodeUtils
{
    ///<summary> CX's Vector class </summary>
    public static class VectorUtils
    {
        #region Mapping Extensions

        /// <summary>
        /// Maps a function to every axis of the <paramref name="vec2"/>
        /// </summary>
        public static Vector2 Map(this ref Vector2 vec2, Func<float, float> mapFunc)
        {
            vec2.x = mapFunc(vec2.x);
            vec2.y = mapFunc(vec2.y);

            return vec2;
        }

        /// <summary>
        /// Maps a function to every axis of the <paramref name="vec2"/>
        /// </summary>
        public static Vector2Int Map(this ref Vector2Int vec2, Func<int, int> mapFunc)
        {
            vec2.x = mapFunc(vec2.x);
            vec2.y = mapFunc(vec2.y);

            return vec2;
        }


        /// <summary>
        /// Maps a function to every axis of the <paramref name="vec3"/>
        /// </summary>
        public static Vector3 Map(this ref Vector3 vec3, Func<float, float> mapFunc)
        {
            vec3.x = mapFunc(vec3.x);
            vec3.y = mapFunc(vec3.y);
            vec3.z = mapFunc(vec3.z);

            return vec3;
        }

        /// <summary>
        /// Maps a function to every axis of the <paramref name="vec3"/>
        /// </summary>
        public static Vector3Int Map(this ref Vector3Int vec3, Func<int, int> mapFunc)
        {
            vec3.x = mapFunc(vec3.x);
            vec3.y = mapFunc(vec3.y);
            vec3.z = mapFunc(vec3.z);

            return vec3;
        }


        /// <summary>
        /// Maps a function to every axis of the <paramref name="vec4"/>
        /// </summary>
        public static Vector4 Map(this ref Vector4 vec4, Func<float, float> mapFunc)
        {
            vec4.x = mapFunc(vec4.x);
            vec4.y = mapFunc(vec4.y);
            vec4.z = mapFunc(vec4.z);
            vec4.w = mapFunc(vec4.w);

            return vec4;
        }

        /// <summary>
        /// Maps a function to every axis of the <paramref name="vec4"/>
        /// </summary>
        public static Vector4 Map(this ref Vector4 vec4, Func<float, int> mapFunc)
        {
            vec4.x = mapFunc(vec4.x);
            vec4.y = mapFunc(vec4.y);
            vec4.z = mapFunc(vec4.z);
            vec4.w = mapFunc(vec4.w);

            return vec4;
        }

        #endregion

        #region Math

        ///<summary> Get's the angle between two vectors </summary>
        ///<param name="from"> The vector start with </param>
        ///<param name="to"> The vector end with </param>
        public static float AngleBetweenVects2D(Vector2 from, Vector2 to)
        {
            float angle = Vector2.Angle(from, to);
            float diffY = to.y - from.y;

            if (diffY < 0) angle = -angle;

            return angle;
        }

        #endregion

        #region Random

        /// <summary> Generates a random vector2 </summary>
        public static Vector2 RandomVec2(float min, float max) => new Vector2(Random.Range(min, max), Random.Range(min, max));

        /// <summary> Generates a random vector2Int </summary>
        public static Vector2Int RandomVec2Int(int min, int max) => new Vector2Int(Random.Range(min, max), Random.Range(min, max));


        /// <summary> Generates a random vector3 </summary>
        public static Vector3 RandomVec3(float min, float max) => new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));

        /// <summary> Generates a random vector3Int </summary>
        public static Vector3Int RandomVec3Int(int min, int max) => new Vector3Int(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));


        /// <summary> Generates a random vector4 </summary>
        public static Vector4 RandomVec4(float min, float max) => new Vector4(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));

        /// <summary> Generates a random vector4Int
        /// <para>QUICK NOTE: there is no Vector4Int, so becareful of using it as an integer</para></summary>
        public static Vector4 RandomVec4Int(int min, int max) => new Vector4(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));

        /// <summary>
        /// Generates a random direction based on the given direction
        /// <para>QUICK NOTE: this method offsets the current vector</para>
        /// </summary>
        public static Vector3 RandomDirection(Vector3 direction)
        {
            //get's the original magnitude
            float originMagnitude = direction.magnitude;

            direction = direction.normalized;
            //adding a new direction on to the original direction
            direction += RandomVec3(0f, 1f);
            direction.SetLength(originMagnitude);

            return direction;
        }

        #endregion

        #region Rounding Extensions

        ///<summary>
        ///Round the Given vector
        ///</summary>
        public static Vector2 Round(this ref Vector2 vec2) => vec2.Map((val) => Mathf.Round(val));

        ///<summary>
        ///Round the Given vector
        ///</summary>
        public static Vector3 Round(this ref Vector3 vec3) => vec3.Map((val) => Mathf.Round(val));

        ///<summary>
        ///Round the Given vector
        ///</summary>
        public static Vector4 Round(this ref Vector4 vec4) => vec4.Map((val) => Mathf.Round(val));

        ///<summary>
        ///Round the Given vector to their int version
        ///</summary>
        public static Vector2Int GetRoundToInt(this Vector2 vec2)
        {
            Vector2 rounded = vec2.Round();
            return new Vector2Int((int) rounded.x, (int) rounded.y);
        }

        ///<summary>
        ///Round the Given vector to their int version
        ///<para>Returns the new version, but does not override the original version</para>
        ///</summary>
        public static Vector3Int GetRoundToInt(this Vector3 vec3)
        {
            Vector3 rounded = vec3.Round();
            return new Vector3Int((int) rounded.x, (int) rounded.y, (int) rounded.z);
        }

        ///<summary>
        ///Round the Given vector to their int version
        ///<para>Returns the new version, but does not override the original version</para>
        ///</summary>
        public static Vector4 GetRoundToInt(this Vector4 vec4) => vec4.Map((val) => Mathf.RoundToInt(val));

        #endregion

        #region Floor & Ceil Extensions

        public static Vector2 Floor(this Vector2 vec2) => vec2.Map((val) => Mathf.Floor(val));

        public static Vector3 Floor(this Vector3 vec3) => vec3.Map((val) => Mathf.Floor(val));

        public static Vector4 Floor(this Vector4 vec4) => vec4.Map((val) => Mathf.Floor(val));

        public static Vector2 Ceil(this Vector2 vec2) => vec2.Map((val) => Mathf.Ceil(val));

        public static Vector3 Ceil(this Vector3 vec3) => vec3.Map((val) => Mathf.Ceil(val));

        public static Vector4 Ceil(this Vector4 vec4) => vec4.Map((val) => Mathf.Ceil(val));

        #endregion

        #region Vector Manipulation Extensions & Utils

        /// <summary>
        /// Get's the length of this Vector
        /// </summary>
        public static float GetLength(this Vector2 vect) => Mathf.Sqrt(vect.x * vect.x + vect.y * vect.y);

        /// <inheritdoc cref="GetLength(Vector2)"/>
        public static float GetLength(this Vector3 vect) => Mathf.Sqrt(vect.x * vect.x + vect.y * vect.y + vect.z * vect.z);

        /// <inheritdoc cref="GetLength(Vector2)"/>
        public static float GetLength(this Vector4 vect) => Mathf.Sqrt(vect.x * vect.x + vect.y * vect.y + vect.z * vect.z + vect.w * vect.w);


        ///<summary>
        ///Set's the magnitude / length of the <paramref name="vect"/> to the given length
        ///</summary>
        public static Vector3 SetLength(this ref Vector3 vect, float len) => vect = vect.normalized * len;

        /// <inheritdoc cref="SetLength(ref Vector3, float)"/>
        public static Vector2 SetLength(this ref Vector2 vect, float len) => vect = vect.normalized * len;

        /// <inheritdoc cref="SetLength(ref Vector3, float)"/>
        public static Vector4 SetLength(this ref Vector4 vect, float len) => vect = vect.normalized * len;


        ///<summary> Clamps the vector by magnitude value </summary>
        public static Vector3 Clamp(Vector3 vect, float minLen, float maxLen) => vect.magnitude > maxLen ? vect.SetLength(maxLen) : (vect.magnitude < minLen ? vect.SetLength(minLen) : vect);

        #endregion
    }
}