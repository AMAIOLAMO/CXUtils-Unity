using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary> A helper for controlling texts </summary>
    public class TextUtils : CXBaseUtils
    {
        #region SpawnTxtOnWorld

        /// <summary> Instantiates a text on the world </summary>
        public static TextMesh SpawnTextOnWorld(Transform parent, Vector3 position, Func<TextMesh, TextMesh> modifier,
            int sortingOrder = 0, bool usingLocalPosition = false)
        {
            GameObject go_Text = new GameObject("Text_World", typeof(TextMesh));
            Transform trans = go_Text.transform;
            TextMesh txtMesh = go_Text.GetComponent<TextMesh>();

            //parent
            if (parent != null)
                trans.SetParent(parent);

            //setting transform
            if (usingLocalPosition)
                trans.localPosition = position;
            else
                trans.position = position;

            //txtMesh set
            txtMesh = modifier(txtMesh);

            //set sorting order
            txtMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return txtMesh;
        }

        /// <summary> Instantiates a text on the world </summary>
        public static TextMesh SpawnTextOnWorld(Vector3 position, string text, int fontSize,
            Color color, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center,
            bool usingLocalPosition = false) =>
            SpawnTextOnWorld(null, position, text, fontSize, color, textAnchor, textAlignment, 0, usingLocalPosition);

        /// <summary> Instantiates a text on the world </summary>
        public static TextMesh SpawnTextOnWorld(Transform parent,
            Vector3 position, string text, int fontSize = 30, Color color = default, TextAnchor textAnchor = TextAnchor.MiddleCenter,
            TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0, bool usingLocalPosition = false) =>
            SpawnTextOnWorld(parent, position,
                (txtMesh) => SetTextMesh(txtMesh, text, fontSize, color, textAnchor, textAlignment),
                sortingOrder, usingLocalPosition);

        //Just a simplifier for setting the text mesh obj
        private static TextMesh SetTextMesh(TextMesh txtMesh, string text, int fontSize, Color color,
            TextAnchor textAnchor, TextAlignment textAlignment)
        {
            txtMesh.text = text;
            txtMesh.fontSize = fontSize;
            txtMesh.color = color;
            txtMesh.anchor = textAnchor;
            txtMesh.alignment = textAlignment;

            return txtMesh;
        }

        #endregion
    }
}
