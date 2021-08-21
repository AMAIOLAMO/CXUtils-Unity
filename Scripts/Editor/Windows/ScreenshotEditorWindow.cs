using System.IO;
using UnityEditor;
using UnityEngine;

namespace CXUtils.Editors
{
    public class ScreenshotWindow : EditorWindow
    {
        ScreenCapture.StereoScreenCaptureMode _captureMode = ScreenCapture.StereoScreenCaptureMode.BothEyes;

        string _saveName = "save name here";
        string _savePath = "save path here";

        void OnGUI()
        {
            _savePath = EditorGUILayout.TextField("SavePath", _savePath);
            _saveName = EditorGUILayout.TextField("SaveName", _saveName);

            _captureMode = (ScreenCapture.StereoScreenCaptureMode)EditorGUILayout.EnumPopup("Stereo Screen Capture mode", _captureMode);

            bool isDirectory = Directory.Exists(_savePath);

            string resultPath = Path.Combine(_savePath, _saveName);

            using ( new GUILayout.HorizontalScope() )
            {
                GUILayout.Label("Result Path");
                GUILayout.Box(resultPath);
            }

            GUI.enabled = isDirectory;

            DoEditor(resultPath);

            GUI.enabled = true;

            if ( !isDirectory )
                EditorGUILayout.HelpBox("Save path must be a directory!", MessageType.Error);

            if ( string.IsNullOrWhiteSpace(_saveName) )
                EditorGUILayout.HelpBox("Save name must be written!", MessageType.Error);

            if ( File.Exists(resultPath) )
                EditorGUILayout.HelpBox("There's a file that exist with the same name, beware that you might override the original file", MessageType.Info);

            // == is playing ==
        }

        [MenuItem("CXUtils/Tools/Screenshot Window")]
        public static void OpenWindow()
        {
            GetWindow<ScreenshotWindow>(nameof(ScreenshotWindow));
        }

        void DoEditor(string resultPath)
        {
            if ( GUILayout.Button("Capture") )
            {
                Debug.Log("Saving screenshot to path: " + resultPath);

                ScreenCapture.CaptureScreenshot(resultPath, _captureMode);
            }
        }
    }
}
