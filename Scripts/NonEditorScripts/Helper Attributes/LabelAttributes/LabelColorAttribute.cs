using System;
using UnityEditor;
using UnityEngine;
/*
 * Made by CXRedix
 * Free tool for unity.
 */
namespace CXUtils.HelperAttributes
{
    /// <summary> Overrides the color of this object </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ColorModifierAttribute : MultiPropertyAttribute
    {
        public EnumAttributeColor LabelContentColor { get; set; } = EnumAttributeColor.black;
        public string colorVar = null;

        public ColorModifierAttribute(EnumAttributeColor labelContentColor) =>
            LabelContentColor = labelContentColor;

        public ColorModifierAttribute(string colorVar) =>
            this.colorVar = colorVar;

        #region drawer code
        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            //let it draw the editor property
            if (colorVar != null)
            {
                var color = property.serializedObject.FindProperty(colorVar);

                if (color == null)
                    HelpBoxError(position, "No such color var item");

                else
                {
                    if (color.propertyType == SerializedPropertyType.Color)
                    {
                        GUI.color = color.colorValue;
                        base.Multi_OnGUI(position, property, label, isLast);
                    }
                    else
                        HelpBoxError(position, "Item type is not color!");
                }
            }
            else
                GUI.color = CXEColorReciever.GetColor(LabelContentColor);

            base.Multi_OnGUI(position, property, label, isLast);
        }
        #endregion
    }
}