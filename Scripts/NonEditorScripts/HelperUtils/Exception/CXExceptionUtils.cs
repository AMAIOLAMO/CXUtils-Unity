using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary>
    /// CX Exception error types
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// Error type: The code logic is not accessible here.
        /// </summary>
        NotAccessible
    }

    /// <summary>
    /// CX Exception invalid type
    /// </summary>
    public enum InvalidType
    {
        /// <summary>
        /// Invalid type: The value is invalid.
        /// </summary>
        ValueInvalid
    }

    /// <summary> CX's Exception Utils, good for handeling Exceptions </summary>
    public static class ExceptionUtils
    {
        //Error
        const string Error_NotAccessible = "Error: The code here is been accessed but it should not be accessed!";

        //Invalid
        const string Invalid_ValueInvalid = "Invalid: The value that is been modified is Invalid!";

        /// <summary>
        /// Throws an error using the error type
        /// </summary>
        public static Exception GetException(ErrorType errorType, Exception innerException = null) => new Exception(GetErrorMsg(errorType), innerException);

        ///<inheritdoc cref="GetException(ErrorType, Exception)"/>
        public static Exception GetException(InvalidType invalidType, Exception innerException = null) => new Exception(GetInvalidMsg(invalidType), innerException);

        /// <summary>
        /// Throws an exception and error inside of unity
        /// <para><paramref name="onUnity"/> for logging error to unity using <see cref="Debug.LogError(object)"/></para>
        /// </summary>
        public static void ThrowInUnity<TException>(this TException ex, object errorMsg, bool onUnity = true) where TException : Exception, new()
        {
            if (onUnity) Debug.LogError(errorMsg);

            throw ex;
        }

        /// <inheritdoc cref="ThrowInUnity{TException}(TException, object, bool)"/>
        public static void ThrowInUnity<TException>(this TException ex, string objName, object errorMsg, bool onUnity = true) where TException : Exception, new() =>
            ex.ThrowInUnity($"{objName}: {errorMsg}", onUnity);

        #region Simplifying

        static string GetInvalidMsg(InvalidType invalidType)
        {
            switch (invalidType)
            {
                case InvalidType.ValueInvalid: return Invalid_ValueInvalid;
                default: throw GetException(ErrorType.NotAccessible);
            }
        }

        static string GetErrorMsg(ErrorType errorType)
        {
            switch (errorType)
            {
                case ErrorType.NotAccessible: return Error_NotAccessible;
                default: throw GetException(ErrorType.NotAccessible);
            }
        }

        #endregion
    }
}
