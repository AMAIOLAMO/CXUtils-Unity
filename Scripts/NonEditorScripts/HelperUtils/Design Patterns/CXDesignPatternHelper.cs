using UnityEngine;
using CXUtils.CodeUtils;

namespace CXUtils.DesignPatterns
{
    /// <summary> A helper class for making design patterns </summary>
    public class DesignPatternUtils : IBaseUtils
    {
        /// <summary> Generates a simple singleton instance
        /// <para>QUICK NOTE: the given type should be inherited by <seealso cref="Object"/></para></summary>
        public static T GenerateSingleton<T>(T sender, T instance, bool dontDestroyOnLoad = false) where T : Object
        {
            if(instance == null)
            {
                instance = sender;

                if(dontDestroyOnLoad)
                    Object.DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }
}
