using System;

namespace CXUtils.UsefulTypes
{
    /// <summary>
    ///     represents two floats
    /// </summary>
    [Serializable]
    public readonly struct Float2 : IEquatable<Float2>, IFormattable
    {
        public readonly float x, y;
        public Float2( float x, float y ) => ( this.x, this.y ) = ( x, y );
        public Float2( Float2 other ) => ( x, y ) = ( other.x, other.y );

        public static Float2 MinValue => new Float2( float.MinValue, float.MinValue );
        public static Float2 MaxValue => new Float2( float.MaxValue, float.MaxValue );
        public static Float2 Epsilon => new Float2( float.Epsilon, float.Epsilon );

        public static Float2 One => (Float2)1f;
        public static Float2 Half => (Float2).5f;
        public static Float2 Quarter => (Float2).25f;

        public static Float2 Up => new Float2( 0f, 1f );
        public static Float2 Down => new Float2( 0f, -1f );
        public static Float2 Left => new Float2( -1f, 0f );
        public static Float2 Right => new Float2( 1f, 0f );

        public float SqrMagnitude => x * x + y * y;
        public float Magnitude => (float)Math.Sqrt( SqrMagnitude );

        public Float2 Normalized => this / Magnitude;

        public Float2 Floor => new Float2( (float)Math.Floor( x ), (float)Math.Floor( y ) );
        public Float2 Ceil => new Float2( (float)Math.Ceiling( x ), (float)Math.Ceiling( y ) );

        public Int2 FloorInt => new Int2( (int)Math.Floor( x ), (int)Math.Floor( y ) );
        public Int2 CeilInt => new Int2( (int)Math.Ceiling( x ), (int)Math.Ceiling( y ) );

        public bool Equals( Float2 other ) => x.Equals( other.x ) && y.Equals( other.y );
        public override bool Equals( object obj ) => obj is Float2 other && Equals( other );
        public override int GetHashCode()
        {
            unchecked { return ( x.GetHashCode() * 397 ) ^ y.GetHashCode(); }
        }

        #region Operator overloading

        public static Float2 operator +( Float2 a, Float2 b ) => new Float2( a.x + b.x, a.y + b.y );
        public static Float2 operator -( Float2 a, Float2 b ) => new Float2( a.x - b.x, a.y - b.y );
        public static Float2 operator *( Float2 a, Float2 b ) => new Float2( a.x * b.x, a.y * b.y );
        public static Float2 operator /( Float2 a, Float2 b ) => new Float2( a.x / b.x, a.y / b.y );

        public static Float2 operator *( Float2 a, float value ) => new Float2( a.x * value, a.y * value );
        public static Float2 operator /( Float2 a, float value ) => new Float2( a.x / value, a.y / value );
        public static Float2 operator *( float value, Float2 a ) => new Float2( a.x * value, a.y * value );
        public static Float2 operator /( float value, Float2 a ) => new Float2( a.x / value, a.y / value );

        public static explicit operator Float2( float value ) => new Float2( value, value );

        #endregion

        #region Utility

        public float Dot( Float2 other ) => x * other.x + y * other.y;
        public Float2 Reflect( Float2 normal ) => this - 2f * Dot( normal ) * normal;

        public Float2 Min( Float2 other ) => new Float2( Math.Min( x, other.x ), Math.Min( y, other.y ) );
        public Float2 Max( Float2 other ) => new Float2( Math.Max( x, other.x ), Math.Max( y, other.y ) );

        public override string ToString() => "(" + x + ", " + y + ")";
        public string ToString( string format ) => "(" + x.ToString( format ) + ", " + y.ToString( format ) + ")";
        public string ToString( string format, IFormatProvider formatProvider ) =>
            "(" + x.ToString( format, formatProvider ) + ", " + y.ToString( format, formatProvider ) + ")";

        #endregion
    }

    [Serializable]
    public readonly struct Int2 : IEquatable<Int2>, IFormattable
    {
        public readonly int x, y;

        public Int2( int x, int y ) => ( this.x, this.y ) = ( x, y );
        public Int2( Int2 other ) => ( x, y ) = ( other.x, other.y );

        public static Int2 MinValue => new Int2( int.MinValue, int.MinValue );
        public static Int2 MaxValue => new Int2( int.MaxValue, int.MaxValue );

        public static Int2 One => (Int2)1;

        public static Int2 Up => new Int2( 0, 1 );
        public static Int2 Down => new Int2( 0, -1 );
        public static Int2 Left => new Int2( -1, 0 );
        public static Int2 Right => new Int2( 1, 0 );
        public bool Equals( Int2 other ) => x == other.x && y == other.y;
        public string ToString( string format, IFormatProvider formatProvider ) =>
            "(" + x.ToString( format, formatProvider ) + ", " + y.ToString( format, formatProvider ) + ")";
        public override bool Equals( object obj ) => obj is Int2 other && Equals( other );
        public override int GetHashCode()
        {
            unchecked { return ( x.GetHashCode() * 397 ) ^ y.GetHashCode(); }
        }

        public override string ToString() => "(" + x + ", " + y + ")";
        public string ToString( string format ) => "(" + x.ToString( format ) + ", " + y.ToString( format ) + ")";

        #region Operator overloading

        public static Int2 operator +( Int2 a, Int2 b ) => new Int2( a.x + b.x, a.y + b.y );
        public static Int2 operator -( Int2 a, Int2 b ) => new Int2( a.x - b.x, a.y - b.y );
        public static Int2 operator *( Int2 a, Int2 b ) => new Int2( a.x * b.x, a.y * b.y );
        public static Int2 operator /( Int2 a, Int2 b ) => new Int2( a.x / b.x, a.y / b.y );

        public static Int2 operator *( Int2 a, int value ) => new Int2( a.x * value, a.y * value );
        public static Int2 operator /( Int2 a, int value ) => new Int2( a.x / value, a.y / value );
        public static Int2 operator *( int value, Int2 a ) => new Int2( a.x * value, a.y * value );
        public static Int2 operator /( int value, Int2 a ) => new Int2( a.x / value, a.y / value );

        public static explicit operator Int2( int value ) => new Int2( value, value );

        #endregion

        #region Utility

        public Int2 Min( Int2 other ) => new Int2( Math.Min( x, other.x ), Math.Min( y, other.y ) );
        public Int2 Max( Int2 other ) => new Int2( Math.Max( x, other.x ), Math.Max( y, other.y ) );

        #endregion
    }
}
