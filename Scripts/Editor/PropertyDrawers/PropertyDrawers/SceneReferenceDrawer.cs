using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CXUtils.Unity
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scenePathProperty = property.FindPropertyRelative("_scenePath");
            var buildIndexProperty = property.FindPropertyRelative("_buildIndex");

            var oldSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePathProperty.stringValue);

            EditorGUI.BeginChangeCheck();

            SceneAsset newSceneAsset;

            using ( new GUI.GroupScope(position, label.text, GUI.skin.box) )
            {
                var newRect = new Rect(0f, GUI.skin.box.CalcHeight(label, position.width), position.width, EditorGUIUtility.singleLineHeight);
                newSceneAsset = EditorGUI.ObjectField(newRect, oldSceneAsset, typeof(SceneAsset), false) as SceneAsset;
            }

            if ( EditorGUI.EndChangeCheck() )
            {
                if ( newSceneAsset == null )
                {
                    scenePathProperty.stringValue = null;
                    buildIndexProperty.intValue = -1;
                    return;
                }

                var newPath = AssetDatabase.GetAssetPath(newSceneAsset);
                var newScene = SceneManager.GetSceneByPath(newPath);

                scenePathProperty.stringValue = newPath;

                buildIndexProperty.intValue = newScene.IsValid() ? newScene.buildIndex : -1;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + GUI.skin.box.CalcSize(label).y;
        }
    }
}

