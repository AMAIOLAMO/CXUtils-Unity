using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary>
    ///     CX Exception error types
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        ///     Error type: The code logic is not accessible here.
        /// </summary>
        NotAccessible
    }

    /// <summary>
    ///     CX Exception invalid type
    /// </summary>
    public enum InvalidType
    {
        /// <summary>
        ///     Invalid type: The value is invalid.
        /// </summary>
        ValueInvalid, ValueOutOfRange
    }

    /// <summary> CX's Exception Utils, good for handeling Exceptions </summary>
    public static class ExceptionUtils
    {
        //Error
        const string ERROR_MSG_NOT_ACCESSIBLE = "The code here is been accessed but it should not be accessed!";

        //Invalid
        const string INVALID_MSG_VALUE_INVALID = "The value that is been modified is Invalid!",
            INVALID_MSG_VALUE_OUT_OF_RANGE = "The value is out of range!";

        public static class Invalid
        {
            public static Exception InvalidValue => new Exception( INVALID_MSG_VALUE_INVALID );
            public static Exception ValueOutOfRange => new Exception( INVALID_MSG_VALUE_OUT_OF_RANGE );
        }

        public static class Error
        {
            public static Exception NotAccessible => new Exception( ERROR_MSG_NOT_ACCESSIBLE );
        }
    }
}
