using System;
using System.Reflection;

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
        public ToStepAttribute(float step)
        {
            _step = step;
        }

        private float _step;

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            if(property.propertyType != SerializedPropertyType.Float)
            {
                base.OnGUI(position, property, label, fieldInfo);
                EditorGUILayout.HelpBox("Cannot use To Step attribute in a non Float field if u want to Step int use ToStepIntAttribute Instead!", MessageType.Warning);
                return;
            }

            throw new NotImplementedException();
        }
#endif
    }
}
