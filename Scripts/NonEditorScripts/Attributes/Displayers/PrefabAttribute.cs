using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This shows the label to be a prefab icon
    /// </summary>
    public class PrefabAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        const string PREFAB_ICON = "Prefab Icon";
        GUIContent IconContext;
#endif

        public PrefabAttribute()
        {
#if UNITY_EDITOR
            IconContext = EditorGUIUtility.IconContent( PREFAB_ICON );
#endif
        }

#if UNITY_EDITOR

        public override GUIContent BuildLabel( GUIContent label )
        {
            IconContext.text = label.text;
            return IconContext;
        }
#endif
    }
}
