using System.Collections.Generic;
using System.Diagnostics;

namespace CXUtils.Types
{
    /// <summary>
    ///     The base of all spline classes<br />
    ///     DO NOT INHERIT THIS CLASS UNLESS YOU KNOW WHAT YOU ARE DOING
    /// </summary>
    public abstract class SplineBase<T>
    {
        protected readonly List<T> points;
        public SplineBase( List<T> points, bool isLoop = false )
        {
            Debug.Assert( points.Count > 3, nameof( points ) + " must have at least 4 points to create a spline!" );

            this.points = points;
            SetLoop( isLoop );
        }
        public SplineBase() => points = new List<T>();
        public bool IsLoop { get; private set; }

        public virtual T this[ int index ] => points[index];
        public int Count => points.Count;
        public int SegmentCount => points.Count / 3;

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

        public T[] GetSegmentRaw( int i )
        {
            int firstIndex = i * 3;

            return new[] { points[firstIndex], points[firstIndex + 1], points[firstIndex + 2], points[LoopIndex( firstIndex + 3 )] };
        }

        /// <summary>
        ///     Moves a point in the spline statically (which means it doesn't affect other points)
        /// </summary>
        public virtual void MoveStatic( int i, T position ) => points[i] = position;


        //if we got negative values, this will wrap values around and if over values then we can wrap around to 0 again
        protected int LoopIndex( int i ) => ( i + points.Count ) % points.Count;

        protected abstract void DoLoopChanged();
        public abstract void PushAnchor( T anchor );
    }

    public static class SplineUtils
    {
        /// <summary>
        ///     Is this index a control point?
        /// </summary>
        public static bool IsControl( int i ) => i % 3 != 0;

        /// <summary>
        ///     Is this index an anchor point?
        /// </summary>
        public static bool IsAnchor( int i ) => i % 3 == 0;
    }

    /// <summary>
    ///     A class that handles all spline calculations
    /// </summary>
    public sealed class Spline2D : SplineBase<Float2>
    {
        public Spline2D( List<Float2> points, bool isLoop = false ) : base( points, isLoop ) { }

        public Spline2D( Float2 center, bool isLoop = false )
        {
            points.Add( center + Float2.NegX );
            points.Add( center + ( Float2.NegX + Float2.PosY ).Halve );
            points.Add( center + ( Float2.PosX + Float2.NegY ).Halve );
            points.Add( center + Float2.PosX );

            SetLoop( isLoop );
        }

        public CubicBezier2D GetSegment( int i ) => new CubicBezier2D( GetSegmentRaw( i ) );

        public override void PushAnchor( Float2 anchor )
        {
            points.Add( points[points.Count - 1] * 2f - points[points.Count - 2] ); // last control point's other side control point
            points.Add( ( points[points.Count - 1] + anchor ).Halve ); // the new anchor's new control point
            points.Add( anchor ); // finally the new anchor
        }

        /// <summary>
        ///     Moves a point in the spline but move the controls also
        /// </summary>
        public void MoveDynamic( int i, Float2 position )
        {
            var delta = position - points[i];
            points[i] = position;

            if ( SplineUtils.IsAnchor( i ) )
            {
                if ( i + 1 < points.Count || IsLoop ) points[LoopIndex( i + 1 )] += delta;
                if ( i - 1 >= 0 || IsLoop ) points[LoopIndex( i - 1 )] += delta;
            }
            else
            {
                bool nextAnchor = SplineUtils.IsAnchor( i + 1 );

                int mirroredControlIndex = nextAnchor ? i + 2 : i - 2;
                int anchorIndex = nextAnchor ? i + 1 : i - 1;

                if ( ( mirroredControlIndex < 0 || mirroredControlIndex >= points.Count ) && !IsLoop ) return;

                ( anchorIndex, mirroredControlIndex ) = ( LoopIndex( anchorIndex ), LoopIndex( mirroredControlIndex ) );

                float originalDistance = ( points[anchorIndex] - points[mirroredControlIndex] ).Magnitude;
                var direction = ( points[anchorIndex] - position ).Normalized;

                points[mirroredControlIndex] = points[anchorIndex] + direction * originalDistance;
            }
        }

        protected override void DoLoopChanged()
        {
            if ( IsLoop )
            {
                points.Add( points[points.Count - 1] * 2f - points[points.Count - 2] ); // last control point's other side control point
                points.Add( points[0] * 2f - points[1] ); // first control point's other side control point
                return;
            }
            
            //remove the last two control points (which is the loop control point)
            points.RemoveRange( points.Count - 2, 2 );
        }
    }
}
