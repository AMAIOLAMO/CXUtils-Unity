﻿using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// Base class of all Color Multi Property Attributes
    /// </summary>
    public abstract class ColorAttribute : MultiPropertyAttribute
    {
        public ColorAttribute( string hexColor, bool onlyThisField = false ) =>
            (_hexColor, _onlyThisField) = (hexColor, onlyThisField);

        private readonly string _hexColor;
        private readonly bool _onlyThisField;

#if UNITY_EDITOR
        public abstract Color GetColor();
        public abstract void SetColor( Color color );

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo )
        {
            Color originColor = GetColor();

            if ( ColorUtility.TryParseHtmlString( _hexColor, out Color color ) )
                SetColor( color );
            else
                EditorGUILayout.HelpBox( "the given hexColor is invalid!", MessageType.Error );

            base.OnGUI( position, property, label, fieldInfo );

            if ( _onlyThisField )
                SetColor( originColor );
        }
#endif
    }
}
