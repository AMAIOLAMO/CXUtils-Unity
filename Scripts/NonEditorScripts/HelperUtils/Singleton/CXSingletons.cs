using System;
using CXUtils.CodeUtils;
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

        private static bool _isApplicationQuitting = false;

        /// <summary>
        /// The instance of this class <br/>
        /// Simply a wrapper around <see cref="GetInstance"/>
        /// </summary>
        public static T Instance => GetInstance();

        /// <summary>
        /// Get's an instance of this singleton class
        /// </summary>
        public static T GetInstance()
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<T>();

            if (_instance != null)
                return _instance;
            
            //if didn't get any instance and is already quitting
            if (_isApplicationQuitting)
            {
                Debug.LogWarning("Application is already quitting and you are still accessing a singleton! " +
                                 "(If you want to clear something up, use OnDisable instead)");
                return null;
            }
            
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
        
            // hook to the quitting event with an application quit variable
            Application.quitting += OnApplicationQuit;
        }
        
        private void OnDestroy() =>
            Application.quitting -= OnApplicationQuit;

        private void OnApplicationQuit() =>
            _isApplicationQuitting = true;
    }

    /// <summary>
    /// A singleton that auto does the <see cref="MonoBehaviour.DontDestroyOnLoad"/> method for you
    /// </summary>
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