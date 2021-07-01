using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    ///     This will disable a field whenever the given field name's condition is false
    /// </summary>
    public class DisableWhenAttribute : MultiPropertyAttribute
    {

        readonly string _fieldName;
        public DisableWhenAttribute( string fieldName )
        {
            _fieldName = fieldName;
        }


#if UNITY_EDITOR
        public override void OnGUI( in Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo )
        {
            //todo: get a disable variable from the target attribute's scope and get a boolean from the target field name, if not send a help box, else use it for the disable group scope

            //string propertyPath = property.propertyPath;

            //string conditionPath = propertyPath.Replace(property.name, _fieldName);

            var conditionalSP = property.serializedObject.FindProperty( _fieldName );

            if ( conditionalSP == null )
            {
                EditorGUILayout.HelpBox( _fieldName + " does not exist in the context to disable, are you missing something?", MessageType.Warning );
                return;
            }

            if ( conditionalSP.propertyType != SerializedPropertyType.Boolean )
            {
                EditorGUILayout.HelpBox( "You can't use a Diable when attribute on fields that aren't boolean!", MessageType.Warning );
                return;
            }

            using ( new EditorGUI.DisabledGroupScope( !conditionalSP.boolValue ) )
            {
                base.OnGUI( position, property, label, fieldInfo );
            }
        }
#endif
    }
}
