using UnityEditor;
using UnityEngine;
using System;

namespace CXUtils.HelperAttributes
{
    /// <summary> Will enable the field if the current condition match </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Field |
                    AttributeTargets.Enum | AttributeTargets.Struct,
                    AllowMultiple = false)]
    public class ActiveIfAttribute : MultiPropertyAttribute
    {
        readonly string condition;
        public ActiveIfAttribute(string condition) =>
            this.condition = condition;

        #region draw code
        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            //get var
            var conditionBoolean = property.serializedObject.FindProperty(condition);

            if (conditionBoolean == null)
            {
                EditorGUI.HelpBox(position, $"Condition \"{condition}\" doesn't exist, " +
                    "it must be a boolean value", MessageType.Error);
                StopNextDraw();
            }

            else if (conditionBoolean.propertyType == SerializedPropertyType.Boolean)
            {
                GUI.enabled = conditionBoolean.boolValue;
                base.Multi_OnGUI(position, property, label, isLast);
            }

            else
            {
                EditorGUI.HelpBox(position, "Condition must be a boolean value", MessageType.Error);
                StopNextDraw();
            }
        }
        #endregion
    }
}

