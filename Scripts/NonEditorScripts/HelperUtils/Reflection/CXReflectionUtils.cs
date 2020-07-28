using System;
using System.Reflection;
using System.Linq.Expressions;

namespace CXUtils.CodeUtils
{
    ///<summary> A simple reflection helper </summary>
    public class ReflectionUtils
    {
        #region GetInfos

        #region MethodInfo

        ///<summary> Get's a method according to the method that is given </summary>
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> exp) => GetMethodInfo<Action<T>>(exp);

        public static MethodInfo GetMethodInfo<T>(Expression<T> exp) where T : Delegate
        {
            var memb = exp.Body as MethodCallExpression;

            if (memb.Equals(null)) new ArgumentException().ThrowInUnity("The given expression is not a method!");

            return memb.Method;
        }

        #endregion

        #endregion
    }
}