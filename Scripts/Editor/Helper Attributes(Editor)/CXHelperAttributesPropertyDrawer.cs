using UnityEngine;
using UnityEditor;
using CXUtils.DebugHelper;
using UnityEngine.UIElements;

namespace CXUtils.HelperAttributes
{
    /// <summary> Show's the given thing </summary>
    [CustomPropertyDrawer(typeof(ShowInInspectorAttribute))]
    public class ShowInInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DebugFunc.Log(this, "yes");
            EditorGUI.PropertyField(position, property, new GUIContent("Lol"), true);
            //base.OnGUI(position, property, label);
        }
    }

    /// <summary> The Property drawer of the show in inspector attribute </summary>
    [CustomPropertyDrawer(typeof(OverrideLabelAttribute))]
    public class OverrideLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            OverrideLabelAttribute attr = attribute as OverrideLabelAttribute;
            GUIContent currentLabel = new GUIContent(label);

            if (attr._labelTxt != default)
                currentLabel.text = attr._labelTxt;

            EditorGUI.PropertyField(position, property, currentLabel, true);
            //base.OnGUI(position, property, label);
        }
    }

    /// <summary> Overrides the GUI's color </summary>
    [CustomPropertyDrawer(typeof(LabelColorAttribute))]
    public class OverrideColorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LabelColorAttribute attr = attribute as LabelColorAttribute;
            GUIContent currentLabel = new GUIContent(label);

            EditorGUI.BeginProperty(position, currentLabel, property);

            GUI.contentColor = EnumAttributeColorReciever.GetColor(attr._labelContentColor);
            GUI.backgroundColor = EnumAttributeColorReciever.GetColor(attr._labelBGColor);

            EditorGUI.PropertyField(position, property, currentLabel, true);

            EditorGUI.EndProperty();
            //base.OnGUI(position, property, label);
        }
    }
}