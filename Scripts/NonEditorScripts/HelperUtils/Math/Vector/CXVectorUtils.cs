using System;
using UnityEngine;

using Random = UnityEngine.Random;

namespace CXUtils.CodeUtils
{
    ///<summary> CX's Vector class </summary>
    public class VectorUtils : IBaseUtils
    {
        #region Map

        /// <summary> Maps a function to every axis of the vector 2 </summary>
        public static Vector2 Map(Vector2 vec2, Func<float, float> mapFunc)
        {
            vec2.x = mapFunc(vec2.x);
            vec2.y = mapFunc(vec2.y);

            return vec2;
        }

        /// <summary> Maps a function to every axis of the vector 3 </summary>
        public static Vector3 Map(Vector3 vec3, Func<float, float> mapFunc)
        {
            vec3.x = mapFunc(vec3.x);
            vec3.y = mapFunc(vec3.y);
            vec3.z = mapFunc(vec3.z);

            return vec3;
        }

        /// <summary> Maps a function to every axis of the vector 4 </summary>
        public static Vector4 Map(Vector4 vec4, Func<float, float> mapFunc)
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
            
            if (diffY < 0)
                angle = -angle;

            return angle;
        }

        #endregion

        #region Random

        /// <summary> Generates a random vector2 </summary>
        public static Vector2 RandomVec2(float min, float max) =>
            new Vector2(Random.Range(min, max), Random.Range(min, max));

        /// <summary> Generates a random vector3 </summary>
        public static Vector3 RandomVec3(float min, float max) =>
            new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));

        /// <summary> Generates a random vector4 </summary>
        public static Vector4 RandomVec4(float min, float max) =>
            new Vector4(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));

        /// <summary> Generates a random vector2Int </summary>
        public static Vector2Int RandomVec2Int(int min, int max) =>
            new Vector2Int(Random.Range(min, max), Random.Range(min, max));

        /// <summary> Generates a random vector3Int </summary>
        public static Vector3Int RandomVec3Int(int min, int max) =>
            new Vector3Int(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));

        /// <summary> Generates a random vector4Int
        /// <para>QUICK NOTE: there is no Vector4Int, so becareful of using it as an integer</para></summary>
        public static Vector4 RandomVec4Int(int min, int max) =>
            new Vector4(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        #endregion

        #region Rounding

        ///<summary> Round the Given to Int </summary>
        public static Vector2 Round(Vector2 vec2)
        {
            vec2 = Map(vec2, (val) => Mathf.Round(val));
            return new Vector2(vec2.x, vec2.y);
        }

        ///<summary> Round the Given to Int </summary>
        public static Vector3 Round(Vector3 vec3)
        {
            vec3 = Map(vec3, (val) => Mathf.Round(val));
            return new Vector3(vec3.x, vec3.y, vec3.z);
        }

        ///<summary> Round the Given to Int </summary>
        public static Vector4 Round(Vector4 vec4)
        {
            vec4 = Map(vec4, (val) => Mathf.Round(val));
            return new Vector4(vec4.x, vec4.y, vec4.z, vec4.w);
        }

        ///<summary> Round the Given to Int </summary>
        public static Vector2Int RoundToInt(Vector2 vec2)
        {
            Vector2 rounded = Round(vec2);
            return new Vector2Int((int)rounded.x, (int)rounded.y);
        }

        ///<summary> Round the Given to Int </summary>
        public static Vector3Int RoundToInt(Vector3 vec3)
        {
            Vector3 rounded = Round(vec3);
            return new Vector3Int((int)rounded.x, (int)rounded.y, (int)rounded.z);
        }

        ///<summary> Round the Given to Int </summary>
        public static Vector4 RoundToInt(Vector4 vec4)
        {
            vec4 = Map(vec4, (val) => Mathf.RoundToInt(val));
            return new Vector4(vec4.x, vec4.y, vec4.z, vec4.w);
        }

        #endregion

        #region Floor & Ceil

        public static Vector2 Floor(Vector2 vec2) =>
            Map(vec2, (val) => Mathf.Floor(val));

        public static Vector3 Floor(Vector3 vec3) =>
            Map(vec3, (val) => Mathf.Floor(val));

        public static Vector4 Floor(Vector4 vec4) =>
            Map(vec4, (val) => Mathf.Floor(val));

        public static Vector2 Ceil(Vector2 vec2) =>
            Map(vec2, (val) => Mathf.Ceil(val));

        public static Vector3 Ceil(Vector3 vec3) =>
            Map(vec3, (val) => Mathf.Ceil(val));

        public static Vector4 Ceil(Vector4 vec4) =>
            Map(vec4, (val) => Mathf.Ceil(val));

        #endregion

        #region Vector Manipulations

        ///<summary> Set's the magnitude / length of the given vector to the given length </summary>
        public static Vector3 SetLength(Vector3 vect, float len)
        {
            vect = vect.normalized;
            vect *= len;
            return vect;
        }

        ///<summary> Clamps the vector by magnitude value </summary>
        public static Vector3 Clamp(Vector3 vect, float minMagnitude, float maxMagnitude)
        {
            if (vect.magnitude > maxMagnitude)
                return SetLength(vect, maxMagnitude);

            //else
            if (vect.magnitude < minMagnitude)
                return SetLength(vect, minMagnitude);

            //else
            return vect;
        }

        #endregion
    }
}