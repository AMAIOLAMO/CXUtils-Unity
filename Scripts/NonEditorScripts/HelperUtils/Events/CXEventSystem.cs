using System;
using UnityEngine;
using System.Collections.Generic;

namespace CXUtils.HelperComponents
{
    /// <summary> A helper component to help for event handeling </summary>
    public class CXEventSystem : MonoBehaviour
    {
        public static CXEventSystem Current { get; private set; }

        /// <summary> actions for things to do </summary>
        public Action<string, object> EventActions;
        private Dictionary<string, object> Events;

        private void Awake()
        {
            Current = this;
        }
    }
}