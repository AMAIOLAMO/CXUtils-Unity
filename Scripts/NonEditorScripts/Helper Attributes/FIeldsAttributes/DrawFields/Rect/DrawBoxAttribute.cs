using System;
using UnityEngine;
using UnityEditor;

namespace CXUtils.HelperAttributes
{
    public enum DrawBoxOptions
    { text, image }

    /// <summary> Draws a box on the GUI </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DrawBoxAttribute : MultiPropertyAttribute
    {
        readonly string obj = default;

        readonly DrawBoxOptions? drawBoxOptions = null;

        readonly GUIContent guiContent = default;

        /// <summary> Draw box's text or an img </summary>
        public DrawBoxAttribute(string obj,
            DrawBoxOptions drawBoxOptions = DrawBoxOptions.text)
        {
            this.obj = obj;
            this.drawBoxOptions = drawBoxOptions;
        }

        /// <summary> Draw box's GuiContent </summary>
        public DrawBoxAttribute(string text, string imgName, string toolTip = default)
        {
            drawBoxOptions = null;
            guiContent = new GUIContent(text, toolTip);
            obj = imgName;
        }


        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            if(drawBoxOptions == null)
            {
                var img = property.serializedObject.FindProperty(obj);

                if(img == null)
                {
                    HelpBoxError(position, "There is no item called this 1");
                    return;
                }

                if (img.propertyType == SerializedPropertyType.ObjectReference)
                {
                    guiContent.image = (Texture)img.objectReferenceValue;
                    GUI.Box(position, guiContent);
                    base.Multi_OnGUI(position, property, label, isLast);
                }
                else
                    HelpBoxError(position, "There is no Texture");
            }
            else
            {
                if (drawBoxOptions == DrawBoxOptions.text)
                {
                    GUI.Box(position, obj);
                    base.Multi_OnGUI(position, property, label, isLast);
                }
                else if (drawBoxOptions == DrawBoxOptions.image)
                {
                    var img = property.serializedObject.FindProperty(obj);
                    if(img == null)
                    {
                        HelpBoxError(position, "There is no item called this 2");
                        return;
                    }

                    if (img.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        GUI.Box(position, (Texture)img.objectReferenceValue);
                        base.Multi_OnGUI(position, property, label, isLast);
                    }
                    else
                        HelpBoxError(position, "There is no Texture");
                }
            }
            
        }
    }
}