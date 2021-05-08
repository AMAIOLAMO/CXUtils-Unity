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
    public class LimitMinAttribute : MultiPropertyAttribute
    {
        public LimitMinAttribute(float minValue)
        {
            _minValue = minValue;
        }

        private float _minValue;

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            base.OnGUI(position, property, label, fieldInfo);

            if(property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Max(property.floatValue, _minValue);
                return;
            }

            EditorGUILayout.HelpBox("Limit Min cannot be used on types other than float! if you want to limit Int, use LimitMinIntAttribute instead!", MessageType.Warning);
        }
#endif
    }
}
