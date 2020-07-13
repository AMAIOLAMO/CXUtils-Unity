using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary> CX's Exception Utils, good for handeling Exceptions </summary>
    public class CXExceptionUtils
    {
        /// <summary> Throws an exception and error </summary>
        public static void ThrowException<TException>(object errorMsg, bool onUnity = true)
            where TException : SystemException, new()
        {
            if (onUnity)
                Debug.LogError(errorMsg);
            
            throw new TException();
        }
    }
}
