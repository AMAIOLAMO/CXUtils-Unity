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
    public class LimitMaxAttribute : MultiPropertyAttribute
    {
        public LimitMaxAttribute(float maxValue)
        {
            _maxValue = maxValue;
        }

        private float _maxValue;

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            base.OnGUI(position, property, label, fieldInfo);

            if(property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Min(property.floatValue, _maxValue);
                return;
            }

            EditorGUILayout.HelpBox("Limit Max cannot be used on types other than float! if you want to limit Int, use LimitMaxIntAttribute instead!", MessageType.Warning);
        }
#endif
    }
}
