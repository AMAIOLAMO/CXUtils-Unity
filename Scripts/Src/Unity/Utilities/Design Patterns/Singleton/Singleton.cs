using UnityEngine;

namespace CXUtils.Unity.DesignPatterns
{
    /// <summary>
    ///     A base singleton class that you can inherit to control from <br />
    ///     This class doesn't Do the <see cref="MonoBehaviour.DontDestroyOnLoad" /> method
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;

        static bool _isApplicationQuitting;

        /// <summary>
        ///     The instance of this class <br />
        ///     Simply a wrapper around <see cref="GetInstance" />
        /// </summary>
        public static T Instance => GetInstance();
        
        protected virtual void Awake()
        {
            if ( instance == null )
                instance = this as T;

            else if ( instance != this as T )
                Destroy( gameObject );

            // hook to the quitting event with an application quit variable
            Application.quitting += OnApplicationQuit;
        }

        protected virtual void OnInstanceInit() { }

        protected virtual void OnDestroy()
        {
            Application.quitting -= OnApplicationQuit;
        }

        void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
        }

        /// <summary>
        ///     Get's an instance of this singleton class
        /// </summary>
        public static T GetInstance()
        {
            if ( instance != null ) return instance;

            instance = FindObjectOfType<T>();

            if ( instance != null )
                return instance;

            //if didn't get any instance and is already quitting
            if ( _isApplicationQuitting )
            {
                Debug.LogWarning( "Application is already quitting and you are still accessing a singleton! " +
                                  "(If you want to clear something up, use OnDisable instead)" );
                return null;
            }

            //else just create it
            var obj = new GameObject { name = typeof( T ).Name };

            instance = obj.AddComponent<T>();

            return instance;
        }
    }
}
