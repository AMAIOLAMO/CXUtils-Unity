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

        public static Float2 MinValue => (Float2)float.MinValue;
        public static Float2 MaxValue => (Float2)float.MaxValue;
        public static Float2 Epsilon => (Float2)float.Epsilon;

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
        public static explicit operator Float2( Float3 value ) => new Float2( value.x, value.y );

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

    /// <summary>
    ///     represents three floats
    /// </summary>
    [Serializable]
    public readonly struct Float3 : IEquatable<Float3>, IFormattable
    {
        public readonly float x, y, z;
        public Float3( float x, float y, float z ) => ( this.x, this.y, this.z ) = ( x, y, z );
        public Float3( Float3 other ) => ( x, y, z ) = ( other.x, other.y, other.z );

        public float SqrMagnitude => x * x + y * y + z * z;
        public float Magnitude => (float)Math.Sqrt( SqrMagnitude );

        public Float3 Normalized => this / Magnitude;

        public Float3 Floor => new Float3( (float)Math.Floor( x ), (float)Math.Floor( y ), (float)Math.Floor( z ) );
        public Float3 Ceil => new Float3( (float)Math.Ceiling( x ), (float)Math.Ceiling( y ), (float)Math.Ceiling( z ) );

        public Int3 FloorInt => new Int3( (int)Math.Floor( x ), (int)Math.Floor( y ), (int)Math.Floor( z ) );
        public Int3 CeilInt => new Int3( (int)Math.Ceiling( x ), (int)Math.Ceiling( y ), (int)Math.Ceiling( z ) );

        public bool Equals( Float3 other ) => x.Equals( other.x ) && y.Equals( other.y ) && z.Equals( other.z );

        public override bool Equals( object obj ) => obj is Float3 other && Equals( other );
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = x.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ y.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ z.GetHashCode();
                return hashCode;
            }
        }

        #region Operator overloading

        public static Float3 operator +( Float3 a, Float3 b ) => new Float3( a.x + b.x, a.y + b.y, a.z + b.z );
        public static Float3 operator -( Float3 a, Float3 b ) => new Float3( a.x - b.x, a.y - b.y, a.z - b.z );
        public static Float3 operator *( Float3 a, Float3 b ) => new Float3( a.x * b.x, a.y * b.y, a.z * b.z );
        public static Float3 operator /( Float3 a, Float3 b ) => new Float3( a.x / b.x, a.y / b.y, a.x / b.x );

        public static Float3 operator *( Float3 a, float value ) => new Float3( a.x * value, a.y * value, a.z * value );
        public static Float3 operator /( Float3 a, float value ) => new Float3( a.x / value, a.y / value, a.z / value );
        public static Float3 operator *( float value, Float3 a ) => a * value;
        public static Float3 operator /( float value, Float3 a ) => a / value;

        public static explicit operator Float3( float value ) => new Float3( value, value, value );
        public static explicit operator Float3( Float2 value ) => new Float3( value.x, value.y, 0f );

        #endregion

        #region Utility

        public float Dot( Float3 other ) => x * other.x + y * other.y + other.z * other.z;

        public Float3 Cross( Float3 other ) => new Float3( y * other.z - z * other.y, z * other.x - x * other.z, x * other.y - y * other.x );

        public string ToString( string format, IFormatProvider formatProvider ) =>
            "(" + x.ToString( format, formatProvider ) + ", " + y.ToString( format, formatProvider ) + ", " + z.ToString( format, formatProvider ) + ")";
        public override string ToString() => "(" + x + ", " + y + ", " + z + ")";
        public string ToString( string format ) => "(" + x.ToString( format ) + ", " + y.ToString( format ) + ", " + z.ToString( format ) + ")";

        #endregion
    }

    /// <summary>
    ///     represents two integers
    /// </summary>
    [Serializable]
    public readonly struct Int2 : IEquatable<Int2>, IFormattable
    {
        public readonly int x, y;

        public Int2( int x, int y ) => ( this.x, this.y ) = ( x, y );
        public Int2( Int2 other ) => ( x, y ) = ( other.x, other.y );

        public static Int2 MinValue => (Int2)int.MinValue;
        public static Int2 MaxValue => (Int2)int.MaxValue;

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
        public static Int2 operator *( int value, Int2 a ) => a * value;
        public static Int2 operator /( int value, Int2 a ) => a / value;

        public static explicit operator Int2( int value ) => new Int2( value, value );

        #endregion

        #region Utility

        public Int2 Min( Int2 other ) => new Int2( Math.Min( x, other.x ), Math.Min( y, other.y ) );
        public Int2 Max( Int2 other ) => new Int2( Math.Max( x, other.x ), Math.Max( y, other.y ) );

        #endregion
    }

    /// <summary>
    ///     represents three integers
    /// </summary>
    [Serializable]
    public readonly struct Int3 : IEquatable<Int3>, IFormattable
    {
        public readonly int x, y, z;

        public Int3( int x, int y, int z ) => ( this.x, this.y, this.z ) = ( x, y, z );
        public Int3( Int3 other ) => ( x, y, z ) = ( other.x, other.y, other.z );

        public static Int3 MinValue => (Int3)int.MinValue;
        public static Int3 MaxValue => (Int3)int.MaxValue;

        public static Int3 One => (Int3)1;

        public static Int3 Up => new Int3( 0, 1, 0 );
        public static Int3 Down => new Int3( 0, -1, 0 );
        public static Int3 Left => new Int3( -1, 0, 0 );
        public static Int3 Right => new Int3( 1, 0, 0 );
        public bool Equals( Int3 other ) => x == other.x && y == other.y && z == other.z;
        public override bool Equals( object obj ) => obj is Int3 other && Equals( other );
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = x.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ y.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ z.GetHashCode();
                return hashCode;
            }
        }

        #region Operator overloading

        public static Int3 operator +( Int3 a, Int3 b ) => new Int3( a.x + b.x, a.y + b.y, a.z + b.z );
        public static Int3 operator -( Int3 a, Int3 b ) => new Int3( a.x - b.x, a.y - b.y, a.z - b.z );
        public static Int3 operator *( Int3 a, Int3 b ) => new Int3( a.x * b.x, a.y * b.y, a.z * b.z );
        public static Int3 operator /( Int3 a, Int3 b ) => new Int3( a.x / b.x, a.y / b.y, a.x / b.x );

        public static Int3 operator *( Int3 a, int value ) => new Int3( a.x * value, a.y * value, a.z * value );
        public static Int3 operator /( Int3 a, int value ) => new Int3( a.x / value, a.y / value, a.z / value );
        public static Int3 operator *( int value, Int3 a ) => a * value;
        public static Int3 operator /( int value, Int3 a ) => a / value;

        public static explicit operator Int3( int value ) => new Int3( value, value, value );
        public static explicit operator Int3( Int2 value ) => new Int3( value.x, value.y, 0 );

        #endregion

        #region Utility

        public Int3 Min( Int3 other ) => new Int3( Math.Min( x, other.x ), Math.Min( y, other.y ), Math.Min( z, other.z ) );
        public Int3 Max( Int3 other ) => new Int3( Math.Max( x, other.x ), Math.Max( y, other.y ), Math.Max( z, other.z ) );

        public string ToString( string format, IFormatProvider formatProvider ) =>
            "(" + x.ToString( format, formatProvider ) + ", " + y.ToString( format, formatProvider ) + ", " + z.ToString( format, formatProvider ) + ")";
        public override string ToString() => "(" + x + ", " + y + ", " + z + ")";
        public string ToString( string format ) => "(" + x.ToString( format ) + ", " + y.ToString( format ) + ", " + z.ToString( format ) + ")";

        #endregion
    }
}
