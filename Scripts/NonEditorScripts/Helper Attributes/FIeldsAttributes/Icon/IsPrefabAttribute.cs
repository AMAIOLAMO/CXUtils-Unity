using UnityEngine;
using UnityEditor;
using System;
/*
 * Made by CXRedix
 * Free tool for unity.
 */
namespace CXUtils.HelperAttributes
{
    /// <summary> Gives the field to say that this is a prefab </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class IsPrefabAttribute : MultiPropertyAttribute
    {
        readonly GUIContent prefabIcon;

        public IsPrefabAttribute(string ToolTip = default) =>
            prefabIcon = EditorGUIUtility.IconContent("Prefab Icon", ToolTip);

        public override GUIContent ConstructLabel(GUIContent label)
        {
            //switch both (only exchange text)
            prefabIcon.text = label.text;
            return prefabIcon;
        }
    }
}