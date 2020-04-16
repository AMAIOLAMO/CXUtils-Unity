using CXUtils.DebugHelper;
using System;
using UnityEditor;
using UnityEngine;

namespace CXUtils.HelperAttributes
{
    /// <summary> A multi property float Range Attribute </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IntRangeAttribute : MultiPropertyAttribute
    {
        readonly int _min, _max;

        public IntRangeAttribute(int min, int max) =>
            (_min, _max) = (min, max);

        #region draw code
        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            //in Range
            if (property.propertyType == SerializedPropertyType.Integer && isLast)
                EditorGUI.IntSlider(position, property, _min, _max);

            else
            {
                if (!isLast)
                    CanLetNextDraw = false;

                else
                    HelpBoxLogError(position,
                        "Use can only assign on INT",
                        "wrong field type!");
            }
        }
        #endregion

        #region Script Methods
        private void HelpBoxLogError(Rect position, string helpBoxText, string logErrorText)
        {
            EditorGUI.HelpBox(position, helpBoxText, MessageType.Error);
            DebugFunc.LogError<Exception>(this, logErrorText);
        }
        #endregion
    }
}
