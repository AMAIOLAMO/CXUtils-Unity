using CXUtils.Domain.Types;
using UnityEngine;

namespace CXUtils.Common
{
    /// <summary>
    ///     An utility for drawing directly using GL
    /// </summary>
    public static class GLUtils
    {
        /// <summary>
        ///     Submits a vertex
        /// </summary>
        public static void Vertex2( Float2 value ) => GL.Vertex3( value.x, value.y, 0f );

        /// <inheritdoc cref="Vertex2" />
        public static void Vertex3( Float3 value ) => GL.Vertex3( value.x, value.y, value.z );

        /// <summary>
        ///     Submits two vertices / points to GL
        /// </summary>
        public static void DrawLineRaw( Float2 a, Float2 b )
        {
            Vertex2( a );
            Vertex2( b );
        }

        public static void Begin( int mode, Material material, Color color )
        {
            GL.Begin( mode );
            material.SetPass( 0 );
            GL.Color( color );
        }

        public static void DrawLine( Float2 a, Float2 b, Color color, Material material )
        {
            Begin( GL.LINES, material, color );
            DrawLineRaw( a, b );
            GL.End();
        }

        public static void DrawWireBoxRaw( Float2 origin, Float2 size )
        {
            var end = origin + size;

            DrawLineRaw( origin, new Float2( end.x, origin.y ) );
            DrawLineRaw( origin, new Float2( origin.x, end.y ) );

            DrawLineRaw( end, new Float2( origin.x, end.y ) );
            DrawLineRaw( end, new Float2( end.x, origin.y ) );
        }

        public static void DrawWireBox( Float2 origin, Float2 size, Color color, Material material )
        {
            Begin( GL.LINES, material, color );
            DrawWireBoxRaw( origin, size );
            GL.End();
        }
    }
}
