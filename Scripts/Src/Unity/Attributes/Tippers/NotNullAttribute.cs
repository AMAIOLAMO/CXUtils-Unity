using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    ///     This shows a error box indicates that this field cannot be null
    /// </summary>
    public class NotNullAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override void OnGUI( in Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo )
        {
            base.OnGUI( position, property, label, fieldInfo );

            //if property is not null and object reference value is null then we show message
            if ( property?.objectReferenceValue != null )
                return;

            string resultMessage = "Variable or Property: " + property.displayName + " cannot be null!";

            EditorGUILayout.HelpBox( resultMessage, MessageType.Error );
        }
#endif
    }
}
