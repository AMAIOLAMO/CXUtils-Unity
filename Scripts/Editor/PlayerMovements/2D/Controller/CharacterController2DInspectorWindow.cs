using UnityEditor;
using CXUtils.HelperComponents;

[CustomEditor(typeof(CharacterController2D))]

public class CharacterController2DInspectorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        CharacterController2D charControl2D = (CharacterController2D)target;

        base.OnInspectorGUI();

        
        if(charControl2D.GamePerspecOptions == CharacterController2D.GamePerspectiveOptions.Platformer)
        {
            EditorGUILayout.LabelField("Platformer Extra Content");

            charControl2D.CharacterGroundCheck =
                (CharacterGroundCheck2D)
                EditorGUILayout.ObjectField("Ground Check 2D", charControl2D.CharacterGroundCheck, typeof(CharacterGroundCheck2D), true);

            charControl2D.PlayerCurrentJumpStrength =
                EditorGUILayout.FloatField("Jump Strength", charControl2D.PlayerCurrentJumpStrength);
        }
    }
}