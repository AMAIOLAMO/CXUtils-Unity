using System;
using UnityEditor;
using UnityEngine;

namespace CXUtils.HelperAttributes
{
    /// <summary> Shows a field class or property in the inspector window </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class ShowInInspectorAttribute : MultiPropertyAttribute
    {
        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            EditorGUI.PropertyField(position, property, new GUIContent("Lol"), true);
        }
    }
}