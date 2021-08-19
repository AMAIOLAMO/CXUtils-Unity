using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This limits the target value above a certain threshold
    /// </summary>
    [AttributeUsage( AttributeTargets.Field )]
    public class LimitMinIntAttribute : MultiPropertyAttribute
    {
        public LimitMinIntAttribute( int minValue )
        {
            _minValue = minValue;
        }

        private int _minValue;

#if UNITY_EDITOR

        public override SerializedProperty BuildProperty( SerializedProperty property )
        {
            if ( property.propertyType == SerializedPropertyType.Integer )
            {
                property.intValue = Mathf.Max( property.intValue, _minValue );
                return property;
            }

            EditorGUILayout.HelpBox( "Limit Min Int cannot be used on types other than int! if you want to limit float, use LimitMinAttribute instead!", MessageType.Warning );
            return property;
        }
#endif
    }
}
