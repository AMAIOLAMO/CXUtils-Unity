using UnityEditor;
using UnityEngine;

namespace CXUtils.Unity
{
    //NOT FINISHED!
    //[CustomPropertyDrawer(typeof(SceneBundle))]
    public class SceneBundleDrawer : PropertyDrawer
    {
        bool _foldout = false;

        const string REFERENCES_PROPERTY_IDENTIFIER = "_references";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var referencesProperty = property.FindPropertyRelative(REFERENCES_PROPERTY_IDENTIFIER);

            _foldout = EditorGUI.BeginFoldoutHeaderGroup(position, _foldout, label);

            if (_foldout)
            {
                float propertyHeight = EditorGUI.GetPropertyHeight(referencesProperty, true);

                int referencesArraySize = referencesProperty.arraySize;

                for ( int i = 0; i < referencesArraySize; ++i )
                {
                    EditorGUI.PropertyField(new Rect(0, propertyHeight * i, position.width, propertyHeight), referencesProperty.GetArrayElementAtIndex(i), true);
                }

                if ( GUI.Button(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + propertyHeight * referencesArraySize, position.width, EditorGUIUtility.singleLineHeight), "Add New"))
                {
                    Debug.Log("hey :D");
                }
            }

            EditorGUI.EndFoldoutHeaderGroup();

            if ( referencesProperty.arraySize != 0 )
                return;

            //else
            EditorGUILayout.HelpBox(nameof(SceneBundle) + " cannot have 0 elements!", MessageType.Error);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if ( !_foldout )
                return base.GetPropertyHeight(property, label) + GUI.skin.box.CalcHeight(label, Screen.width);
            //else

            var referencesProperty = property.FindPropertyRelative(REFERENCES_PROPERTY_IDENTIFIER);

            float propertyHeight = EditorGUI.GetPropertyHeight(referencesProperty, true);

            return base.GetPropertyHeight(property, label) + GUI.skin.box.CalcHeight(label, Screen.width) + referencesProperty.arraySize * propertyHeight;
        }
    }
}

