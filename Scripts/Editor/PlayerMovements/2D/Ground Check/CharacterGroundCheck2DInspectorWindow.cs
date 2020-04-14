using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CXUtils.DebugHelper;

[CustomEditor(typeof(CharacterGroundCheck2D))]
public class CharacterGroundCheck2DInspectorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        //CharacterGroundCheck2D charGroundC2D = (CharacterGroundCheck2D)target;
        base.OnInspectorGUI();
        
        //charGroundC2D.Tags =
        //    new string[EditorGUILayout.IntField("Tags", charGroundC2D.Tags.Length)];

        //if(charGroundC2D.UsingTags)
        //{
        //    for(int i = 0; i < charGroundC2D.Tags.Length; i ++)
        //        charGroundC2D.Tags[i] = EditorGUILayout.TextField($"{i}", charGroundC2D.Tags[i]);
        //}
    }
}
