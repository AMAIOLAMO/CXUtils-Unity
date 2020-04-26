using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary> A helper for controlling texts </summary>
    public struct TextUtils
    {
        #region Spawn Text Mesh on world

        /// <summary> Instantiates a text on the world </summary>
        public static TextMesh SpawnTextOnWorld(Vector3 position, string text, int fontSize,
            Color color, bool usingLocalPosition = false) =>
            SpawnTextOnWorld(position, text, fontSize, color, TextAnchor.MiddleCenter, TextAlignment.Center, usingLocalPosition);

        /// <summary> Instantiates a text on the world </summary>
        public static TextMesh SpawnTextOnWorld(Vector3 position, string text, int fontSize,
            Color color, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center,
            bool usingLocalPosition = false) =>
            SpawnTextOnWorld(null, position, text, fontSize, color, textAnchor, textAlignment, 0, usingLocalPosition);

        /// <summary> Instantiates a text on the world </summary>
        public static TextMesh SpawnTextOnWorld(Transform parent,
            Vector3 position, string text, int fontSize, Color color, TextAnchor textAnchor = TextAnchor.MiddleCenter,
            TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0, bool usingLocalPosition = false)
        {
            GameObject go_Text = new GameObject("Text_World", typeof(TextMesh));
            Transform trans = go_Text.transform;
            TextMesh txtMesh = go_Text.GetComponent<TextMesh>();

            if (parent != null)
                trans.SetParent(parent);


            if (usingLocalPosition)
                go_Text.transform.localPosition = position;
            else
                go_Text.transform.position = position;

            txtMesh.text = text;
            txtMesh.fontSize = fontSize;
            txtMesh.color = color;
            txtMesh.anchor = textAnchor;
            txtMesh.alignment = textAlignment;

            txtMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return txtMesh;
        }

        #endregion
    }
}
