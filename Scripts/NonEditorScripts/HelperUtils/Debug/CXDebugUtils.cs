﻿using System;
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
            Dlog(sender, msg);

        /// <summary> Logs a list of objects using ToString </summary>
        public static void LogList<T>(object sender, T[] listT, LogListOptions logListMode = LogListOptions.Single, string between = ", ")
        {
            if (listT.Length == 0)
            {
                Dlog(sender, $"Items(0): List: {nameof(listT)}'s length is 0");
                return;
            }

            if (listT.Length == 1)
                switch (logListMode)
                {
                    case LogListOptions.Single:
                        Dlog(sender, $"Items({listT.Length}): {listT[0]}");
                        return;

                    case LogListOptions.Multiple:
                        Dlog(sender, $"Items({listT.Length}):\nItem 0 : {listT[0]}");
                        return;
                }

            StringBuilder sb = new StringBuilder();

            int i;
            int listIndexMax = listT.Length - 1;

            switch (logListMode)
            {
                case LogListOptions.Single:
                    sb.Append($"Items({listT.Length}): ");

                    for (i = 0; i < listIndexMax; i++)
                        sb.Append($"{listT[i]}{between}");

                    sb.Append($"{listT[i]}");
                    break;

                case LogListOptions.Multiple:
                    for (i = 0; i < listIndexMax; i++)
                        sb.Append($"\nItem {i} : {listT[i]}{between}");

                    sb.Append($"\nItem {listIndexMax} : {listT[i]}");
                    break;
            }

            Dlog(sender, sb.ToString());
        }

        /// <summary> Logs an Error </summary>
        public static void LogError(object sender, string msg) => DlogError<Exception>(sender, msg);

        /// <summary> Logs an Error </summary>
        public static void LogError<T>(object sender, string msg) where T : Exception, new() => DlogError<T>(sender, msg);

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
            return false;
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

        /// <summary> Get's the current FPS (Frames per second) </summary>
        public static float GetFPS() => GetCurrentFPS();

        #endregion

        #region ScriptMethods

        #region Logs

        static void Dlog(object sender, object msg) =>
            Debug.Log($"[{sender}] {msg}");

        static void DlogError(object sender, object msg) =>
            Debug.LogError($"[{sender}] {msg}");

        static void DlogError<T>(object sender, string msg) where T : Exception, new()
        {
            DlogError(sender, msg);
            throw new T();
        }

        #endregion

        #region Performance

        static float deltaTime = 0f;

        private static float GetCurrentFPS()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            return 1f / deltaTime;
        }

        #endregion

        #endregion
    }
}
