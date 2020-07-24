using UnityEngine;
using CXUtils.CodeUtils;

namespace CXUtils.DesignPatterns
{
    /// <summary> A helper class for making design patterns </summary>
    public class DesignPatternUtils : IBaseUtils
    {
        /// <summary>
        /// Creates a simple singleton manager.
        /// <para> this instance needs to be inherited from <see cref="Object"/>.</para>
        /// </summary>
        public static T SingletonManager<T>(T sender, T instance, bool dontDestroyOnLoad = true) where T : Object
        {
            instance = instance ?? sender;
            
            if(dontDestroyOnLoad)
                Object.DontDestroyOnLoad(sender);

            return instance;
        }
    }
}
