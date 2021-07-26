using System.Collections.Generic;
using System.Diagnostics;

namespace CXUtils.Types
{
    /// <summary>
    ///     A class that handles all spline calculations
    /// </summary>
    public class Spline2D
    {
        readonly List<Float2> _points;

        public Spline2D( List<Float2> points, bool isLoop = false )
        {
            Debug.Assert( points.Count > 3, nameof( points ) + " must have at least 4 points to create a spline!" );

            _points = points;
            SetLoop( isLoop );
        }

        public Spline2D( Float2 center, bool isLoop = false )
        {
            _points = new List<Float2>
            {
                center + Float2.NegX,
                center + ( Float2.NegX + Float2.PosY ).Halve,
                center + ( Float2.PosX + Float2.NegY ).Halve,
                center + Float2.PosX
            };

            SetLoop( isLoop );
        }
        public bool IsLoop { get; private set; }

        public Float2 this[ int index ] => _points[index];

        public int Count => _points.Count;
        public int SegmentCount => _points.Count / 3;
        public void SetLoop( bool isLoop )
        {
            if ( IsLoop == isLoop ) return;

            //else changed
            IsLoop = isLoop;

            DoLoopChanged();
        }

        public void ToggleLoop()
        {
            IsLoop = !IsLoop;

            DoLoopChanged();
        }

        public Float2[] GetSegmentRaw( int i )
        {
            int firstIndex = i * 3;

            return new[] { _points[firstIndex], _points[firstIndex + 1], _points[firstIndex + 2], _points[LoopIndex( firstIndex + 3 )] };
        }

        public CubicBezier2D GetSegment( int i ) => new CubicBezier2D( GetSegmentRaw( i ) );

        public void PushAnchor( Float2 anchor )
        {
            _points.Add( _points[_points.Count - 1] * 2f - _points[_points.Count - 2] ); // last control point's other side control point
            _points.Add( ( _points[_points.Count - 1] + anchor ).Halve ); // the new anchor's new control point
            _points.Add( anchor ); // finally the new anchor
        }

        /// <summary>
        ///     Moves a point in the spline statically (which means it doesn't affect other points)
        /// </summary>
        public void MoveStatic( int index, Float2 position ) => _points[index] = position;

        /// <summary>
        ///     Moves a point in the spline but move the controls also
        /// </summary>
        public void MoveWithControls( int index, Float2 position )
        {

        }

        /// <summary>
        ///     Is this index a control point?
        /// </summary>
        public static bool IsControl( int i ) => i % 3 != 0;

        /// <summary>
        ///     Is this index an anchor point?
        /// </summary>
        public static bool IsAnchor( int i ) => i % 3 == 0;

        void DoLoopChanged()
        {
            if ( IsLoop )
            {
                _points.Add( _points[_points.Count - 1] * 2f - _points[_points.Count - 2] ); // last control point's other side control point
                _points.Add( _points[0] * 2f - _points[1] ); // first control point's other side control point
            }
            else
            {
                //remove the last two control points
                _points.RemoveRange( _points.Count - 2, 2 );
            }
        }

        int LoopIndex( int i ) =>
            //if we got negative values, this will wrap values around and if over values then we can wrap around to 0 again
            ( i + _points.Count ) % _points.Count;
    }
}
