using MathUtils = CXUtils.CodeUtils.MathUtils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This will force the step value into a step (NOT IN USE)
    /// </summary>
    public class ToStepAttribute : MultiPropertyAttribute
    {
        public ToStepAttribute( float step )
        {
            _step = step;
        }

        private readonly float _step;

#if UNITY_EDITOR
        public override SerializedProperty BuildProperty( SerializedProperty property )
        {
            if ( _step <= 0 )
            {
                EditorGUILayout.HelpBox( "Cannot use " + _step.ToString() + " because it's below 0!", MessageType.Warning );
                return property;
            }

            if ( property.propertyType != SerializedPropertyType.Float )
            {
                EditorGUILayout.HelpBox( "Cannot use To Step attribute in a non Float field if u want to Step int use ToStepIntAttribute Instead!", MessageType.Warning );
                return property;
            }

            property.floatValue = MathUtils.RoundOnStep(property.floatValue, _step);
            return property;
        }
#endif
    }
}
