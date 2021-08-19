using UnityEditor;
using UnityEngine;

namespace CXUtils.Unity
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scenePathProperty = property.FindPropertyRelative("_scenePath");

            var oldSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePathProperty.stringValue);

            EditorGUI.BeginChangeCheck();

            var newSceneAsset = EditorGUI.ObjectField(position, label, oldSceneAsset, typeof(SceneAsset), false) as SceneAsset;
            
            if ( EditorGUI.EndChangeCheck() )
            {
                if ( newSceneAsset == null )
                {
                    scenePathProperty.stringValue = null;
                    return;
                }

                var newPath = AssetDatabase.GetAssetPath(newSceneAsset);

                scenePathProperty.stringValue = newPath;
            }
        }
    }
}

