using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CXUtils.Unity
{
    /// <summary>
    ///     A wrapper around for <see cref="SceneAsset"/> that only can be used in editor
    /// </summary>
    [Serializable]
    public class SceneReference
    {
        [SerializeField] string _scenePath;
        [SerializeField] int _buildIndex;

        SceneReference() { }

        public string ScenePath => _scenePath;
        public int BuildIndex => _buildIndex;

        public static implicit operator string(SceneReference reference) => reference.ScenePath;
    }
}

