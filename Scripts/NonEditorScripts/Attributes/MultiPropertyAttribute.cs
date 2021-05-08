#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// Base of all CX property attributes (since this will work for all overrides)
    /// </summary>
    public abstract class MultiPropertyAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        /// <summary>
        /// The basic OnGUI Method from property drawers
        /// </summary>
        public virtual void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo) =>
            EditorGUI.PropertyField(position, property, label);

        /// <summary>
        /// Recieves a new position
        /// </summary>
        /// <returns>New position that will be changed</returns>
        public virtual Rect GetPosition(Rect position) => position;

        /// <summary>
        /// Recieves a new property
        /// </summary>
        /// <returns>New property that will be changed</returns>
        public virtual SerializedProperty GetProperty(SerializedProperty property) => property;

        /// <summary>
        /// Recieves a new label
        /// </summary>
        /// <returns>New label that will be changed</returns>
        public virtual GUIContent GetLabel(GUIContent label) => label;
#endif
    }
}
