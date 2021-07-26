using System.Collections.Generic;
using CXUtils.CodeUtils;
using UnityEngine;

namespace CXUtils.Types
{
    /// <summary>
    ///     A class that handles all spline calculations
    /// </summary>
    public class Spline2D
    {
        readonly List<Float2> _points;

        public Spline2D( List<Float2> points )
        {
            Debug.Assert( points.Count > 3, nameof( points ) + " must have at least 4 points to create a spline!" );

            _points = points;
        }

        public Spline2D( Float2 center ) => _points = new List<Float2>
        {
            center + Float2.NegX,
            center + ( Float2.NegX + Float2.PosY ).Halve,
            center + ( Float2.PosX + Float2.NegY ).Halve,
            center + Float2.PosX
        };

        public Float2 this[ int index ] => _points[index];

        public int Count => _points.Count;
        public int SegmentCount => _points.Count / 3;

        public Float2[] GetSegmentRaw( int i )
        {
            int firstIndex = i * 3;

            return new[] { _points[firstIndex], _points[firstIndex + 1], _points[firstIndex + 2], _points[firstIndex + 3] };
        }

        public CubicBezier2D GetSegment( int i ) => new CubicBezier2D( GetSegmentRaw( i ) );

        public void PushAnchor( Float2 anchor )
        {
            _points.Add( _points[_points.Count - 1] * 2f - _points[_points.Count - 2] ); // last control point's other side control point
            _points.Add( ( _points[_points.Count - 1] + anchor ).Halve );                     // the new anchor's new control point
            _points.Add( anchor );                                                            // finally the new anchor
        }

        public void MovePoint( int index, Float2 position )
        {
            _points[index] = position;
            
        }

        public readonly struct CubicBezier2D
        {
            readonly Float2[] _buffer;

            public Float2 Anchor1 => _buffer[0];
            public Float2 Control1 => _buffer[1];
            public Float2 Control2 => _buffer[2];
            public Float2 Anchor2 => _buffer[3];

            public CubicBezier2D( Float2 control1, Float2 anchor1, Float2 control2, Float2 anchor2 ) =>
                _buffer = new[] { anchor1, control1, control2, anchor2 };

            public CubicBezier2D( Float2[] points )
            {
                Debug.Assert( points.Length == 4, nameof( points ) + " must have 4 points to create a segment!" );

                _buffer = points;
            }

            public Float2 Sample( float t ) => TweenUtils.CubicBezier( _buffer[0], _buffer[1], _buffer[2], _buffer[3], t );
        }
    }
}
