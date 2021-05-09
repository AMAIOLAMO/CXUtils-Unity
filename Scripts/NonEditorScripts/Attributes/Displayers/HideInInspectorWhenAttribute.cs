using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This will disable a field whenever the given field name's condition is false
    /// </summary>
    public class HideInInspectorWhenAttribute : MultiPropertyAttribute
    {
        public HideInInspectorWhenAttribute( string fieldName )
        {
            _fieldName = fieldName;
        }

        private readonly string _fieldName;


#if UNITY_EDITOR
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo )
        {
            //todo: get a disable variable from the target attribute's scope and get a boolean from the target field name, if not send a help box, else use it for the disable group scope

            var conditionSP = property.serializedObject.FindProperty( _fieldName );

            if ( conditionSP == null )
            {
                EditorGUILayout.HelpBox( _fieldName + " does not exist in the context to disable, are you missing something?", MessageType.Warning );
                return;
            }

            if ( conditionSP.propertyType != SerializedPropertyType.Boolean )
            {
                EditorGUILayout.HelpBox( "You can't use a Diable when attribute on fields that aren't boolean!", MessageType.Warning );
                return;
            }

            //if false then won't show
            if ( !conditionSP.boolValue )
                return;

            base.OnGUI( position, property, label, fieldInfo );
        }
#endif
    }
}
