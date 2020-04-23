using System.Text;
using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    #region Enums
    /// <summary> Option flags for logging lists </summary>
    public enum LogListOptions
    { oneLine, Multiline }
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
    public struct DebugUtils
    {
        #region Logs
        /// <summary> Logs a single message </summary>
        public static void Log(object sender, object msg) =>
            Dlog(sender, msg);

        /// <summary> Logs a list of objects using ToString </summary>
        public static void LogList<T>(object sender, T[] listT, LogListOptions logListMode = LogListOptions.oneLine, string between = ", ")
        {
            if (listT.Length == 0)
                return;

            StringBuilder sb = new StringBuilder();
            int i;
            int listIndexMax = listT.Length - 1;

            switch (logListMode)
            {
                case LogListOptions.oneLine:
                sb.Append($"Items({listT.Length}): ");

                for (i = 0; i < listIndexMax; i++)
                    sb.Append($"{listT[i].ToString()}{between}");

                sb.Append($"{listT[i].ToString()}");
                break;

                case LogListOptions.Multiline:
                for (i = 0; i < listIndexMax; i++)
                    sb.Append($"\nItem {i} : {listT[i].ToString()}{between}");

                sb.Append($"\nItem {listIndexMax} : {listT[i].ToString()}");
                break;
            }

            Dlog(sender, sb.ToString());
        }

        /// <summary> Logs an Error </summary>
        public static void LogError(object sender, string msg) =>
            DlogError<Exception>(sender, msg);

        /// <summary> Logs an Error </summary>
        public static void LogError<T>(object sender, string msg) where T : Exception, new() =>
            DlogError<T>(sender, msg);

        /// <summary> Logs the description for this object </summary>
        public static void LogDescription(object sender, IDebugDescribable debugDescribable) =>
            Log(sender, debugDescribable.DebugDescribe());
        #endregion

        #region ScriptMethods
        static void Dlog(object sender, object msg) =>
            Debug.Log($"[{sender}] {msg.ToString()}");


        static void DlogError<T>(object sender, string msg) where T : Exception, new()
        {
            Dlog(sender, msg);
            throw new T();
        }
        #endregion
    }
}

