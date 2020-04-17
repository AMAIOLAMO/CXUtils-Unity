using System;
using UnityEditor;
using UnityEngine;
/*
 * Made by CXRedix
 * Free tool for unity.
 */

namespace CXUtils.HelperAttributes
{
    /// <summary> clamps a given float, double in range </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FloatClampValueAttribute : MultiPropertyAttribute
    {
        readonly float _min, _max;

        public FloatClampValueAttribute(float min, float max) =>
            (_min, _max) = (min, max);

        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            if (property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Clamp(property.floatValue, _min, _max);
                base.Multi_OnGUI(position, property, label, isLast);
            }

            else
                HelpBoxError(position, $"property \"{property.name}\" is not a integer, float or a double!");
        }
    }

    /// <summary> clamps a given int in range </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IntClampValueAttribute : MultiPropertyAttribute
    {
        readonly int _min, _max;

        public IntClampValueAttribute(int min, int max) =>
            (_min, _max) = (min, max);

        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = (int)Mathf.Clamp(property.intValue, _min, _max);
                base.Multi_OnGUI(position, property, label, isLast);
            }

            else
                HelpBoxError(position, $"property \"{property.name}\" is not a integer, float or a double!");
        }
    }
}
