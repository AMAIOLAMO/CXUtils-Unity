using System;
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

    #region interfaces

    /// <summary> An interface that implements the debug describable for the debug helper </summary>
    public interface IDebugDescribable
    {
        /// <summary> Describes an object </summary>
        string DebugDescribe();
    }

    #endregion

    /// <summary> A class full of helper function for debugging </summary>
    public class DebugUtils
    {
        #region Logs

        /// <summary> Logs a single message </summary>
        public static void Log(object sender, object msg) =>
            DLog(sender, msg);

        /// <summary> Logs a list of objects using ToString </summary>
        public static void LogList<T>(object sender, T[] listT, LogListOptions logListMode = LogListOptions.Single, string between = ", ")
        {
            if ( listT.Length == 0 )
            {
                DLog(sender, $"Items(0): List: {nameof(listT)}'s length is 0");
                return;
            }

            if ( listT.Length == 1 )
                switch ( logListMode )
                {
                    case LogListOptions.Single:
                    DLog(sender, $"Items({listT.Length}): {listT[0]}");
                    return;

                    case LogListOptions.Multiple:
                    DLog(sender, $"Items({listT.Length}):\nItem 0 : {listT[0]}");
                    return;
                }

            StringBuilder sb = new StringBuilder();

            int i;
            int listIndexMax = listT.Length - 1;

            switch ( logListMode )
            {
                case LogListOptions.Single:
                sb.Append($"Items({listT.Length}): ");

                for ( i = 0; i < listIndexMax; i++ )
                    sb.Append($"{listT[i]}{between}");

                sb.Append($"{listT[i]}");
                break;

                case LogListOptions.Multiple:
                for ( i = 0; i < listIndexMax; i++ )
                    sb.Append($"\nItem {i} : {listT[i]}{between}");

                sb.Append($"\nItem {listIndexMax} : {listT[i]}");
                break;
            }

            DLog(sender, sb.ToString());
        }

        /// <summary> Logs an Error </summary>
        public static void LogError(object sender, string msg) => DLogError<Exception>(sender, msg);

        /// <summary> Logs an Error </summary>
        public static void LogError<T>(in object sender, in string msg) where T : Exception, new() => DLogError<T>(sender, msg);

        public static void LogWarning(in object sender, in string msg) => DLogWarning(sender, msg);
        
        /// <summary> Logs the description for this object </summary>
        public static void LogDescription(object sender, IDebugDescribable debugDescribable) => Log(sender, debugDescribable.DebugDescribe());

        #endregion

        #region Editor

        /// <summary> Runs a method only inside the editor </summary>
        public static bool RunInEditor(Action action)
        {
#if UNITY_EDITOR
            action?.Invoke();
            return true;
#endif
#pragma warning disable CS0162
            return false;
#pragma warning restore
        }

        /// <summary> Runs a method only inside the build </summary>
        public static bool RunInBuild(Action action)
        {
#if !UNITY_EDITOR
            action?.Invoke();
            return true;
#endif
#pragma warning disable CS0162
            return false;
#pragma warning restore
        }

        /// <summary> Runs two methods between Editor and build </summary>
        public static void RunInEditorOrBuild(Action editorAction, Action buildAction)
        {
#if UNITY_EDITOR
            editorAction?.Invoke();
#else
            buildAction?.Invoke();
#endif
        }

        #endregion

        #region Performance

        private const int FPS_BUFFER_LEN = 60;
        private static readonly int[] FPSBuffer = new int[FPS_BUFFER_LEN];
        private static bool _fpsBufferFull = false;
        private static int _fpsBufferIndex = 0;

        /// <summary>
        /// Recieves the avrg FPS :D
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

                return sum / (_fpsBufferIndex + 1);
            }
            //else

            for ( int i = 0; i < FPS_BUFFER_LEN; i++ )
                sum += FPSBuffer[i];

            return sum / FPS_BUFFER_LEN;
        }

        /// <summary> Get's the current FPS (Frames per second) </summary>
        public static int GetFPS() => (int)( 1f / Time.unscaledDeltaTime );

        #endregion

        #region ScriptMethods

        #region Logs

        private static void DLog(object sender, object msg) =>
            Debug.Log("[" + sender + "]" + msg);

        private static void DLogError(object sender, object msg) =>
            Debug.LogError(LogArgToString(sender, msg));

        private static void DLogError<T>(object sender, string msg) where T : Exception, new()
        {
            DLogError(sender, msg);
            throw new T();
        }

        private static void DLogWarning(object sender, string msg) =>
            Debug.LogWarning(LogArgToString(sender, msg));

        #endregion

        #region Helper Utils

        private static string LogArgToString(in object sender, in object msg) => "[" + sender + "]" + msg;

        #endregion

        #endregion
    }
}

