using System;
using UnityEditor;
using UnityEngine;

namespace CXUtils.HelperAttributes
{
    /// <summary> Show's a icon with the given name that relates in the unity editor </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class UnityIconAttribute : MultiPropertyAttribute
    {
        bool hasIcon = false;

        readonly GUIContent Icon;
        readonly string name;

        public UnityIconAttribute(string name, string ToolTip = default)
        {
            this.name = name;
            Icon = EditorGUIUtility.IconContent(name, ToolTip);
        }

        public override GUIContent ConstructLabel(GUIContent label)
        {
            //switch both (only exchange text)
            if (Icon != null)
            {
                hasIcon = true;
                Icon.text = label.text;
                return Icon;
            }
            return label;
        }

        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            if (hasIcon)
                base.Multi_OnGUI(position, property, label, isLast);
            else
                HelpBoxError(position, $"Unity has no Icon content of \"{name}\"");
        }

    }
}