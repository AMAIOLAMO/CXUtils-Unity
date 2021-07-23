using System;

namespace CXUtils.UsefulTypes
{
    public interface ITypeFloat<T> : IEquatable<T>, IFormattable
    {
        /// <summary>
        ///     returns a new vector with a new magnitude of value
        /// </summary>
        T MagnitudeOf( float value );
    }
    
    /// <summary>
    ///     represents two floats
    /// </summary>
    [Serializable]
    public readonly struct Float2 : ITypeFloat<Float2>
    {
        public readonly float x, y;
        public Float2( float x, float y ) => ( this.x, this.y ) = ( x, y );
        public Float2( Float2 other ) => ( x, y ) = ( other.x, other.y );

        public static Float2 MinValue => (Float2)float.MinValue;
        public static Float2 MaxValue => (Float2)float.MaxValue;
        public static Float2 Epsilon => (Float2)float.Epsilon;

        public static Float2 Zero => (Float2)0f;
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

        public static Float2 operator -( Float2 a ) => new Float2( -a.x, -a.y );

        public static implicit operator Float2( Float3 value ) => new Float2( value.x, value.y );

        public static explicit operator Float2( float value ) => new Float2( value, value );
        public static explicit operator Float2( Int2 value ) => new Float2( value.x, value.y );

        #endregion

        #region Utility

        public float Dot( Float2 other ) => x * other.x + y * other.y;
        public Float2 Reflect( Float2 normal ) => this - 2f * Dot( normal ) * normal;

        public Float2 Min( Float2 other ) => new Float2( Math.Min( x, other.x ), Math.Min( y, other.y ) );
        public Float2 Max( Float2 other ) => new Float2( Math.Max( x, other.x ), Math.Max( y, other.y ) );

        /// <summary>
        ///     returns a new Float2 with a direction of this and a specified target magnitude
        /// </summary>
        public Float2 MagnitudeOf( float magnitude ) => Normalized * magnitude;

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
    public readonly struct Float3 : ITypeFloat<Float3>
    {
        public readonly float x, y, z;
        public Float3( float x, float y, float z ) => ( this.x, this.y, this.z ) = ( x, y, z );
        public Float3( Float3 other ) => ( x, y, z ) = ( other.x, other.y, other.z );

        public static Float3 MinValue => (Float3)float.MinValue;
        public static Float3 MaxValue => (Float3)float.MaxValue;
        public static Float3 Epsilon => (Float3)float.Epsilon;

        public static Float3 Zero => (Float3)0f;
        public static Float3 One => (Float3)1f;
        public static Float3 Half => (Float3).5f;
        public static Float3 Quarter => (Float3).25f;

        public static Float3 Up => new Float3( 0f, 1f, 0f );
        public static Float3 Down => new Float3( 0f, -1f, 0f );
        public static Float3 Left => new Float3( -1f, 0f, 0f );
        public static Float3 Right => new Float3( 1f, 0f, 0f );
        public static Float3 Forward => new Float3( 0f, 0f, 1f );
        public static Float3 Backward => new Float3( 0f, 0f, -1f );

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

        public static Float3 operator -( Float3 a ) => new Float3( -a.x, -a.y, -a.z );

        public static explicit operator Float3( float value ) => new Float3( value, value, value );
        public static explicit operator Float3( Float2 value ) => new Float3( value.x, value.y, 0f );
        public static explicit operator Float3( Int3 value ) => new Float3( value.x, value.y, value.z );

        #endregion

        #region Utility

        public float Dot( Float3 other ) => x * other.x + y * other.y + z * other.z;

        public Float3 Cross( Float3 other ) => new Float3( y * other.z - z * other.y, z * other.x - x * other.z, x * other.y - y * other.x );

        
        public Float3 MagnitudeOf( float magnitude ) => Normalized * magnitude;

        public string ToString( string format, IFormatProvider formatProvider ) =>
            "(" + x.ToString( format, formatProvider ) + ", " + y.ToString( format, formatProvider ) + ", " + z.ToString( format, formatProvider ) + ")";
        public override string ToString() => "(" + x + ", " + y + ", " + z + ")";
        public string ToString( string format ) => "(" + x.ToString( format ) + ", " + y.ToString( format ) + ", " + z.ToString( format ) + ")";

        #endregion
    }

    /// <summary>
    ///     represents four floats
    /// </summary>
    [Serializable]
    public readonly struct Float4 : ITypeFloat<Float4>
    {
        public readonly float x, y, z, w;
        public Float4( float x, float y, float z, float w ) => ( this.x, this.y, this.z, this.w ) = ( x, y, z, w );
        public Float4( Float4 other ) => ( x, y, z, w ) = ( other.x, other.y, other.z, other.w );

        public static Float4 MinValue => (Float4)float.MinValue;
        public static Float4 MaxValue => (Float4)float.MaxValue;
        public static Float4 Epsilon => (Float4)float.Epsilon;

        public static Float4 Zero => (Float4)0f;
        public static Float4 One => (Float4)1f;
        public static Float4 Half => (Float4).5f;
        public static Float4 Quarter => (Float4).25f;

        public static Float4 Up => new Float4( 0f, 1f, 0f, 0f );
        public static Float4 Down => new Float4( 0f, -1f, 0f, 0f );
        public static Float4 Left => new Float4( -1f, 0f, 0f, 0f );
        public static Float4 Right => new Float4( 1f, 0f, 0f, 0f );
        public static Float4 Forward => new Float4( 0f, 0f, 1f, 0f );
        public static Float4 Backward => new Float4( 0f, 0f, -1f, 0f );

        public float SqrMagnitude => x * x + y * y + z * z;
        public float Magnitude => (float)Math.Sqrt( SqrMagnitude );

        public Float4 Normalized => this / Magnitude;

        public Float4 Floor => new Float4( (float)Math.Floor( x ), (float)Math.Floor( y ), (float)Math.Floor( z ), (float)Math.Floor( w ) );
        public Float4 Ceil => new Float4( (float)Math.Ceiling( x ), (float)Math.Ceiling( y ), (float)Math.Ceiling( z ), (float)Math.Floor( w ) );

        public Int4 FloorInt => new Int4( (int)Math.Floor( x ), (int)Math.Floor( y ), (int)Math.Floor( z ), (int)Math.Floor( w ) );
        public Int4 CeilInt => new Int4( (int)Math.Ceiling( x ), (int)Math.Ceiling( y ), (int)Math.Ceiling( z ), (int)Math.Ceiling( w ) );

        public bool Equals( Float4 other ) => x.Equals( other.x ) && y.Equals( other.y ) && z.Equals( other.z );

        public override bool Equals( object obj ) => obj is Float3 other && Equals( other );

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = x.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ y.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ z.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ w.GetHashCode();
                return hashCode;
            }
        }

