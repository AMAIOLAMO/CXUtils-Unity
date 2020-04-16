using System;
using UnityEditor;
using UnityEngine;

namespace CXUtils.HelperAttributes
{
    /// <summary> A multi property float Range Attribute </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NotNullAttribute : MultiPropertyAttribute
    {
        #region draw code
        public override void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            bool hasValue = false;

            if (property != null)
            {
                if (!hasValue && property.objectReferenceValue != null)
                    hasValue = true;

                if (!hasValue && property.exposedReferenceValue != null)
                    hasValue = true;
            }

            if (!hasValue)
                Debug.LogError($"{property.name} cannot be null!");

            base.Multi_OnGUI(position, property, label, isLast);
        }
        #endregion
    }
}
