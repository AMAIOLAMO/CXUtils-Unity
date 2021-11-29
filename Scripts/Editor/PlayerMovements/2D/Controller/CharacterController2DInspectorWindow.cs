using CXUtils.Components;
using UnityEditor;

[CustomEditor( typeof( CharacterController2D ) )]
public class CharacterController2DInspectorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        var charControl2D = (CharacterController2D)target;

        base.OnInspectorGUI();


        if ( charControl2D.Perspec != CharacterController2D.PerspectiveMode.Platformer ) return;

        EditorGUILayout.LabelField( "Platformer Extra Content" );

        charControl2D.CharacterGroundCheck =
            (CharacterGroundCheck2D)
            EditorGUILayout.ObjectField( "Ground Check 2D", charControl2D.CharacterGroundCheck, typeof( CharacterGroundCheck2D ), true );

        charControl2D.PlayerCurrentJumpStrength =
            EditorGUILayout.FloatField( "Jump Strength", charControl2D.PlayerCurrentJumpStrength );
    }
}
