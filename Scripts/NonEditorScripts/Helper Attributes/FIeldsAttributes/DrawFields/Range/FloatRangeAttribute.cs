using UnityEngine;
using System;
using UnityEditor;
/*
 * Made by CXRedix
 * Free tool for unity.
 */
namespace CXUtils.HelperAttributes
{
    /// <summary> A multi property float Range Attribute </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FloatRangeAttribute : MultiPropertyAttribute
    {
        readonly float _min, _max;

        public FloatRangeAttribute(float min, float max) =>
            (_min, _max) = (min, max);

        #region draw code
        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            //in Range
            if ((property.propertyType == SerializedPropertyType.Float ||
                property.propertyType == SerializedPropertyType.Integer))
            {
                EditorGUI.Slider(position, property, _min, _max, label);
                StopNextDraw();
            }
            else
            {
                HelpBoxError(position, "Use can only assign on INT , FLOAT or DOUBLE", false);
            }
        }
        #endregion

    }
}
