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
        public static TextMesh SpawnTextOnWorld( Transform parent, Vector3 position,
            Func<TextMesh, TextMesh> modifier, int sortingOrder = 0, bool usingLocalPosition = false )
        {
            var goText = new GameObject( "Text_World", typeof( TextMesh ) );
            var transform = goText.transform;
            var txtMesh = goText.GetComponent<TextMesh>();

            //parent
            if ( parent != null )
                transform.SetParent( parent );

            //setting transform
            if ( usingLocalPosition )
                transform.localPosition = position;
            else
                transform.position = position;

            //txtMesh set
            txtMesh = modifier( txtMesh );

            //set sorting order
            txtMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return txtMesh;
        }

        /// <inheritdoc cref="SpawnTextOnWorld(UnityEngine.Transform,UnityEngine.Vector3,System.Func{UnityEngine.TextMesh,UnityEngine.TextMesh},int,bool)"/>
        public static TextMesh SpawnTextOnWorld( Vector3 position, string text, int fontSize,
            Color color, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center,
            bool usingLocalPosition = false ) => SpawnTextOnWorld( null, position, text, fontSize, color, textAnchor, textAlignment, 0, usingLocalPosition );

        /// <inheritdoc cref="SpawnTextOnWorld(UnityEngine.Transform,UnityEngine.Vector3,System.Func{UnityEngine.TextMesh,UnityEngine.TextMesh},int,bool)"/>
        public static TextMesh SpawnTextOnWorld( Transform parent,
            Vector3 position, string text, int fontSize = 30, Color color = default, TextAnchor textAnchor = TextAnchor.MiddleCenter,
            TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0, bool usingLocalPosition = false ) =>
            SpawnTextOnWorld( parent, position,
                txtMesh => SetTextMesh( txtMesh, text, fontSize, color, textAnchor, textAlignment ), sortingOrder, usingLocalPosition );

        static TextMesh SetTextMesh( TextMesh txtMesh, string text, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment )
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
