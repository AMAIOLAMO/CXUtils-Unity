using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This shows a error box indicates that this field cannot be null
    /// </summary>
    public class NotNullAttribute : MultiPropertyAttribute
    {
        public NotNullAttribute() { }

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            base.OnGUI(position, property, label, fieldInfo);

            //if property is not null and object refrence value is null then we show message
            if ( property?.objectReferenceValue == null )
                EditorGUILayout.HelpBox("Variable or Property: " + property.displayName + " cannot be null!", MessageType.Error);
        }
#endif
    }
}
