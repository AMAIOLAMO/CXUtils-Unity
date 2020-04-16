using System;
using UnityEngine;
using UnityEditor;

namespace CXUtils.HelperAttributes
{
    /// <summary> Creates a new Label field on top of current field (not implemented) </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class LabelAttribute : MultiPropertyAttribute
    {
        public LabelAttribute() =>
            throw new NotImplementedException();
    }
}

