using System;
using UnityEngine;
using System.Collections.Generic;

namespace CXUtils.DataUtils.Infos
{
    /// <summary> A single input info that stores an input </summary>
    [Serializable]
    public class InputInfo : InfoBase
    {
        #region Vars

        //Exposed
        public KeyValuePair<string, KeyCode> NameKeyPair { get => nameKeyPair; set => nameKeyPair = value; }

        //Private
        [SerializeField] private KeyValuePair<string, KeyCode> nameKeyPair;

        #endregion

        //Constructor
        public InputInfo(KeyValuePair<string, KeyCode> nameKeyPair) =>
            NameKeyPair = nameKeyPair;

        public InputInfo(string name = "__None__", KeyCode key = KeyCode.None) =>
            NameKeyPair = new KeyValuePair<string, KeyCode>(name, key);

        #region Script Methods

        public new string DebugDescribe() =>
            $"InputInfo: Name: {NameKeyPair.Key}, KeyCode: {NameKeyPair.Value}";

        public new string ToString() =>
            "Input Info";

        public bool Equals(InputInfo inputInfo) =>
            (NameKeyPair.Key.Equals(inputInfo.NameKeyPair.Key) && NameKeyPair.Value == inputInfo.NameKeyPair.Value);

        #endregion
    }
}

