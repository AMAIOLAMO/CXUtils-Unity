using CXUtils.DebugHelper;
using System;
using UnityEngine;

namespace CXUtils.HelperAttributes
{
    /// <summary> Shows a field class or property in the inspector window </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class ShowInInspectorAttribute : PropertyAttribute
    {
    }

    /// <summary> Overrides the current label </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class OverrideLabelAttribute : PropertyAttribute
    {
        public string _labelTxt { get; set; } = default;

        public OverrideLabelAttribute(string labelTxt = null) =>
            _labelTxt = labelTxt;
    }

    /// <summary> Overrides the color of this object </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class LabelColorAttribute : PropertyAttribute
    {
        public EnumAttributeColor _labelContentColor { get; set; } = EnumAttributeColor.black;
        public EnumAttributeColor _labelBGColor { get; set; } = EnumAttributeColor.white;

        public LabelColorAttribute(EnumAttributeColor labelContentColor, EnumAttributeColor labelBGColor)
        {
            _labelContentColor = labelContentColor;
            _labelBGColor = labelBGColor;
        }
    }

}