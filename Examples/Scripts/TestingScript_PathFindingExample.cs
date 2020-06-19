using UnityEngine;
using UnityEngine.UI;
using CXUtils.CodeUtils;
using System.Collections.Generic;
using CXUtils.GridSystem.PathFinding;

namespace CXUtils.Test
{
    //disabling IDE0044 warning (because this is monobehaviour)
#pragma warning disable IDE0044
    public class TestingScript_PathFindingExample : MonoBehaviour
    {
        #region Fields
        [SerializeField] int width = default;
        [SerializeField] int height = default;

        PathFinding pathFinder;
        List<Vector3> debugVisuals = new List<Vector3>();

        [SerializeField] Color GridBGColor = default;
        [SerializeField] Text debugText = default;
        [SerializeField] Text cutCornerText = default;
        [SerializeField] GameObject spawningEffect = default;

        [SerializeField] Vector2Int startGridPosition = default;

        [Range(0f, 1f)]
        [SerializeField] float scale = .4f;

        [Range(0f, 1f)]
        [SerializeField] float threshHold = .5f;
        private bool usingDiagonal = true;
        private bool cuttingCorners = false;

        CameraShake camShake;
        Camera mainCam;

        #endregion

        #region Main Threads

        private void Start()
        {
            mainCam = Camera.main;
            camShake = new CameraShake(mainCam.transform, 0.1f);
            pathFinder = new PathFinding(width, height, 1, Vector3.zero, GridSystem.GridDimentionOptions.XZ);
            debugVisuals.Clear();

            startGridPosition = Vector2Int.zero;

            //clear text
            WriteUseDiagonalText($"UsingDiagonal: {usingDiagonal}");
            WriteUseCutCornersText($"Cutting corners: {cuttingCorners}");
        }

        void Update()
        {
            CheckInputs();
        }

        private void OnDrawGizmos()
        {
            pathFinder = new PathFinding(width, height, 1, Vector3.zero, GridSystem.GridDimentionOptions.XZ);
            Gizmos.color = GridBGColor;
            Bounds worldBounds = pathFinder.Grid.GetWorldBounds();

            Gizmos.DrawCube(worldBounds.center, worldBounds.size);

            DrawUnWalkableDebugVisuals();
            DrawStartingPositionDebug();
            pathFinder.Grid.DrawDebug(Color.red, Color.white, 0.1f);
        }

        #endregion

        #region Script Utils

        private void CheckInputs()
        {
            if (Input.GetMouseButtonDown(0))
                DrawPathFinderDebug();

            //diagonal and cut corners
            if (Input.GetKeyDown(KeyCode.T))
            {
                usingDiagonal = !usingDiagonal;
                WriteUseDiagonalText($"UsingDiagonal: {usingDiagonal}");
                if (usingDiagonal)
                    WriteUseCutCornersText($"Cutting corners: {cuttingCorners}");
                else
                    WriteUseCutCornersText();
            }
            //cut corners
            if (Input.GetKeyDown(KeyCode.C) && usingDiagonal)
            {
                cuttingCorners = !cuttingCorners;
                WriteUseCutCornersText($"Cutting corners: {cuttingCorners}");
            }

            if (debugVisuals.Count > 0 && Input.GetKeyDown(KeyCode.R))
                RemoveAllBlock();

            if (Input.GetKeyDown(KeyCode.Y))
                ProceduralGenerateWalls(scale, threshHold);

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
            {
                SetStartingPosition();
            }

            else if (Input.GetMouseButtonDown(1))
                IsWalkable_Add_Remove();
        }

        private void DrawPathFinderDebug()
        {
            Vector3 mousePos = CameraUtils.GetMouseOnWorldPos();

            if (pathFinder.Grid.TryGetGridPosition(mousePos, out Vector2Int gridPos))
            {
                List<PathNode> path;

                if (usingDiagonal)
                    path = pathFinder.FindPath_Diagonal(startGridPosition, gridPos,
                        cuttingCorners ? PathFindingOptions.Normal_CutCorners : PathFindingOptions.Normal);

                else
                    path = pathFinder.FindPath_Straight(startGridPosition, gridPos);

                //have path
                if (path != null)
                {
                    //simplifying the path for less drawing lines (because it will be annoying)
                    path = pathFinder.SimplifyPath(path);



                    pathFinder.DrawLineDebug(path, 5f, Color.red);
                }

            }
        }

        private void IsWalkable_Add_Remove()
        {
            Vector3 mousePos = CameraUtils.GetMouseOnWorldPos();

            if (pathFinder.Grid.TryGetValue(mousePos, out PathNode pathNode))
            {
                if (!pathNode.GridPosition.Equals(startGridPosition))
                {
                    //and reverse it
                    pathNode.isWalkable = !pathNode.isWalkable;

                    Vector3 currentPos = pathFinder.Grid.GetWorldPosition(pathNode.GridPosition) + pathFinder.Grid.CellCenterOffset;

                    if (!pathNode.isWalkable)
                        debugVisuals.Add(currentPos);

                    else if (debugVisuals.Contains(currentPos))
                        debugVisuals.Remove(currentPos);
                }
            }
        }

        private void DrawUnWalkableDebugVisuals()
        {
            if (debugVisuals.Count == 0)
                return;

            //else
            foreach (var vector in debugVisuals)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(vector, Vector3.one * pathFinder.Grid.CellSize);
            }
        }

        private void DrawStartingPositionDebug()
        {
            if (startGridPosition != null)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawCube(
                    pathFinder.Grid.GetWorldPosition(startGridPosition) + pathFinder.Grid.CellCenterOffset,
                    Vector3.one * pathFinder.Grid.CellSize
                    );
            }
        }

        private void RemoveAllBlock()
        {

            foreach (var vect in debugVisuals)
                Instantiate(spawningEffect, vect, mainCam.transform.rotation, transform);


            camShake.StartShake(this, mainCam.transform.position, .5f);
            pathFinder.Grid.MapValues((x, y) => new PathNode(x, y));

            debugVisuals.Clear();
        }

        private void ProceduralGenerateWalls(float scale = .4f, float threshHold = .6f, float? seed = null)
        {
            //Reset the walls
            if (debugVisuals.Count > 0)
                RemoveAllBlock();

            threshHold = Mathf.Clamp01(threshHold);

            float currentSeed = seed ?? Random.Range(0f, 1f);

            pathFinder.Grid.MapValues
            (
                (x, y) =>
                {
                    bool isWalkable = !MathUtils.PerlinNoise_FlipCoin(new Vector2(x, y), scale, threshHold, seed);
                    
                    Vector3 currentPosition = pathFinder.Grid.GetWorldPosition(x, y) + pathFinder.Grid.CellCenterOffset;

                    if (!isWalkable && !(new Vector2Int(x, y).Equals(startGridPosition)))
                        debugVisuals.Add(currentPosition);

                    return new PathNode(x, y, isWalkable);
                }
            );
        }

        private void SetStartingPosition()
        {
            Vector2 mousePosition = CameraUtils.GetMouseOnWorldPos();
            if (pathFinder.Grid.TryGetGridPosition(mousePosition, out Vector2Int gridPosition))
            {
                //check if is walkable, then set
                if (pathFinder.GetIsWalkable(gridPosition))
                    startGridPosition = gridPosition;
            }
        }

        private void WriteUseDiagonalText(string text = null) =>
            debugText.text = text;

        private void WriteUseCutCornersText(string text = null) =>
            cutCornerText.text = text;

        #endregion
    }
}