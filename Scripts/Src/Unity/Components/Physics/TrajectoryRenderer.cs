using System.Collections.Generic;
using CXUtils.Common;
using UnityEngine;

namespace CXUtils.Components
{
    /// <summary>
    ///     A Trajectory Renderer for rendering trajectories on x and y position
    /// </summary>
    [AddComponentMenu( "CXUtils/Physics/" + nameof( TrajectoryRenderer ) )]
    public class TrajectoryRenderer : MonoBehaviour
    {
        float CalculateY( float x )
        {
            float formulaLeft = x * MathUtils.Tan( _initialAcceleration );
            float formulaRightUp = _accelerationDueGravity * x * x;
            float formulaRightDown = 2 * _initVelocity * _initVelocity * ( 1f - MathUtils.Cos( 2f * _initialAcceleration ) / 2f );
            float formulaRight = formulaRightUp / formulaRightDown;
            return formulaLeft - formulaRight;
        }

        #region Variables

        [Header( "Requirements" )]
        [SerializeField] LineRenderer _lineRenderer;

        [Header( "Starting position" )]
        [SerializeField] Vector3 _startPosition = Vector3.zero;

        [Header( "Acceleration due to gravity" )]
        [SerializeField] float _accelerationDueGravity = 1f;

        [Header( "Initial velocity" )]
        [SerializeField] float _initVelocity = 1f;

        [Header( "Initial angle" )]
        [SerializeField] float _initialAcceleration = 45f;

        [Header( "Others" )]
        [SerializeField] bool _debugMode;

        readonly List<Vector3> _positions = new List<Vector3>();

        #region Properties

        public LineRenderer LineRenderer
        {
            get => _lineRenderer;
            set => _lineRenderer = value;
        }
        public Vector3 StartPosition
        {
            get => _startPosition;
            set => _startPosition = value;
        }
        public float AccelerationDueGravity
        {
            get => _accelerationDueGravity;
            set => _accelerationDueGravity = value;
        }
        public float InitVelocity
        {
            get => _initVelocity;
            set => _initVelocity = value;
        }
        public float InitialAcceleration
        {
            get => _initialAcceleration;
            set => _initialAcceleration = value;
        }
        public bool DebugMode
        {
            get => _debugMode;
            set => _debugMode = value;
        }

        #endregion

        #endregion

        #region Script Methods

        /// <summary>
        ///     Start drawing the trajectory
        /// </summary>
        /// <param name="lineLength">This is the total line length of the trajectory</param>
        /// <param name="step">This will make the trajectory more and more high resolution</param>
        public void DrawTrajectoryArc( float lineLength, float step = .5f )
        {
            _positions.Clear();

            //clear
            _lineRenderer.positionCount = 0;

            //draws the trajectory arc
            CalculatePositions( lineLength, step );

            //apply
            _lineRenderer.positionCount = _positions.Count;
            _lineRenderer.SetPositions( _positions.ToArray() );

            //debug
            #if UNITY_EDITOR
            if ( _debugMode ) Debug.Log( $"[{name}: DebugMode] Finish!" );
            #endif
        }

        void CalculatePositions( float lineLength, float step )
        {
            for ( float x = 0; x < lineLength; x += step )
            {
                //inside this for loop every iteration we calculate the whole formula
                float newY = CalculateY( x );
                var newPos = _startPosition + new Vector3( x, newY );

                _positions.Add( newPos );
            }
        }

        #endregion
    }
}
