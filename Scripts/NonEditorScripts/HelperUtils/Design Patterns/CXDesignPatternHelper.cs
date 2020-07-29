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
            
            if(dontDestroyOnLoad) Object.DontDestroyOnLoad(sender);

            return instance;
        }
    }

    /// <summary>
    /// An implementable singleton base class (Thread safe, using <seealso cref="lockObj"/>)
    /// </summary>
    public abstract class SingletonBase<T> where T : class, new()
    {
        private static readonly object lockObj = new object(); //NOTE this object is for locking the threads when multiple commands construct the singleton at the same time :D

        /// <summary>
        /// An instance of this singleton.
        /// <para>QUICK NOTE: will create a new instance if there is no instance created before</para>
        /// </summary>
        public static T Instance {
            get
            {
                lock(lockObj)
                    if (instance == null) instance = GetNewInstance();

                return instance;
            }
            protected set => instance = value; 
        }

        protected static T instance;

        /// <summary>
        /// Get's a new instance of this singleton
        /// </summary>
        protected static T GetNewInstance() => new T();
    }
}
