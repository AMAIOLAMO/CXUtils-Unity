#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    public class LabelIconAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        private readonly GUIContent _iconContext;
#endif

        public LabelIconAttribute( string contextName )
        {
#if UNITY_EDITOR
            _iconContext = EditorGUIUtility.IconContent( contextName );
#endif
        }

#if UNITY_EDITOR

        public override GUIContent BuildLabel( GUIContent label )
        {
            if ( _iconContext == null )
                return label;

            _iconContext.text = label.text;
            _iconContext.tooltip = label.tooltip;
            return _iconContext;
        }
#endif
    }
}
