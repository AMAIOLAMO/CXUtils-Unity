using System;
using UnityEngine;

namespace CXUtils.CXMath
{

    ///<summary> Cx's Math Function Class </summary>
    public struct Mathf
    {
        public enum CheckRangeMode
        { valueLessEq, valueGreatEq, valueBothEq }

        ///<summary> This will check if the float is in the given range </summary>
        public static bool CheckFloatInRange(float x, float Min, float Max,
        CheckRangeMode checkRangeMode = CheckRangeMode.valueBothEq)
        {
            switch (checkRangeMode)
            {
                case CheckRangeMode.valueLessEq:
                return (x > Min && x <= Max);

                case CheckRangeMode.valueGreatEq:
                return (x >= Min && x < Max);

                default:
                return (x >= Min && x <= Max);
            }
        }

        ///<summary> Map the given value from the given range to the another given range </summary>
        public static float Map(float val, float in_min, float in_max, float out_min, float out_max) =>
            ((val - in_min) * (out_max - out_min)) / (in_max - in_min) + out_min;

        ///<summary> Checks if the two lines in 2D will collide with each other </summary>
        public static bool LineIntersection2D(float x1, float x2, float x3, float x4,
        float y1, float y2, float y3, float y4, out float t, out float u)
        {
            //write the line intersection
            float t_up = (x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4);
            float u_up = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3));
            float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            //calculate
            (t, u) = (t_up / den, u_up / den);

