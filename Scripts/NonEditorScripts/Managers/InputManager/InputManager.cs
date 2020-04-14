using CXUtils.Infos;
using UnityEngine;

namespace CXUtils.Manager
{
    /// <summary> Manages all the input </summary>
    public class InputManager : MonoBehaviour
    {
        #region Vars and Fields
        [SerializeField] private InputInfo[] inputInfos;
        
        public static InputManager Instance { get; private set; }
        public InputInfo[] InputInfos { get => inputInfos; set => inputInfos = value; }
        #endregion

        #region MainThread
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        #region ScriptMethods
        /// <summary> Finds an input info by name </summary>
        public InputInfo? FindInputInfoByName(string name)
        {
            foreach (var i in InputInfos)
                if (i.Name.Equals(name))
                    return i;

            return null;
        }
        #endregion
    }
}