        #region Operator overloading

        public static Float4 operator +( Float4 a, Float4 b ) => new Float4( a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w );
        public static Float4 operator -( Float4 a, Float4 b ) => new Float4( a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w );
        public static Float4 operator *( Float4 a, Float4 b ) => new Float4( a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w );
        public static Float4 operator /( Float4 a, Float4 b ) => new Float4( a.x / b.x, a.y / b.y, a.x / b.x, a.w / b.w );
        public static Float4 operator *( Float4 a, float value ) => new Float4( a.x * value, a.y * value, a.z * value, a.w * value );
        public static Float4 operator /( Float4 a, float value ) => new Float4( a.x / value, a.y / value, a.z / value, a.w / value );
        public static Float4 operator *( float value, Float4 a ) => a * value;
        public static Float4 operator /( float value, Float4 a ) => a / value;
        public static Float4 operator -( Float4 a ) => new Float4( -a.x, -a.y, -a.z, -a.w );
        public static explicit operator Float4( float value ) => new Float4( value, value, value, value );
        public static explicit operator Float4( Float3 value ) => new Float4( value.x, value.y, value.z, 0f );

        #endregion

        #region Utility

        public float Dot( Float4 other ) => x * other.x + y * other.y + z * other.z + w * other.w;

        /// <summary>
        ///     returns a new Float3 with a direction of this and a specified target magnitude
        /// </summary>
        public Float4 MagnitudeOf( float magnitude ) => Normalized * magnitude;

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

        public static Int2 Zero => (Int2)0;
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
        public static Int2 operator %( Int2 a, int value ) => new Int2( a.x % value, a.y % value );
        public static Int2 operator *( int value, Int2 a ) => a * value;
        public static Int2 operator /( int value, Int2 a ) => a / value;
        public static Int2 operator -( Int2 a ) => new Int2( -a.x, -a.y );
        public static explicit operator Int2( Int3 value ) => new Int2( value.x, value.y );
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

