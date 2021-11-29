using System;
using UnityEngine;

namespace CXUtils.Unity
{
    /// <summary>
    ///     A helper for text debugging
    /// </summary>
    public struct TextUtils
    {
        /// <summary>
        ///     Instantiates a text on the scene world
        /// </summary>
        public static TextMesh SpawnText( Transform parent, Vector3 position,
            Func<TextMesh, TextMesh> modifier, int sortOrder = 0, bool useLocalPosition = false )
        {
            var goText = new GameObject( "Text_World", typeof( TextMesh ) );
            var transform = goText.transform;
            var txtMesh = goText.GetComponent<TextMesh>();

            //parent
            if ( parent != null )
                transform.SetParent( parent );

            //setting transform
            if ( useLocalPosition )
                transform.localPosition = position;
            else
                transform.position = position;

            //txtMesh set
            txtMesh = modifier( txtMesh );

            //set sorting order
            txtMesh.GetComponent<MeshRenderer>().sortingOrder = sortOrder;

            return txtMesh;
        }

        /// <inheritdoc
        ///     cref="SpawnText" />
        public static TextMesh SpawnText( Vector3 position, string text, int fontSize,
            Color color, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center,
            bool useLocalPosition = false ) =>
            SpawnText( null, position, text, fontSize, color, textAnchor, textAlignment, 0, useLocalPosition );

        /// <inheritdoc
        ///     cref="SpawnText" />
        public static TextMesh SpawnText( Transform parent,
            Vector3 position, string text, int fontSize = 30, Color color = default, TextAnchor textAnchor = TextAnchor.MiddleCenter,
            TextAlignment textAlignment = TextAlignment.Center, int sortOrder = 0, bool useLocalPosition = false ) =>
            SpawnText( parent, position,
                txtMesh => SetTextMesh( txtMesh, text, fontSize, color, textAnchor, textAlignment ),
                sortOrder, useLocalPosition );

        static TextMesh SetTextMesh( TextMesh txtMesh, string text, int fontSize,
            Color color, TextAnchor textAnchor, TextAlignment textAlignment )
        {
            txtMesh.text = text;
            txtMesh.fontSize = fontSize;
            txtMesh.color = color;
            txtMesh.anchor = textAnchor;
            txtMesh.alignment = textAlignment;

            return txtMesh;
        }
    }
}
