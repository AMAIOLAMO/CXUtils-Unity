using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// This makes the target field readonly inside the inspector
    /// </summary>
    public class ReadonlyAttribute : MultiPropertyAttribute
    {
        public ReadonlyAttribute(bool withLabel = true)
        {
            _withLabel = withLabel;
        }

        private bool _withLabel;

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            using (new EditorGUI.DisabledGroupScope(true))
                base.OnGUI(position, property, label, fieldInfo);
        }
#endif
    }
}
