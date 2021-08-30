using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace CXUtils.Common
{
    #region Enums

    /// <summary> Option flags for logging lists </summary>
    public enum LogListType
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
        #region Performance

        /// <summary>
        ///     Get's the current FPS (Frames per second)
        /// </summary>
        [Pure]
        public static int GetFPS(float deltaTime) => (int)( 1f / deltaTime );

        #endregion
        #region Logs

        /// <summary> Logs a single message </summary>
        public static void Log( object sender, object msg )
        {
            DLog( sender, msg );
        }

        /// <summary>
        ///     Logs a list of objects using ToString
        /// </summary>
        public static void LogList<T>( object sender, T[] listT, LogListType logListMode = LogListType.Single, string between = ", " )
        {
            switch ( listT.Length )
            {
                case 0:
                    DLog( sender, $"Items(0): List: {nameof( listT )}'s length is 0" );
                    return;

                case 1:
                    switch ( logListMode )
                    {
                        case LogListType.Single:
                            DLog( sender, $"Items({listT.Length}): {{{listT[0]}}}" );
                            return;

                        case LogListType.Multiple:
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
                case LogListType.Single:
                    sb.Append( $"Items({listT.Length}): " );

                    for ( i = 0; i < listIndexMax; i++ )
                        sb.Append( $"{listT[i]}{between}" );

                    sb.Append( $"{listT[i]}" );
                    break;

                case LogListType.Multiple:
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
        static string LogArgToString( object sender, object msg ) => "[" + sender + "] " + msg;

        #endregion

        #endregion
    }
}
