using UnityEngine;
using System;
using CXUtils.CodeUtils;

namespace CXUtils.DataUtils.Infos
{
    /// <summary> A single input info that stores an input </summary>
    [Serializable]
    public struct InputInfo : IDebugDescribable
    {
        #region Vars and fields
        [SerializeField] private string name;
        [SerializeField] private KeyCode key;

        public string Name { get => name; set => name = value; }
        public KeyCode Key { get => key; set => key = value; }
        #endregion

        //Constructor
        public InputInfo(string name = "__None__", KeyCode key = KeyCode.None) =>
            (this.name, this.key) = (name, key);

        //IDebug Describbable    
        public string DebugDescribe() =>
            $"InputInfo: Name: {Name}, KeyCode: {Key}";


        //Script Methods

        public bool Equals(InputInfo inputInfo) =>
            (name.Equals(inputInfo.Name) && key == inputInfo.Key);
    }
}

