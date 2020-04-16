using System;
using UnityEditor;
using UnityEngine;

namespace CXUtils.HelperAttributes
{
    /// <summary> Overrides the color of this object </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class LabelColorAttribute : MultiPropertyAttribute
    {
        public EnumAttributeColor LabelContentColor { get; set; } = EnumAttributeColor.black;

        public LabelColorAttribute(EnumAttributeColor labelContentColor)
        {
            this.LabelContentColor = labelContentColor;
        }

        #region drawer code
        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            //let it draw the editor property
            GUI.color = EnumAttributeColorReciever.GetColor(LabelContentColor);

            if (isLast)
                base.Multi_OnGUI(position, property, label, isLast);
        }
        #endregion
    }
}