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
		public static void DrawText(string text, Float3 global, Color color, GUIStyle style)
		{
#if UNITY_EDITOR
			if (SceneView.currentDrawingSceneView == null) return;

			Handles.BeginGUI();

			DrawTextRaw(text, global, color, SceneView.currentDrawingSceneView, style);

			Handles.EndGUI();
#endif
		}

		public static void DrawText(string text, Float3 global) => DrawText(text, global, Color.white, GUI.skin.box);

#if UNITY_EDITOR
		static void DrawTextRaw(string text, Float3 global, Color color, SceneView sceneView, GUIStyle style)
		{
			Vector3 screenPos = sceneView.camera.WorldToScreenPoint(global.ToUnity());
			Vector2 size = style.CalcSize(new GUIContent(text));
			GUI.color = color;
			Rect position = new Rect(screenPos.x - size.x * .5f, -screenPos.y + sceneView.position.height - size.y * 2f, size.x, size.y);
			GUI.Label(position, text, style);
		}
#endif
	}
}
