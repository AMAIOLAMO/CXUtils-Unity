using UnityEngine;

namespace CXUtils.DesignPatterns
{
    /// <summary> A helper class for making design patterns </summary>
    public class DesignPatternUtils
    {
        /// <summary>
        /// Creates a simple singleton manager.
        /// <para> this instance needs to be a Unity <seealso cref="Object"/>.</para>
        /// </summary>
        public static T SingletonManager<T>(T sender, T instance, bool dontDestroyOnLoad = true) where T : Object
        {
            instance = instance ?? sender;

            if ( dontDestroyOnLoad )
                Object.DontDestroyOnLoad(sender);

            return instance;
        }
    }

    /// <summary>
    /// A interface that implements a singleton
    /// </summary>
    public interface ISingleton<T> where T : class, new()
    {
        /// <summary>
        /// An instance of this singleton.
        /// <para>QUICK NOTE: will create a new instance if there is no instance created before</para>
        /// </summary>
        T Instance { get; set; }

        /// <summary>
        /// Get's a new instance of this singleton
        /// </summary>
        T GetNewInstance();
    }
}
