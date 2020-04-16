using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace CXUtils.HelperAttributes
{
    /// <summary> Allow multiple property attribute to use at the same time 
    /// (meant to be derived) </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public abstract class MultiPropertyAttribute : CXPropertyAttribute
    {

        public bool IsLabelChanged { get; set; } = false;

        public bool IsPositionChanged { get; set; } = false;

        public bool IsPropertyChanged { get; set; } = false;

        /// <summary>  Can let the next on gui draw or not </summary>
        public bool CanLetNextDraw { get; set; } = true;

        public List<object> storedAttributes = new List<object>();

        #region Draw code
        public override GUIContent ConstructLabel(GUIContent label)
        {
            IsLabelChanged = true;
            return base.ConstructLabel(label);
        }

        public override Rect ConstructPosition(Rect position)
        {
            IsPositionChanged = true;
            return base.ConstructPosition(position);
        }

        public virtual void StopNextDraw() =>
            CanLetNextDraw = false;

        public override SerializedProperty ConstructProperty(SerializedProperty serializedProperty)
        {
            IsPropertyChanged = true;
            return base.ConstructProperty(serializedProperty);
        }

        public virtual void Multi_OnGUI(Rect position, SerializedProperty property, GUIContent label, bool isLast)
        {
            if (isLast)
                EditorGUI.PropertyField(position, property, label);
        }
        #endregion

        #region Error handeling
        public void HelpBoxError(Rect position, string message, bool stopNextDraw = true)
        {
            EditorGUI.HelpBox(position, message, MessageType.Error);
            if(stopNextDraw)
                StopNextDraw();
        }
        #endregion
    }
}