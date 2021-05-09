using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This will disable a field whenever the given field name's condition is false
    /// </summary>
    public class DisableWhenAttribute : MultiPropertyAttribute
    {
        public DisableWhenAttribute(string fieldName, bool withLabel = true)
        {
            _fieldName = fieldName;
            _withLabel = withLabel;
        }

        private readonly string _fieldName;
        private readonly bool _withLabel;


#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            //todo: get a disable variable from the target attribute's scope and get a boolean from the target field name, if not send a help box, else use it for the disable group scope
            
            string propertyPath = property.propertyPath;

            string conditionPath = propertyPath.Replace(property.name, _fieldName);

            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
            
            if(sourcePropertyValue == null)
            {
                EditorGUILayout.HelpBox(_fieldName + " does not exist in the context to disable, are you missing something?", MessageType.Warning);
                return;
            }
            
            if(sourcePropertyValue.propertyType != SerializedPropertyType.Boolean)
            {
                EditorGUILayout.HelpBox("You can't use a Diable when attribute on fields that aren't boolean!", MessageType.Warning);
                return;
            }

            using (new EditorGUI.DisabledGroupScope(!sourcePropertyValue.boolValue))
            {
                if ( _withLabel )
                    EditorGUI.PropertyField(position, property, label);
                else
                    EditorGUI.PropertyField(position, property);
            }
        }
#endif
    }
}
