using CXUtils.Domain.Types;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace CXUtils.Unity
{
    /// <summary>
    ///     An utility to draw stuff in <see cref="SceneView" />
    /// </summary>
    public static class HandleUtils
    {
#if UNITY_EDITOR
        static void DrawTextRaw( string text, Float3 worldPosition, Color color, SceneView sceneView, GUIStyle style )
        {
            var screenPos = sceneView.camera.WorldToScreenPoint( worldPosition.ToUnity() );
            var size = style.CalcSize( new GUIContent( text ) );
            GUI.color = color;
            GUI.Label( new Rect( screenPos.x - size.x * .5f, -screenPos.y + sceneView.position.height - size.y * 2f, size.x, size.y ), text, style );
        }
#endif

        public static void DrawText( string text, Float3 worldPosition, Color color, GUIStyle style )
        {
#if UNITY_EDITOR
            if ( SceneView.currentDrawingSceneView == null ) return;

            Handles.BeginGUI();

            DrawTextRaw( text, worldPosition, color, SceneView.currentDrawingSceneView, style );

            Handles.EndGUI();
#endif
        }

        public static void DrawText( string text, Float3 worldPosition ) => DrawText( text, worldPosition, Color.white, GUI.skin.box );
    }
}
