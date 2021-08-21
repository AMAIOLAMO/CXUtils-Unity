using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CXUtils.Unity
{
    /// <summary>
    ///     A bundle of <see cref="SceneReference"/>s that could be easily controlled and managed
    /// </summary>
    [Serializable]
    public class SceneBundle
    {
        [SerializeField] SceneReference[] _references;

        public SceneReference this[int index] => _references[index];

        public int Length => _references.Length;

        public void OverrideLoad()
        {
            AssertBundleLength();

            //load current to override everything
            SceneManager.LoadScene(_references[0]);

            for(int i = 1; i < _references.Length; ++i )
                SceneManager.LoadScene(_references[i], LoadSceneMode.Additive);
        }

        public void AdditiveLoad()
        {
            AssertBundleLength();

            for ( int i = 0; i < _references.Length; ++i )
                SceneManager.LoadScene(_references[i], LoadSceneMode.Additive);
        }

        public AsyncOperation[] OverrideLoadAsync()
        {
            AssertBundleLength();

            var operations = new AsyncOperation[_references.Length];

            //load current to override everything
            operations[0] = SceneManager.LoadSceneAsync(_references[0]);

            for ( int i = 1; i < _references.Length; ++i )
                operations[i] = SceneManager.LoadSceneAsync(_references[i], LoadSceneMode.Additive);

            return operations;
        }

        public AsyncOperation[] AdditiveLoadAsync()
        {
            AssertBundleLength();

            var operations = new AsyncOperation[_references.Length];

            for ( int i = 0; i < _references.Length; ++i )
                operations[i] = SceneManager.LoadSceneAsync(_references[i], LoadSceneMode.Additive);

            return operations;
        }

        void AssertBundleLength() => Debug.Assert(_references.Length == 0, nameof(_references) + " cannot load 0 scene references!");
    }
}

