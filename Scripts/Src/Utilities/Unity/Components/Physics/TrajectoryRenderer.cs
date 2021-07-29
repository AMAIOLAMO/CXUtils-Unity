using System.Collections.Generic;
using UnityEngine;

namespace CXUtils.HelperComponents
{
    ///<summary> A Trajectory Renderer for rendering trajectories on x and y position </summary>
    [AddComponentMenu( "CXUtils/Physics/TrajectoryRenderer" )]
    public class TrajectoryRenderer : MonoBehaviour
    {
        float CalculateFormulaY( float currentX )
        {
            float formulaLeft = currentX * Mathf.Tan( initA );
            float formulaRightUp = aDueToGravity * currentX * currentX;
            float formulaRightDown = 2 * initV * initV * ( 1f - Mathf.Cos( 2 * initA ) / 2f );
            float formulaRight = formulaRightUp / formulaRightDown;
            return formulaLeft - formulaRight;
        }
        
        #region Variables

        [Header( "Requirements" )]
        public LineRenderer lineRenderer;

        [Header( "Starting position" )]
        public Vector3 startPosition = Vector3.zero;

        [Header( "Acceleration due to gravity" )]
        public float aDueToGravity = 1f;

        [Header( "Initial velocity" )]
        public float initV = 1f;

        [Header( "Initial angle" )]
        public float initA = 45f;

        [Header( "Others" )]
        public bool enableDebugMode;

        #endregion

        #region Script Methods

        /// <summary> Set's the trajectory </summary>
        /// <param name="renderer">The Line Renderer in use</param>
        /// <param name="startPosition">Starting position of the trajectory</param>
        /// <param name="accelerationDueToGravity">the acceleration due to gravity</param>
        /// <param name="initialVelocity">the initial velocity of the trajectory</param>
        /// <param name="angleOfInitVelocityFromHorizontalPos">the angle of the position</param>
        public void Set( LineRenderer renderer, Vector3 startPosition,
            float accelerationDueToGravity, float initialVelocity = 1f,
            float angleOfInitVelocityFromHorizontalPos = 0f )
        {
            ( lineRenderer, aDueToGravity ) = ( renderer, accelerationDueToGravity );

            ( initV, initA ) = ( initialVelocity, angleOfInitVelocityFromHorizontalPos );

            this.startPosition = startPosition;
        }

        /// <summary> Start drawing the trajectory </summary>
        /// <param name="lineLength">This is the total line length of the trajectory</param>
        /// <param name="step">This will make the trajectory more and more high resolution</param>
        public void DrawTrajectoryArc( float lineLength, float step = .5f )
        {
            //list of vectors to store the points of the trajectory
            var positions = new List<Vector3>();

            //clear
            lineRenderer.positionCount = 0;

            //draws the trajectory arc
            for ( float x = 0; x < lineLength; x += step )
            {
                //inside this for loop every iteration we calculate the whole formula
                float newY = CalculateFormulaY( x );
                var newPos = startPosition + new Vector3( x, newY );

                positions.Add( newPos );
            }

            //apply
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions( positions.ToArray() );

            //debug
            if ( enableDebugMode ) Debug.Log( $"[{name}: DebugMode] Finish!" );

        }

        #endregion
    }
}
