using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary> CX's Exception Utils, good for handeling Exceptions </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Throws an exception and error inside of unity
        /// </summary>
        public static void ThrowInUnity<TException>(this TException ex, object errorMsg, bool onUnity = true) where TException : Exception, new()
        {
            if (onUnity) Debug.LogError(errorMsg);

            throw ex;
        }

        /// <inheritdoc cref="ThrowInUnity{TException}(TException, object, bool)"/>
        public static void ThrowInUnity<TException>(this TException ex, string objName, object errorMsg, bool onUnity = true) where TException : Exception, new() =>
            ex.ThrowInUnity($"{objName}: {errorMsg}", onUnity);
    }
}
