using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This shows a error box indicates that this field cannot be null
    /// </summary>
    public class AsTagAttribute : MultiPropertyAttribute
    {
        public AsTagAttribute(bool withLabel = true)
        {
            _withLabel = withLabel;
        }

        bool _withLabel;

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            if(property.propertyType != SerializedPropertyType.String)
            {
                base.OnGUI(position, property, label, fieldInfo);
                EditorGUILayout.HelpBox("Cannot use as tag attribute in a non string field!", MessageType.Warning);
                return;
            }

            if(_withLabel)
                property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
            else
                property.stringValue = EditorGUI.TagField(position, property.stringValue);
        }
#endif
    }
}
