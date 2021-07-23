using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    #region Enums

    /// <summary> Option flags for logging lists </summary>
    public enum LogListOptions
    {
        /// <summary> Logs on one line </summary>
        Single,
        /// <summary> Logs on multiple lines </summary>
        Multiple
    }

    #endregion

    /// <summary> A class full of helper function for debugging </summary>
    public static class DebugUtils
    {
        #region Logs

        /// <summary> Logs a single message </summary>
        public static void Log( object sender, object msg )
        {
            DLog( sender, msg );
        }

        /// <summary> Logs a list of objects using ToString </summary>
        public static void LogList<T>( object sender, T[] listT, LogListOptions logListMode = LogListOptions.Single, string between = ", " )
        {
            switch ( listT.Length )
            {
                case 0:
                    DLog( sender, $"Items(0): List: {nameof( listT )}'s length is 0" );
                    return;

                case 1:
                    switch ( logListMode )
                    {
                        case LogListOptions.Single:
                            DLog( sender,  $"Items({listT.Length}): {{{listT[0]}}}" );
                            return;

                        case LogListOptions.Multiple:
                            DLog( sender, $"Items({listT.Length}):\nItem 0 : {listT[0]}" );
                            return;

                        default:
                            throw ExceptionUtils.Error.NotAccessible;
                    }
            }

            var sb = new StringBuilder();

            int i;
            int listIndexMax = listT.Length - 1;

            switch ( logListMode )
            {
                case LogListOptions.Single:
                    sb.Append( $"Items({listT.Length}): " );

                    for ( i = 0; i < listIndexMax; i++ )
                        sb.Append( $"{listT[i]}{between}" );

                    sb.Append( $"{listT[i]}" );
                    break;

                case LogListOptions.Multiple:
                    for ( i = 0; i < listIndexMax; i++ )
                        sb.Append( $"\nItem {i} : {listT[i]}{between}" );

                    sb.Append( $"\nItem {listIndexMax} : {listT[i]}" );
                    break;
                
                default:
                    throw ExceptionUtils.Error.NotAccessible;
            }

            DLog( sender, sb.ToString() );
        }

        /// <summary> Logs an Error </summary>
        public static void LogError( in object sender, in string msg )
        {
            DLogError<Exception>( sender, msg );
        }

        /// <summary> Logs an Error </summary>
        public static void LogError<T>( in object sender, in string msg ) where T : Exception, new()
        {
            DLogError<T>( sender, msg );
        }

        public static void LogWarning( in object sender, in string msg )
        {
            DLogWarning( sender, msg );
        }

        #endregion

        #region Performance

        const int FPS_BUFFER_LEN = 60;
        static readonly int[] FPSBuffer = new int[FPS_BUFFER_LEN];
        static bool _fpsBufferFull;
        static int _fpsBufferIndex;

        /// <summary>
        ///     Recieves the avrg FPS :D
        /// </summary>
        public static int GetAvrgFPS()
        {
            FPSBuffer[_fpsBufferIndex++] = GetFPS();

            if ( _fpsBufferIndex >= FPS_BUFFER_LEN )
            {
                _fpsBufferIndex = 0;
                _fpsBufferFull = true;
            }

            int sum = 0;

            if ( !_fpsBufferFull )
            {
                for ( int i = 0; i <= _fpsBufferIndex; i++ )
                    sum += FPSBuffer[i];

                return sum / ( _fpsBufferIndex + 1 );
            }
            //else

            for ( int i = 0; i < FPS_BUFFER_LEN; i++ )
                sum += FPSBuffer[i];

            return sum / FPS_BUFFER_LEN;
        }

        /// <summary> Get's the current FPS (Frames per second) </summary>
        [Pure]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static int GetFPS()
        {
            return ( int )( 1f / Time.unscaledDeltaTime );
        }

        #endregion

        #region ScriptMethods

        #region Logs

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static void DLog( object sender, object msg )
        {
            Debug.Log( LogArgToString( sender, msg ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static void DLogError( object sender, object msg )
        {
            Debug.LogError( LogArgToString( sender, msg ) );
        }

        static void DLogError<T>( object sender, string msg ) where T : Exception, new()
        {
            DLogError( sender, msg );
            throw new T();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static void DLogWarning( object sender, string msg )
        {
            Debug.LogWarning( LogArgToString( sender, msg ) );
        }

        #endregion

        #region Helper Utils

        [Pure]
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static string LogArgToString( object sender, object msg )
        {
            return "[" + sender + "] " + msg;
        }

        #endregion

        #endregion
    }
}
