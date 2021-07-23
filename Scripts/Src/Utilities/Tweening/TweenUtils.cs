﻿using System;

namespace CXUtils.CodeUtils
{
    /// <summary>
    ///     A basic tweening library
    /// </summary>
    public static class TweenUtils
    {
        /// <summary>
        ///     linear interpolates between <paramref name="a" /> and <paramref name="b" /> using <paramref name="t" /> <br />
        ///     NOTE: This is not clamped
        /// </summary>
        /// <param name="a">initial value</param>
        /// <param name="b">destination value</param>
        /// <param name="t">the percentage that calculates between <paramref name="a" /> and <paramref name="b" /></param>
        public static float Lerp( float a, float b, float t ) => ( b - a ) * t + a;

        public static float LerpClamp( float a, float b, float t ) => Lerp( a, b, MathUtils.Clamp01( t ) );

        #region Tweens

        public static float EaseInSine( float t ) => 1f - (float)Math.Cos( t * MathUtils.PI * .5f );
        public static float EaseOutSine( float t ) => (float)Math.Sin( t * MathUtils.PI * .5f );
        public static float EaseInOutSine( float t ) => -( (float)Math.Cos( MathUtils.PI * t ) - 1f ) * .5f;

        public static float EaseInQuad( float t ) => t * t;
        public static float EaseOutQuad( float t ) => 1f - ( 1f - t ) * ( 1f - t );
        public static float EaseInOutQuad( float t ) => t < .5f ? 2f * t * t : 1f - (float)Math.Pow( -2f * t + 2f, 2f ) * .5f;

        public static float EaseInCubic( float t ) => t * t * t;
        public static float EaseOunCubic( float t ) => 1 - ( 1f - t ) * ( 1f - t ) * ( 1f - t );
        public static float EaseInOunCubic( float t ) => t < .5f ? 4f * t * t * t : 1f - (float)Math.Pow( -2f * t + 2f, 3f ) * .5f;

        public static float EaseInExpo( float t ) => t == 0f ? 0f : (float)Math.Pow( 2f, 10f * t - 10f );
        public static float EaseOutExpo( float t ) => t == 1f ? 1f : 1f - (float)Math.Pow( 2f, -10f * t );
        public static float EaseInOutExpo( float t ) => t == 0
            ? 0f
            : t == 1f
                ? 1f
                : t < .5f
                    ? (float)Math.Pow( 2f, 20f * t - 10f ) / 2f
                    : ( 2f - (float)Math.Pow( 2f, -20f * t + 10f ) ) / 2f;

        public static float EaseInCirc( float t ) => 1f - (float)Math.Sqrt( 1.0 - Math.Pow( t, 2.0 ) );
        public static float EaseOutCirc( float t ) => (float)Math.Sqrt( 1.0 - Math.Pow( t - 1f, 2.0 ) );
        public static float EaseInOutCirc( float t ) => t < .5f
            ? EaseInCirc( t ) * .5f
            : ( EaseOutCirc( t ) + 1f ) * .5f;

        const float BACK_C1 = 1.70158f;
        const float BACK_C2 = BACK_C1 * 1.525f;
        const float BACK_C3 = BACK_C1 + 1f;

        public static float EaseInBack( float t ) => BACK_C3 * t * t * t - BACK_C1 * t * t;
        public static float EaseOutBack( float t ) => 1f + BACK_C3 * (float)Math.Pow( t - 1f, 3f ) + BACK_C1 * (float)Math.Pow( t - 1f, 2f );
        public static float EaseInOutBack( float t ) => t < .5f
            ? (float)Math.Pow( 2f * t, 2f ) * ( ( BACK_C2 + 1f ) * 2f * t - BACK_C2 ) * .5f
            : ( (float)Math.Pow( 2f * t - 2f, 2f ) * ( ( BACK_C2 + 1f ) * ( t * 2f - 2f ) + BACK_C2 ) + 2f ) * .5f;

        const float ELASTIC_C1 = 2f * MathUtils.PI / 3f;
        const float ELASTIC_C2 = 2f * MathUtils.PI / 4.5f;

        public static float EaseInElastic( float t ) => t == 0f
            ? 0f
            : t == 1f
                ? 1f
                : (float)( -Math.Pow( 2.0, 10.0 * t - 10.0 ) * Math.Sin( ( t * 10.0 - 10.75 ) * ELASTIC_C1 ) );
        public static float EaseOutElastic( float t ) => t == 0f
            ? 0f
            : t == 1f
                ? 1f
                : (float)( Math.Pow( 2.0, -10.0 * t ) * Math.Sin( ( t * 10.0 - .75 ) * ELASTIC_C1 ) + 1.0 );
        public static float EaseInOutElastic( float t ) => t == 0f
            ? 0f
            : t == 1f
                ? 1f
                : t < .5f
                    ? (float)( -( Math.Pow( 2.0, 20.0 * t - 10.0 ) * Math.Sin( ( 20.0 * t - 11.125 ) * ELASTIC_C2 ) ) * .5 )
                    : (float)( Math.Pow( 2.0, -20.0 * t + 10.0 ) * Math.Sin( ( 20.0 * t - 11.125 ) * ELASTIC_C2 ) / 2.0 + 1.0 );

        const float BOUNCE_B1 = 7.5625f,
            BOUNCE_D1 = 2.75f;

        public static float EaseInBounce( float t ) => 1f - EaseOutBounce( 1f - t );
        public static float EaseOutBounce( float t )
        {
            if ( t < 1f / BOUNCE_D1 ) return BOUNCE_B1 * t * t;
            if ( t < 2f / BOUNCE_D1 ) return BOUNCE_B1 * ( t -= 1.5f / BOUNCE_D1 ) * t + 0.75f;
            if ( t < 2.5f / BOUNCE_D1 ) return BOUNCE_B1 * ( t -= 2.25f / BOUNCE_D1 ) * t + 0.9375f;

            return BOUNCE_B1 * ( t -= 2.625f / BOUNCE_D1 ) * t + 0.984375f;
        }
        public static float EaseInOutBounce( float t ) => t < .5f
            ? ( 1f - EaseOutBounce( 1f - 2f * t ) ) / 2f
            : ( 1f + EaseOutBounce( 2f * t - 1f ) ) / 2f;

        #endregion
    }
}