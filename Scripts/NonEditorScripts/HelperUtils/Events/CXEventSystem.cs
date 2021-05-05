using System;
using UnityEngine;
using System.Collections.Generic;
using CXUtils.DesignPatterns;

namespace CXUtils.HelperComponents
{
    /// <summary> A helper component to help for event handeling </summary>
    public class CXEventSystem : Singleton<CXEventSystem>
    {
        /// <summary> actions for things to do </summary>
        public Action<string, object> EventActions;
        private Dictionary<string, object> Events;
    }
}