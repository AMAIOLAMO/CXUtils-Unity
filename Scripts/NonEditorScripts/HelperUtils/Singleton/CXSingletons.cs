using UnityEngine;

namespace CXUtils.DesignPatterns
{
    /// <summary>
    /// A base singleton class that you can inherit to control from <br/>
    /// This class doesn't Do the <see cref="MonoBehaviour.DontDestroyOnLoad"/> method
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;

        /// <summary>
        /// The instance of this class
        /// </summary>
        public static T Instance => GetInstance();

        /// <summary>
        /// Get's an instance of this singleton class
        /// </summary>
        public static T GetInstance()
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<T>();

            //if can find it now, then return
            if (_instance != null)
                return _instance;
            
            //else just create it
            var obj = new GameObject { name = typeof(T).Name };
            
            _instance = obj.AddComponent<T>();

            return _instance;
        }

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            
            else if (_instance != this as T)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// A singeton that auto does the <see cref="MonoBehaviour.DontDestroyOnLoad"/> method for you
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DontDestroySingleton<T> : Singleton<T> where T : Component
    {
        protected override void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this as T)
                Destroy(gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }
    }
}