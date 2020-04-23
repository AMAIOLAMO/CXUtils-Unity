using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary> Helps manage the UI text </summary>
    public struct UITextUtils
    {
        #region SpawnText
        /// <summary> Instantiates a text on the world </summary>
        public static TextMesh SpawnTextOnWorld(Transform parent,
            Vector3 localPosition, string text, int fontSize, Color color, TextAnchor textAnchor,
            TextAlignment textAlignment, int sortingOrder)
        {
            GameObject GO_Text = new GameObject("Text_World", typeof(TextMesh));
            Transform trans = GO_Text.transform;

            trans.SetParent(parent);

            TextMesh txtMesh = GO_Text.GetComponent<TextMesh>();

            GO_Text.transform.position = localPosition;

            txtMesh.text = text;
            txtMesh.fontSize = fontSize;
            txtMesh.color = color;
            txtMesh.anchor = textAnchor;

            txtMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return txtMesh;
        }
        #endregion
    }
}
