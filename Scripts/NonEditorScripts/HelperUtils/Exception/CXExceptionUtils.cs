using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace CXUtils.CodeUtils
{
    /// <summary> CX's Exception Utils, good for handeling Exceptions </summary>
    public class ExceptionUtils : IBaseUtils
    {
        /// <summary> Throws an exception and error </summary>
        public static void ThrowException<TException>(object errorMsg, bool onUnity = true)
            where TException : Exception, new()
        {
            if (onUnity)
                Debug.LogError(errorMsg);

            throw new TException();
        }

        /// <summary> Throws an exception and error </summary>
        public static void ThrowException<TException>(string objName, object errorMsg, bool onUnity = true)
            where TException : Exception, new() =>
            ThrowException<TException>($"{objName}: {errorMsg}", onUnity);
    }
}
