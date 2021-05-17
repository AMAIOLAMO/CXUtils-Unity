#if UNITY_EDITOR
using System.Collections.Generic;
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
        public List<object> attrList;

        /// <summary>
        /// The basic OnGUI Method from property drawers
        /// </summary>
        // /// <returns>should the current on gui block other on gui's below?</returns>
        public virtual void OnGUI( Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo ) =>
            EditorGUI.PropertyField( position, property, label, true );

        /// <summary>
        /// Recieves a new position
        /// </summary>
        /// <returns>New position that will be changed</returns>
        public virtual Rect BuildPosition( Rect position ) => position;

        /// <summary>
        /// Recieves a new property
        /// </summary>
        /// <returns>New property that will be changed</returns>
        public virtual SerializedProperty BuildProperty( SerializedProperty property ) => property;

        /// <summary>
        /// Recieves a new label
        /// </summary>
        /// <returns>New label that will be changed</returns>
        public virtual GUIContent BuildLabel( GUIContent label ) => label;

        public virtual float? GetPropertyHeight( SerializedProperty property, GUIContent label ) => null;
#endif
    }
}
