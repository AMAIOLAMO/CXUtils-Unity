using System.Collections.Generic;
using UnityEngine;

///<summary> This is the trajectory renderer for the monobehaviour </summary>
public class TrajectoryRenderer : MonoBehaviour
{
    #region Variables
    [Header("Requirements")]
    public LineRenderer lineRenderer;
    [Header("Things Needed")]
    public Vector3 startingPos = Vector3.zero;
    public float accelerationDueToGravity = 1f;
    public float initialialVelocity = 1f;
    public float angleOfInitVelocityFromHorizontalPos = 45f;

    [Header("Others")]
    public bool enableDebugMode = false;
    #endregion

    #region MainMethod
    /// <summary> Set's the trajectory </summary>
    /// <param name="lineRenderer">The Line Renderer in use</param>
    /// <param name="startingPosition">Starting position of the trajector</param>
    /// <param name="accelerationDueToGravity">the acceleration due to gravity</param>
    /// <param name="initialialVelocity">the initial velocity of the trajectory</param>
    /// <param name="angleOfInitVelocityFromHorizontalPos">the angle of the position</param>
    public void SetTrajectory(LineRenderer lineRenderer, Vector3 startingPosition, float accelerationDueToGravity,
    float initialialVelocity = 1f, float angleOfInitVelocityFromHorizontalPos = 0f)
    {
        //this method will set the trajectory things

        (this.lineRenderer, this.accelerationDueToGravity) =
            (lineRenderer, accelerationDueToGravity);

        (this.initialialVelocity, this.angleOfInitVelocityFromHorizontalPos) =
            (initialialVelocity, angleOfInitVelocityFromHorizontalPos);

        startingPos = startingPosition;
    }

    /// <summary> Start drawing the trajectory </summary>
    /// <param name="lineLength">This is the total line length of the trajectory</param>
    /// <param name="xAddStep">This will make the tractory more and more high resolution</param>
    public void DrawTrajectoryArc(float lineLength, float xAddStep = .5f)
    {

        //make a list of vars
        List<Vector3> positions = new List<Vector3>();
        //clear
        lineRenderer.positionCount = 0;

        //this method will draw the trajectory arc using the variables
        for (float x = 0; x < lineLength; x += xAddStep)
        {
            //inside this for loop we will use the line renderer's things
            float FormulaLeft = x * Mathf.Tan(angleOfInitVelocityFromHorizontalPos);
            float FormulaRightUp = accelerationDueToGravity * x * x;
            float FormulaRightDown = 2 * initialialVelocity * initialialVelocity * (1 - Mathf.Cos(2 * angleOfInitVelocityFromHorizontalPos) / 2);
            float FormulaRight = FormulaRightUp / FormulaRightDown;
            float newY = FormulaLeft - FormulaRight;

            positions.Add(startingPos + new Vector3(x, newY));
        }

        //when finishing calculating // add the things to the linerenderer
        Vector3[] FinPoses = positions.ToArray();
        //apply
        lineRenderer.positionCount = FinPoses.Length;
        lineRenderer.SetPositions(FinPoses);

        //debug log
        if (enableDebugMode)
            Debug.Log($"[{name}: DebugMode] Finish!");

    }
    #endregion
}
