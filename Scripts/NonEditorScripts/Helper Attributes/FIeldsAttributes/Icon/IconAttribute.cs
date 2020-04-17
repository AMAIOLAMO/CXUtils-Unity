using System;
using UnityEditor;
using UnityEngine;
/*
 * Made by CXRedix
 * Free tool for unity.
 */
namespace CXUtils.HelperAttributes
{
    /// <summary> Show's an icon to the left of the field using the given path </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Field |
                    AttributeTargets.Enum | AttributeTargets.Struct,
                    AllowMultiple = false)]
    public class IconAttribute : MultiPropertyAttribute
    {
        readonly string textureVarableName;

        public IconAttribute(string textureVarableName) =>
            this.textureVarableName = textureVarableName;

        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            var textureProp = property.serializedObject.FindProperty(textureVarableName);

            if (textureProp == null)
                EditorGUI.HelpBox(position, $"Texture variable \"{textureVarableName}\" doesn't exist, " +
                    "It need's to be a texture variable", MessageType.Error);

            else if (textureProp.propertyType == SerializedPropertyType.ObjectReference)
            {
                label.image = (Texture)textureProp.objectReferenceValue;

                base.Multi_OnGUI(position, property, label, isLast);
            }
            else
            {
                EditorGUI.HelpBox(position, "Texture variable name need's to be a texture variable", MessageType.Error);
                StopNextDraw();
            }
        }
    }
}