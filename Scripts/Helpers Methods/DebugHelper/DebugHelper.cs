using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;


namespace CXUtils.DebugHelper
{
    public interface IDebugDescribable
    {
        /// <summary> Describes an object </summary>
        string DebugDescribe();
    }

    /// <summary> A class full of helper function for debugging </summary>
    public struct DebugHelper
    {
        #region Vars Defines

        /// <summary> Modes for logging lists </summary>
        public enum LogListMode
        { oneLine, Multiline }

        #endregion

        #region Logs
        /// <summary> Logs a single message </summary>
        public static void Log(object sender, string msg) =>
            Dlog(sender, msg);

        /// <summary> Logs a list of objects using ToString </summary>
        public static void LogList<T>(object sender, T[] listT, LogListMode logListMode = LogListMode.oneLine, string between = ", ")
        {
            StringBuilder sb = new StringBuilder();
            int i;
            int listIndexMax = listT.Length - 1;

            switch (logListMode)
            {
                case LogListMode.oneLine:
                sb.Append($"Items({listT.Length}): ");

                for (i = 0; i < listIndexMax; i++)
                    sb.Append($"{listT[i].ToString()}{between}");

                sb.Append($"{listT[i].ToString()}");
                break;

                case LogListMode.Multiline:
                for (i = 0; i < listIndexMax; i++)
                    sb.Append($"\nItem {i} : {listT[i].ToString()}{between}");

                sb.Append($"\nItem {listIndexMax} : {listT[i].ToString()}");
                break;
            }

            Dlog(sender, sb.ToString());
        }

        /// <summary> Logs an Error </summary>
        public static void LogError(object sender, string msg) =>
            LogError<Exception>(sender, msg);

        /// <summary> Logs an Error </summary>
        public static void LogError<T>(object sender, string msg) where T : Exception, new()
        {
            Dlog(sender, msg);
            throw new T();
        }


        /// <summary> Logs the description for this object </summary>
        public static void LogDescription(object sender, IDebugDescribable debugDescribable) =>
            Log(sender, debugDescribable.DebugDescribe());
        #endregion

        //--------Script methods------------
        #region ScriptMethods
        static void Dlog(object sender, string msg) =>
            Debug.Log($"[{sender.ToString()}] {msg}");
        #endregion
    }
}

