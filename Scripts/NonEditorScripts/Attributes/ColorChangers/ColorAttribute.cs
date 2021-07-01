using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    ///     Base class of all Color Multi Property Attributes
    /// </summary>
    public abstract class ColorAttribute : MultiPropertyAttribute
    {
        readonly string _hexColor;
        readonly bool _onlyThisField;

        public ColorAttribute( string hexColor, bool onlyThisField = false )
        {
            ( _hexColor, _onlyThisField ) = ( hexColor, onlyThisField );
        }

#if UNITY_EDITOR
        public abstract Color GetOriginColor();
        public abstract void SetColor( in Color color );

        public override void OnGUI( in Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo )
        {
            var originColor = GetOriginColor();

            if ( ColorUtility.TryParseHtmlString( _hexColor, out var color ) )
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
