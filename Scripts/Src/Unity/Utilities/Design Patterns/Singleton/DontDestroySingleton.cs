using UnityEngine;

namespace CXUtils.Unity.DesignPatterns
{
    /// <summary>
    ///     A singleton that auto does the <see cref="MonoBehaviour.DontDestroyOnLoad" /> method for you
    /// </summary>
    public abstract class DontDestroySingleton<T> : Singleton<T> where T : Component
    {
        protected override void Awake()
        {
            if ( instance == null )
            {
                instance = this as T;
                DontDestroyOnLoad( gameObject );
            }
            else if ( instance != this as T )
            {
                Destroy( gameObject );
            }
            else
            {
                DontDestroyOnLoad( gameObject );
            }
        }
    }
}
