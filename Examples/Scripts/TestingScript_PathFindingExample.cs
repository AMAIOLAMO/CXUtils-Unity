using UnityEngine;
using UnityEngine.UI;
using CXUtils.CodeUtils;
using System.Collections.Generic;
using CXUtils.GridSystem.PathFinding;

namespace CXUtils.Test
{
    //disabling IDE0044 warning (because this is monobehav)
#pragma warning disable IDE0044
    public class TestingScript_PathFindingExample : MonoBehaviour
    {
        CXPathFinding pathFinder = new CXPathFinding(20, 20, 1, Vector2.zero);
        List<Vector2> debugVisuals = new List<Vector2>();

        [SerializeField] Text debugText = default;
        [SerializeField] GameObject spawningEffect = default;

        [Range(0f, 1f)]
        [SerializeField] float scale = .4f;

        [Range(0f, 1f)]
        [SerializeField] float threshHold = .5f;
        private bool usingDiagonal = true;

        CameraShake camShake;
        Camera mainCam;

        private void Start()
        {
            mainCam = Camera.main;
            camShake = new CameraShake(mainCam.transform, 0.1f);

            debugVisuals.Clear();

            //clear text
            WriteText($"UsingDiagonal: {usingDiagonal}");
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                DrawPathFinderDebug();

            if (Input.GetMouseButtonDown(1))
                IsWalkable_Add_Remove();

            if (Input.GetKeyDown(KeyCode.T))
            {
                usingDiagonal = !usingDiagonal;
                WriteText($"UsingDiagonal: {usingDiagonal}");
            }

            if (debugVisuals.Count > 0 && Input.GetKeyDown(KeyCode.R))
                RemoveAllBlock();

            if (Input.GetKeyDown(KeyCode.Y))
                ProceduralGenerateWalls(scale, threshHold);

        }

        private void OnDrawGizmos()
        {
            DrawDebugVisuals();
            pathFinder.Grid.DrawDebug(Color.black, Color.white, 0.1f);
        }

        #region Script Utils

        private void DrawPathFinderDebug()
        {
            Vector2 mousePos = CameraUtils.GetMouseOnWorldPos();

            if (pathFinder.Grid.TryGetGridPosition(mousePos, out Vector2Int gridPos))
            {
                List<PathNode> path;

                if (usingDiagonal)
                    path = pathFinder.FindPath_Diagonal(new Vector2Int(0, 0), gridPos);

                else
                    path = pathFinder.FindPath_Straight(new Vector2Int(0, 0), gridPos);

                //have path
                if (path != null)
                    pathFinder.DrawLineDebug(path, 5f, Color.red);

            }
        }

        private void IsWalkable_Add_Remove()
        {
            Vector2 mousePos = CameraUtils.GetMouseOnWorldPos();

            //if get grid position
            if (pathFinder.Grid.TryGetGridPosition(mousePos, out Vector2Int gridPos))
                //try to get the grid value
                if (pathFinder.Grid.TryGetValue(gridPos, out PathNode pathNode))
                {
                    //and reverse it
                    pathNode.isWalkable = !pathNode.isWalkable;

                    Vector2 currentPos = pathFinder.Grid.GetWorldPosition(gridPos) + Vector2.one * pathFinder.Grid.CellSize * .5f;

                    if (!pathNode.isWalkable)
                        debugVisuals.Add(currentPos);

                    else if (debugVisuals.Contains(currentPos))
                        debugVisuals.Remove(currentPos);
                }
        }

        private void DrawDebugVisuals()
        {
            if (debugVisuals.Count == 0)
                return;

            //else
            foreach (var vector in debugVisuals)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(vector, Vector2.one * pathFinder.Grid.CellSize);
            }
        }

        private void RemoveAllBlock()
        {
            foreach (var vect in debugVisuals)
                Instantiate(spawningEffect, vect, Quaternion.identity, transform);

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
                    bool isWalkable = !MathUtils.PerlinNoise(new Vector2(x, y), scale, threshHold, seed);

                    Vector2 currentPosition = pathFinder.Grid.GetWorldPosition(x, y) + pathFinder.Grid.CellCenterOffset;

                    if (!isWalkable)
                        debugVisuals.Add(currentPosition);

                    return new PathNode(x, y, isWalkable);
                }
            );
        }

        private void WriteText(string text = null) =>
            debugText.text = text;
        #endregion
    }
}