        public static Int3 Zero => (Int3)0;
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
        public static Int3 operator /( Int3 a, Int3 b ) => new Int3( a.x / b.x, a.y / b.y, a.z / b.z );
        public static Int3 operator *( Int3 a, int value ) => new Int3( a.x * value, a.y * value, a.z * value );
        public static Int3 operator /( Int3 a, int value ) => new Int3( a.x / value, a.y / value, a.z / value );
        public static Int3 operator %( Int3 a, int value ) => new Int3( a.x % value, a.y % value, a.z % value );
        public static Int3 operator *( int value, Int3 a ) => a * value;
        public static Int3 operator /( int value, Int3 a ) => a / value;
        public static Int3 operator -( Int3 a ) => new Int3( -a.x, -a.y, -a.z );
        public static explicit operator Int3( Int2 value ) => new Int3( value.x, value.y, 0 );
        public static explicit operator Int3( int value ) => new Int3( value, value, value );

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

    /// <summary>
    ///     represents four integers
    /// </summary>
    [Serializable]
    public readonly struct Int4 : IEquatable<Int4>, IFormattable
    {
        public readonly int x, y, z, w;

        public Int4( int x, int y, int z, int w ) => ( this.x, this.y, this.z, this.w ) = ( x, y, z, w );
        public Int4( Int4 other ) => ( x, y, z, w ) = ( other.x, other.y, other.z, other.w );

        public static Int4 MinValue => (Int4)int.MinValue;
        public static Int4 MaxValue => (Int4)int.MaxValue;

        public static Int4 Zero => (Int4)0;
        public static Int4 One => (Int4)1;

        public static Int4 Up => new Int4( 0, 1, 0, 0 );
        public static Int4 Down => new Int4( 0, -1, 0, 0 );
        public static Int4 Left => new Int4( -1, 0, 0, 0 );
        public static Int4 Right => new Int4( 1, 0, 0, 0 );
        public bool Equals( Int4 other ) => x == other.x && y == other.y && z == other.z && w == other.w;
        public override bool Equals( object obj ) => obj is Int4 other && Equals( other );

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = x;
                hashCode = ( hashCode * 397 ) ^ y;
                hashCode = ( hashCode * 397 ) ^ z;
                hashCode = ( hashCode * 397 ) ^ w;
                return hashCode;
            }
        }

        #region Operator overloading

        public static Int4 operator +( Int4 a, Int4 b ) => new Int4( a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w );
        public static Int4 operator -( Int4 a, Int4 b ) => new Int4( a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w );
        public static Int4 operator *( Int4 a, Int4 b ) => new Int4( a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w );
        public static Int4 operator /( Int4 a, Int4 b ) => new Int4( a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w );
        public static Int4 operator *( Int4 a, int value ) => new Int4( a.x * value, a.y * value, a.z * value, a.w * value );
        public static Int4 operator /( Int4 a, int value ) => new Int4( a.x / value, a.y / value, a.z / value, a.w / value );
        public static Int4 operator %( Int4 a, int value ) => new Int4( a.x % value, a.y % value, a.z % value, a.w % value );
        public static Int4 operator *( int value, Int4 a ) => a * value;
        public static Int4 operator /( int value, Int4 a ) => a / value;
        public static Int4 operator -( Int4 a ) => new Int4( -a.x, -a.y, -a.z, -a.w );
        public static explicit operator Int4( Int2 value ) => new Int4( value.x, value.y, 0, 0 );
        public static explicit operator Int4( Int3 value ) => new Int4( value.x, value.y, value.z, 0 );
        public static explicit operator Int4( int value ) => new Int4( value, value, value, value );

        #endregion

        #region Utility

        public Int4 Min( Int4 other ) => new Int4( Math.Min( x, other.x ), Math.Min( y, other.y ), Math.Min( z, other.z ), Math.Min( w, other.w ) );
        public Int4 Max( Int4 other ) => new Int4( Math.Max( x, other.x ), Math.Max( y, other.y ), Math.Max( z, other.z ), Math.Min( w, other.w ) );

        public string ToString( string format, IFormatProvider formatProvider ) =>
            "(" + x.ToString( format, formatProvider ) + ", " + y.ToString( format, formatProvider ) + ", " +
            z.ToString( format, formatProvider ) + ", " + w.ToString( format, formatProvider ) + ")";
        public override string ToString() => "(" + x + ", " + y + ", " + z + ", " + w + ")";
        public string ToString( string format ) => "(" + x.ToString( format ) + ", " + y.ToString( format ) + ", " + z.ToString( format ) + ", " + w.ToString( format ) + ")";

        #endregion
    }
}