            //make boolean and check
            bool t_Bool, u_Bool;
            (t_Bool, u_Bool) = (CheckFloatInRange(t, 0f, 1f), CheckFloatInRange(u, 0f, 1f));
            //making the bool to the things
            return (t_Bool && u_Bool);
        }

        ///<summary> Checks if the two lines in 2D will collide with each other </summary>
        public static bool LineIntersection2D(float x1, float x2, float x3, float x4,
        float y1, float y2, float y3, float y4) =>
            LineIntersection2D(x1, x2, x3, x4, y1, y2, y3, y4, out _, out _);

        ///<summary> This will map the whole real Number line into the range of 0 - 1 using calculation 
        ///1f / ((float)Math.Pow(Math.E, -x)); </summary>
        public static float Sigmoid_1(float x) =>
            1f / ((float)Math.Pow(Math.E, -x));

        ///<summary> This will map the whole real Number line into the range of 0 - 1 using calculation 
        ///(float)Math.Pow(Math.E, x) / ((float)Math.Pow(Math.E, x) + 1f); </summary>
        public static float Sigmoid_2(float x) =>
            (float)Math.Pow(Math.E, x) / ((float)Math.Pow(Math.E, x) + 1f);

        ///<summary> Convert's a given degree angle into radiants </summary>
        ///<param name="deg"> The converting Degrees </param>
        public static float DegreeToRadiants(float deg) =>
            deg * UnityEngine.Mathf.Deg2Rad;

        ///<summary> Convert's a given radiant angle to degree </summary>
        ///<param name="rad"> The converting Radiants </param>
        public static float RadiantsToDegree(float rad) =>
            rad * UnityEngine.Mathf.Rad2Deg;

        ///<summary> Get's the angle between two vectors </summary>
        ///<param name="from"> The vector start with </param>
        ///<param name="to"> The vector end with </param>
        public static float AngleBetweenTwoVectors2D(Vector2 from, Vector2 to)
        {
            float angle = Vector2.Angle(from, to);
            Vector2 differenceBetweenFromAndTo = to - from;

            if (differenceBetweenFromAndTo.y < 0)
                angle = -angle;

            return angle;
        }

        /// <summary> The summification function Zigma </summary>
        /// <param name="start_i">The initial i of the zigma</param>
        /// <param name="end_i">the initial end of i of the zigma</param>
        /// <param name="function">The function that will use the i in calculation</param>
        public float Zigma(int start_i, int end_i, Func<float, float> function)
        {
            float ans = 0;
            for (int i = start_i; i <= end_i; i++)
                ans += function(i);
            return ans;
        }

    }

    ///+++++++++++++++++++++MATRIX++++++++++++++++++++++++++

    ///<summary>
    ///CX's Matrix calculations
    ///</summary>
    public class Matrix
    {
        public float[,] data;
        public int Rows { get; private set; }
        public int Colums { get; private set; }

        ///<summary> CXMath Matrix constructor </summary>
        public Matrix(int rows, int colums) =>
            (Rows, Colums, data) = (rows, colums, new float[rows, colums]);

        ///<summary> Uses the function in each matrix value </summary>
        public void Map(Func<float, float> Fn)
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Colums; j++)
                    data[i, j] = Fn(data[i, j]);
        }

        ///<summary> Uses the function in each matrix value </summary>
        public void Map(Func<float> Fn)
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Colums; j++)
                    data[i, j] = Fn();
        }

        ///<summary> Randomize each matrix value between -1 and 1 </summary>
        public void Randomize() =>
            Map(() => UnityEngine.Random.Range(-1, 1f));

        ///<summary> Randomize each matrix value between min and max </summary>
        public void Randomize(float min, float max) =>
            Map(() => UnityEngine.Random.Range(min, max));

        ///<summary> Adds the given Matrix to this matrix </summary>
        public Matrix Add(Matrix b)
        {
            Matrix a = this;
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Colums; j++)
                    a.data[i, j] += b.data[i, j];
            return a;
        }

        /// <summary> Subtracts the given Matrix to this matrix </summary>
        public Matrix Subtract(Matrix b)
        {
            Matrix a = this;
            Matrix _b = b;
            _b.Map((x) => -x);
            return a.Add(_b);
        }

        ///<summary> Multiply all the numbers with the given value </summary>
        public void Multiply(float b) =>
            Map((x) => x * b);

        ///<summary> Multiply the current matrix with the matrix given and returns it </summary>
        public Matrix Multiply(Matrix b)
        {
            Matrix result = new Matrix(Rows, b.Colums);
            float sum;
            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Colums; j++)
                {
                    sum = 0;
                    for (int k = 0; k < Colums; k++)
                        sum += data[i, k] * b.data[k, j];

                    result.data[i, j] = sum;
                }
            }
            return result;
        }
        /// <summary> This transposes the matrix </summary>
        public Matrix Transpose()
        {
            Matrix resultMatrix = new Matrix(Colums, Rows);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Colums; j++)
                    resultMatrix.data[j, i] = data[i, j];

            return resultMatrix;
        }

        public Matrix From1DArray(float[] arr)
        {
            Matrix resultMatrix = new Matrix(arr.Length, 1);
            for (int i = 0; i < resultMatrix.Rows; i++)
                resultMatrix.data[i, 0] = arr[i];
            return resultMatrix;
        }

        public Matrix From2DArray(float[,] arr)
        {
            Matrix resultMatrix = new Matrix(arr.GetLength(0), arr.GetLength(1));

            for (int i = 0; i < resultMatrix.Rows; i++)
                for (int j = 0; j < resultMatrix.Colums; j++)
                    resultMatrix.data[i, j] = arr[i, j];

            return resultMatrix;
        }

        public float[] To1DArray(Matrix a)
        {
            float[] array = new float[a.Rows * a.Colums];

            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Colums; j++)
                    array[i + j] = a.data[i, j];

            return array;
        }

        public float[,] To2DArray(Matrix a) => a.data;

        ///<summary> return's the whole thing into a string </summary>
        public override string ToString() => data.ToString();


        //Static methods +++++++++++++++++++++++++++++++++++++++++++++

        ///<summary> Adds the given a matrix to given b matrix </summary>
        public static Matrix Add(Matrix a, Matrix b)
        {
            Matrix result = a;
            result.Add(b);
            return result;
        }

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            Matrix result = a;
            result = result.Multiply(b);
            return result;
        }

        public static Matrix operator *(Matrix a, float b)
        {
            Matrix result = a;
            result.Multiply(b);
            return result;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            Matrix result = a;
            result = result.Multiply(b);
            return result;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            Matrix result = a;
            result = result.Add(b);
            return result;
        }
        public static Matrix operator -(Matrix a, Matrix b)
        {
            Matrix result = a;
            result = result.Subtract(b);
            return result;
        }

    }
}