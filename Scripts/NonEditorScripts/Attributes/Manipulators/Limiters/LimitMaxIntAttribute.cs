using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This limits the target value under a certain threshold
    /// </summary>
    [AttributeUsage( AttributeTargets.Field )]
    public class LimitMaxIntAttribute : MultiPropertyAttribute
    {
        public LimitMaxIntAttribute( int maxValue )
        {
            _maxValue = maxValue;
        }

        private int _maxValue;

#if UNITY_EDITOR

        public override SerializedProperty BuildProperty( SerializedProperty property )
        {
            if ( property.propertyType == SerializedPropertyType.Integer )
            {
                property.intValue = Mathf.Min( property.intValue, _maxValue );
                return property;
            }

            EditorGUILayout.HelpBox( "Limit Max cannot be used on types other than int! if you want to limit float, use LimitMaxAttribute instead!", MessageType.Warning );
            return property;
        }
#endif
    }
}
