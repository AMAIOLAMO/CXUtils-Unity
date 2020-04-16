using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CXUtils.HelperAttributes
{
    /// <summary> Draws a rect on the inspector </summary>
    [AttributeUsage(AttributeTargets.Field |
                    AttributeTargets.Enum | AttributeTargets.Struct,
                    AllowMultiple = false)]
    public class DrawRectAttribute : MultiPropertyAttribute
    {
        #region enums
        public enum RectConstrains
        { X, Y, Width, Height, All, None }
        #endregion

        #region Vars
        Rect rect = default;

        EnumAttributeColor rectColor = default;
        RectConstrains rectConstrains = default;

        string rectStr = default;
        #endregion

        #region Constructors
        public DrawRectAttribute(float x1, float y1, float w1, float h1,
            EnumAttributeColor rectColor = EnumAttributeColor.white,
            RectConstrains rectConstrains = RectConstrains.None)
        {
            Rect BGRect = default;
            (BGRect.x, BGRect.y) = (x1, y1);
            (BGRect.width, BGRect.height) = (w1, h1);
            this.rectColor = rectColor;
            this.rectConstrains = rectConstrains;
        }

        public DrawRectAttribute(string rect,
            EnumAttributeColor rectColor = EnumAttributeColor.white,
            RectConstrains rectConstrains = RectConstrains.None)
        {
            rectStr = rect;
            this.rectColor = rectColor;
            this.rectConstrains = rectConstrains;
        }
        #endregion

        #region draw code
        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {


            if (rect != default)
            {
                ChangeRectToPosition(ref rect, position);

                EditorGUI.DrawRect(rect, EnumAttributeColorReciever.GetColor(rectColor));
                base.Multi_OnGUI(position, property, label, isLast);
            }
            else
            {
                var Srect = property.serializedObject.FindProperty(rectStr);

                if (Srect != null)
                {
                    if (Srect.propertyType == SerializedPropertyType.Rect)
                    {
                        Rect TempR = new Rect(Srect.rectValue);

                        ChangeRectToPosition(ref TempR, position);

                        Srect.rectValue = TempR;

                        EditorGUI.DrawRect(TempR, EnumAttributeColorReciever.GetColor(rectColor));
                        base.Multi_OnGUI(position, property, label, isLast);
                    }
                    else
                        HelpBoxError(position, $"\"{rectStr}\" should be type Rect");
                }

                else
                    HelpBoxError(position, $"There is no rect variable called: \"{rectStr}\"");
            }
        }
        #endregion

        #region Script Methods
        private void ChangeRectToPosition(ref Rect TempR, Rect position)
        {
            if (rectConstrains == RectConstrains.All || rectConstrains == RectConstrains.X)
                TempR.x = position.x;
            if (rectConstrains == RectConstrains.All || rectConstrains == RectConstrains.Y)
                TempR.y = position.y;
            if (rectConstrains == RectConstrains.All || rectConstrains == RectConstrains.Width)
                TempR.width = position.width;
            if (rectConstrains == RectConstrains.All || rectConstrains == RectConstrains.Height)
                TempR.height = position.height;
        }
        #endregion
    }
}