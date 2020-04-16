using System;
using UnityEditor;
using UnityEngine;

namespace CXUtils.HelperAttributes
{
    /// <summary> clamps a given int, float, double in range </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ClampValueAttribute : MultiPropertyAttribute
    {
        readonly float _min, _max;

        public ClampValueAttribute(float min, float max) =>
            (_min, _max) = (min, max);

        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = (int)Mathf.Clamp(property.intValue, _min, _max);
                base.Multi_OnGUI(position, property, label, isLast);
            }

            else if (property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Clamp(property.floatValue, _min, _max);
                base.Multi_OnGUI(position, property, label, isLast);
            }

            else
                HelpBoxError(position, $"property \"{property.name}\" is not a integer, float or a double!");
        }
    }
}
