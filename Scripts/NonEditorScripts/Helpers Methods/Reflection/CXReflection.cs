using System;
using System.Reflection;
using System.Linq.Expressions;

namespace CXUtils.CodeUtils
{
    ///<summary> A simple reflection helper </summary>
    public class ReflectionUtils
    {
        #region GetMethodInfos

        ///<summary> Get's a method according to the method that is given </summary>
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> exp)
        {
            var memb = exp.Body as MethodCallExpression;

            if(memb.Equals(null))
                throw new ArgumentException("Exp is not a method!","exp");

            return memb.Method;
        }

        #region Action<>
        
        public static MethodInfo GetMethodInfo(Action action) =>
            action.GetMethodInfo();
        
        public static MethodInfo GetMethodInfo<T>(Action<T> action) =>
            action.GetMethodInfo();

        public static MethodInfo GetMethodInfo<T1, T2>(Action<T1, T2> action) =>
            action.GetMethodInfo();
        
        public static MethodInfo GetMethodInfo<T1, T2, T3>(Action<T1, T2, T3> action) =>
            action.GetMethodInfo();
            
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) =>
            action.GetMethodInfo();

        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) =>
            action.GetMethodInfo();
            
        #endregion
        
        #endregion
    }
}