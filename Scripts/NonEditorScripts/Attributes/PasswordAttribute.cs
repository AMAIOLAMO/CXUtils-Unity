using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This censors all the content typed inside a string field (result will still be stored correctly without "*")
    /// </summary>
    public class PasswordAttribute : MultiPropertyAttribute
    {
        public PasswordAttribute(bool withLabel = true)
        {
            _withLabel = withLabel;
        }

        private bool _withLabel;

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            if(property.propertyType != SerializedPropertyType.String)
            {
                base.OnGUI(position, property, label, fieldInfo);
                EditorGUILayout.HelpBox("Cannot use password attribute in a non string field!", MessageType.Warning);
                return;
            }

            if(_withLabel)
                property.stringValue = EditorGUI.PasswordField(position, label, property.stringValue);
            else
                property.stringValue = EditorGUI.PasswordField(position, property.stringValue);
        }
#endif
    }
}
