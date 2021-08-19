using System;
using UnityEngine;

namespace CXUtils.Unity
{
    /// <summary>
    ///     A wrapper around for scene
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

