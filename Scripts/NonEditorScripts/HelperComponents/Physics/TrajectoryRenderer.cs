using System.Collections.Generic;
using UnityEngine;
namespace CXUtils.HelperComponents
{
    ///<summary> A Trajectory Renderer for renderering trajectories on x and y position </summary>
    [AddComponentMenu("CXUtils/Physics/TrajectoryRenderer")]
    public class TrajectoryRenderer : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        public LineRenderer lineRenderer;

        [Header("Starting position")]
        public Vector3 StartPosition = Vector3.zero;

        [Header("Acceleration due to gravity")]
        public float ADueToGravity = 1f;

        [Header("Initial velocity")]
        public float InitV = 1f;

        [Header("Initial angle")]
        public float InitA = 45f;

        [Header("Others")]
        public bool enableDebugMode = false;
        #endregion

        #region Script Methods
        /// <summary> Set's the trajectory </summary>
        /// <param name="lineRenderer">The Line Renderer in use</param>
        /// <param name="startingPosition">Starting position of the trajector</param>
        /// <param name="accelerationDueToGravity">the acceleration due to gravity</param>
        /// <param name="initialialVelocity">the initial velocity of the trajectory</param>
        /// <param name="angleOfInitVelocityFromHorizontalPos">the angle of the position</param>
        public void SetTrajectory(LineRenderer lineRenderer, Vector3 startingPosition,
            float accelerationDueToGravity, float initialialVelocity = 1f,
            float angleOfInitVelocityFromHorizontalPos = 0f)
        {
            (this.lineRenderer, this.ADueToGravity) =
                (lineRenderer, accelerationDueToGravity);

            (this.InitV, this.InitA) =
                (initialialVelocity, angleOfInitVelocityFromHorizontalPos);

            StartPosition = startingPosition;
        }

        /// <summary> Start drawing the trajectory </summary>
        /// <param name="lineLength">This is the total line length of the trajectory</param>
        /// <param name="step">This will make the tractory more and more high resolution</param>
        public void DrawTrajectoryArc(float lineLength, float step = .5f)
        {
            //list of vects to store the points of the trajectory
            List<Vector3> positions = new List<Vector3>();

            //clear
            lineRenderer.positionCount = 0;

            //draws the trajectory arc
            for (float x = 0; x < lineLength; x += step)
            {
                //inside this for loop every iteration we calculate the whole formula
                float newY = CalculateFormulaY(x);
                Vector3 newPos = StartPosition + new Vector3(x, newY);

                positions.Add(newPos);
            }

            //apply
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());

            //debug
            if (enableDebugMode)
                Debug.Log($"[{name}: DebugMode] Finish!");

        }
        #endregion

        #region Script Helper Utils
        private float CalculateFormulaY(float currentX)
        {
            float FormulaLeft = currentX * Mathf.Tan(InitA);
            float FormulaRightUp = ADueToGravity * currentX * currentX;
            float FormulaRightDown = 2 * InitV * InitV * (1 - Mathf.Cos(2 * InitA) / 2);
            float FormulaRight = FormulaRightUp / FormulaRightDown;
            return FormulaLeft - FormulaRight;
        }
        
        #endregion
    }
}
