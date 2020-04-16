using UnityEditor;
using UnityEngine;

namespace CXUtils.HelperAttributes
{
    /// <summary> base class of all cx property attributes </summary>
    public abstract class CXPropertyAttribute : PropertyAttribute
    {
        #region Contstructors
        public virtual GUIContent ConstructLabel(GUIContent label) =>
            label;

        public virtual Rect ConstructPosition(Rect position) =>
            position;

        public virtual SerializedProperty ConstructProperty(SerializedProperty serializedProperty) =>
            serializedProperty;
        #endregion

        #region Draw code

        public virtual float? GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            null;
        #endregion
    }
}

