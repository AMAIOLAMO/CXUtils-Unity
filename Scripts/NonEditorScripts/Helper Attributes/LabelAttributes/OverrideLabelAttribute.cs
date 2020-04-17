using System;
using UnityEditor;
using UnityEngine;
/*
 * Made by CXRedix
 * Free tool for unity.
 */
namespace CXUtils.HelperAttributes
{
    /// <summary> Overrides the current label </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class OverrideLabelAttribute : MultiPropertyAttribute
    {
        public string LabelTxt { get; set; } = default;

        public OverrideLabelAttribute(string labelTxt = default) =>
            LabelTxt = labelTxt;

        #region drawer code
        public override GUIContent ConstructLabel(GUIContent label)
        {
            if (LabelTxt != default)
                label.text = LabelTxt;

            return label;
        }

        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            if(isLast)
                EditorGUI.PropertyField(position, property, label, true);
        }
        #endregion
    }
}

