using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This shows a error box indicates that this field cannot be null
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class LimitMinIntAttribute : MultiPropertyAttribute
    {
        public LimitMinIntAttribute(int minValue)
        {
            _minValue = minValue;
        }

        private int _minValue;

#if UNITY_EDITOR

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            base.OnGUI(position, property, label, fieldInfo);

            if(property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = Mathf.Max(property.intValue, _minValue);
                return;
            }

            EditorGUILayout.HelpBox("Limit Min Int cannot be used on types other than int! if you want to limit float, use LimitMinAttribute instead!", MessageType.Warning);
        }
#endif
    }
}
