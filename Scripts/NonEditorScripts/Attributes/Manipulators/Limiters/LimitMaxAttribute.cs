using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This limits the target value below a certain threshold
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

        public override SerializedProperty GetProperty(SerializedProperty property)
        {
            if(property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Min(property.floatValue, _maxValue);
                return property;
            }
            
            EditorGUILayout.HelpBox("Limit Max cannot be used on types other than float! if you want to limit Int, use LimitMaxIntAttribute instead!", MessageType.Warning);
            return property;
        }
#endif
    }
}
