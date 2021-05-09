using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This makes unity recognize an integer value as a layer value
    /// </summary>
    public class AsLayerAttribute : MultiPropertyAttribute
    {
        public AsLayerAttribute(bool withLabel = true)
        {
            _withLabel = withLabel;
        }

        readonly bool _withLabel;

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            if(property.propertyType != SerializedPropertyType.Integer)
            {
                base.OnGUI(position, property, label, fieldInfo);
                EditorGUILayout.HelpBox("Cannot use as layer attribute in a non int field!", MessageType.Warning);
                return;
            }

            if(_withLabel)
                property.intValue = EditorGUI.LayerField(position, label, property.intValue);
            else
                property.intValue = EditorGUI.LayerField(position, property.intValue);
        }
#endif
    }
}
