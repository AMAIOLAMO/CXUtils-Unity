using UnityEngine;

namespace CXUtils.DataUtils.Infos
{
    /// <summary> The ultimate base class of all the scriptable infos for the scripts in CXUtils (Not in use)</summary>
    public class ScriptableInfo : ScriptableObject, IInfo
    {
        public string DebugDescribe() =>
            $"ScriptableInfo:\nName: {name}";

        /// <summary> Creates an instance of a scriptable info </summary>
        public new static ScriptableInfo CreateInstance(string className) =>
            (ScriptableInfo)ScriptableObject.CreateInstance(className);
    }
}