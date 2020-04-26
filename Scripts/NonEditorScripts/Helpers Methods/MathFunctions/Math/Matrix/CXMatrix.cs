using System;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace CXUtils.CodeUtils
{
    /// <summary> A matrix class from CXUtils </summary>
    [Serializable]
    public struct Matrix
    {
        #region Fields

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary> The Data of this Matrix </summary>
        public float[,] Data { get; private set; }

        public float this[int x, int y]
        {
            get => Data[x, y];
            set => Data[x, y] = value;
        }

        #endregion

        #region Constructor

        public Matrix(int width, int height)
        {
            (Width, Height) = (width, height);

            Data = new float[width, height];
            Map(() => 0);
        }

        public Matrix(int width, int height, float initialValue)
        {
            (Width, Height) = (width, height);

            Data = new float[width, height];
            Map(() => initialValue);
        }

        #endregion

        #region Mapping

        /// <summary> Mapping the matrix with another function that is given the coords of this matrix </summary>
        public void Map(Func<int, int, float> Fn)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    this[x, y] = Fn(x, y);
        }

        /// <summary> Mapping the matrix with another function that is given this matrix's data </summary>
        public void Map(Func<float, float> Fn)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    this[x, y] = Fn(this[x, y]);
        }

        /// <summary> Mapping the data with another function </summary>
        public void Map(Func<float> Fn)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    this[x, y] = Fn();
        }

        #endregion

        #region Matrix Methods

        /// <summary> Iterates through this whole matrix </summary>
        public void Iterate(Action<float> action, Action onNextLineAction = null)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    action?.Invoke(this[x, y]);
                onNextLineAction?.Invoke();
            }
        }

        /// <summary> Iterates through this whole matrix </summary>
        public void Iterate(Action<float, int, int> action, Action onNextLineAction = null)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    action?.Invoke(this[x, y], x, y);
                onNextLineAction?.Invoke();
            }
        }

        /// <summary> Iterates through this whole matrix </summary>
        public void Iterate(Action<float> action) =>
            Iterate(action, null);

        /// <summary> Iterates through this whole matrix </summary>
        public void Iterate(Action<float, int, int> action) =>
            Iterate(action, null);

        #endregion

        #region Matrix Manipulations

        /// <summary> Transposed this Matrix </summary>
        public void Transpose()
        {
            Matrix result = new Matrix(Height, Width);
            Iterate((f, x, y) => result[y, x] = f);
            this = result;
        }

        /// <summary> Gets the Transposed version of this Matrix </summary>
        public Matrix GetTranspose()
        {
            Matrix result = new Matrix(Height, Width);
            Iterate((f, x, y) => result[y, x] = f);
            return result;
        }

        #endregion

        #region Operations

        #region +-

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.SizeEquals(b))
            {
                Matrix result = a;
                result.Map((x, y) =>
                    result[x, y] + b[x, y]);

                return result;
            }
            throw new IndexOutOfRangeException("The Matrix size should be exactly the same!");
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.SizeEquals(b))
            {
                Matrix result = a;
                result.Map((x, y) =>
                    result[x, y] - b[x, y]);

                return result;
            }
            throw new IndexOutOfRangeException("The Matrix size should be exactly the same!");
        }

        #endregion

        #region Multiplication

        public static Matrix operator *(Matrix a, float b)
        {
            Matrix result = a;
            result.Map((data) => b * data);
            return result;
        }

        public static Matrix operator *(float a, Matrix b) =>
            b * a;

        public static Matrix operator *(Matrix a, Matrix b)
        {

            if (a.MultiplyableEquals(b))
            {
                //the multiplied result
                Matrix result = new Matrix(b.Width, a.Height);

                float sum;
                //looping inside the first one
                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        sum = 0;
                        for (int k = 0; k < a.Width; k++)
                            sum += a[k, y] * b[x, k];

                        result[x, y] = sum;
                    }
                }
                return result;
            }
            //else
            throw new IndexOutOfRangeException("Matrix multiplication should follow a.width = b.height rules!");

        }

        #endregion

        #endregion

        #region Script Utils

        /// <summary> Checks if the size of this matrix is equals to the other </summary>
        public bool SizeEquals(Matrix other) =>
            (other.Width.Equals(Width) && other.Height.Equals(Height));

        /// <summary> Checks if the other and this is multiplyable </summary> 
        public bool MultiplyableEquals(Matrix other) =>
            Width.Equals(other.Height);

        /// <summary> Checks if this equals to the other </summary>
        public bool Equals(Matrix other)
        {
            if (SizeEquals(other))
            {
                //looping in and find if every elem of this matrix is equals to the other matrix

                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                        if (!this[x, y].Equals(other[x, y]))
                            return false;

                return true;
            }

            return false;
        }

        /// <summary> Turns the matrix to string (Origin left down) </summary>
        public new string ToString()
        {
            StringBuilder strB = new StringBuilder();

            Iterate((f) => strB.Append($" {f}"), () => strB.Append('\n'));

            return strB.ToString();
        }

        #endregion
    }
}
