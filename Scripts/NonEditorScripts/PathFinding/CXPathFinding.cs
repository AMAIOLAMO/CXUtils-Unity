using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CXUtils.GridSystem;
using System;

namespace CXUtils.GridSystem.PathFinding
{
    public class CXPathNode
    {
        #region Fields
        private CXGrid<CXPathNode> grid;

        public int x { get; }
        public int y { get; }
        #region Cost fields
        /// <summary> Distance from starting node </summary>
        public int GCost { get; set; }

        /// <summary> Distance from end node </summary>
        public int HCost { get; set; }

        /// <summary> Combined distance (GCost + HCost) </summary>
        public int FCost { get; set; }
        #endregion

        /// <summary> The node that this came from (Also called: Parent node) </summary>
        public CXPathNode CameFromNode { get; set; }
        #endregion

        public CXPathNode(CXGrid<CXPathNode> grid, int x, int y)
        {
            this.grid = grid;
            (this.x, this.y) = (x, y);
        }

        public void CalculateFCost() =>
            FCost = GCost + FCost;

        public override string ToString() =>
            $"({x}, {y})";
    }

    /// <summary> A library that implements A* path finding </summary>
    public struct CXPathFinding
    {
        #region Constants
        private const int STRAIGHT_COST = 10; // 1 * 10 = 10
        private const int DIAGONAL_COST = 14; // sqrt(1 * 10^2 + 1 * 10^2) = sqrt(200) = 14
        #endregion

        public CXGrid<CXPathNode> Grid { get; private set; }

        #region Constructors
        public CXPathFinding(int width, int height, float cellSize, Vector2 originPosition) =>
            Grid = new CXGrid<CXPathNode>(width, height, cellSize, originPosition,
                (g, x, y) => new CXPathNode(g, x, y));
        #endregion

        #region Find Path A* Algorithm
        public List<Vector2> FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            if (Grid.TryGetGridPosition(startPosition, out Vector2Int startGridPos) &&
               Grid.TryGetGridPosition(endPosition, out Vector2Int endGridPos))
            {
                List<CXPathNode> path = FindPath(startGridPos, endGridPos);
                //found path
                if (path != null)
                {
                    List<Vector2> vectPath = new List<Vector2>();

                    foreach (var pathNode in path)
                        vectPath.Add(Grid.GetWorldPosition(pathNode.x, pathNode.y));
                }
            }
            //no path or get grid position wrong
            return null;
        }

        public List<CXPathNode> FindPath(Vector2Int startPosition, Vector2Int endPosition)
        {
            CXPathNode startNode = Grid.GetValue(startPosition.x, startPosition.y);
            CXPathNode endNode = Grid.GetValue(endPosition.x, endPosition.y);

            List<CXPathNode> openList = new List<CXPathNode>() { startNode };
            List<CXPathNode> closeList = new List<CXPathNode>();

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    CXPathNode pathNode = Grid.GetValue(x, y);
                    pathNode.GCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.CameFromNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistance(startNode, endNode);
            startNode.CalculateFCost();


            while (openList.Count > 0)
            {
                CXPathNode currentNode = GetLowestPathNode(openList);

                if (currentNode == endNode)
                    return CalculatePath(endNode);

                //else
                openList.Remove(currentNode);
                closeList.Add(currentNode);

                //looping through the neighbours and find the lowest FCost
                foreach (var neighbourNode in GetNeighborNodes(currentNode))
                {
                    //That means it is no longer needed to be searched
                    if (closeList.Contains(neighbourNode))
                        continue;

                    int tentativeGCost =
                        currentNode.GCost + CalculateDistance(currentNode, neighbourNode);

                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.CameFromNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistance(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                            openList.Add(neighbourNode);

                    }
                }
            }

            //out of nodes of the open list
            return null;
        }
        #endregion

        #region Script Methods

        private int CalculateDistance(CXPathNode a, CXPathNode b)
        {
            int xDis = Mathf.Abs(a.x - b.x);
            int yDis = Mathf.Abs(a.y - b.y);

            int rem = Mathf.Abs(xDis - yDis);

            return DIAGONAL_COST * Mathf.Min(xDis, yDis) + STRAIGHT_COST * rem;
        }

        private CXPathNode GetLowestPathNode(List<CXPathNode> pathNodeList)
        {
            //Initially is the start node
            CXPathNode lowestPathNode = pathNodeList[0];

            //Bubble find the lowest path node
            for (int i = 1; i < pathNodeList.Count; i++)
                if (pathNodeList[i].FCost < lowestPathNode.FCost)
                    lowestPathNode = pathNodeList[i];

            return lowestPathNode;
        }

        private List<CXPathNode> CalculatePath(CXPathNode endNode)
        {
            List<CXPathNode> path = new List<CXPathNode>();
            path.Add(endNode);
            CXPathNode currentNode = endNode;

            //while not the start node
            while (currentNode.CameFromNode != null)
            {
                path.Add(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }
            // Reverse the path to the correct direction
            // because this is traced backwards
            path.Reverse();
            return path;
        }

        private List<CXPathNode> GetNeighborNodes(CXPathNode currentNode)
        {
            List<CXPathNode> neightbourNodes = new List<CXPathNode>();

            if (currentNode.x - 1 >= 0)
            {
                //Left
                neightbourNodes.Add(Grid.GetValue(currentNode.x - 1, currentNode.y));
                //Left Down
                if (currentNode.y - 1 >= 0)
                    neightbourNodes.Add(Grid.GetValue(currentNode.x - 1, currentNode.y - 1));
                //Left Up
                if (currentNode.y + 1 < Grid.Height)
                    neightbourNodes.Add(Grid.GetValue(currentNode.x - 1, currentNode.y + 1));
            }

            if (currentNode.x + 1 >= 0)
            {
                //Right
                neightbourNodes.Add(Grid.GetValue(currentNode.x + 1, currentNode.y));
                //Right Down
                if (currentNode.y - 1 >= 0)
                    neightbourNodes.Add(Grid.GetValue(currentNode.x + 1, currentNode.y - 1));
                //Right Up
                if (currentNode.y + 1 < Grid.Height)
                    neightbourNodes.Add(Grid.GetValue(currentNode.x + 1, currentNode.y + 1));
            }
            //Down
            if (currentNode.y - 1 >= 0)
                neightbourNodes.Add(Grid.GetValue(currentNode.x, currentNode.y - 1));
            //Up
            if (currentNode.y + 1 < Grid.Height)
                neightbourNodes.Add(Grid.GetValue(currentNode.x, currentNode.y + 1));

            return neightbourNodes;
        }
        #endregion
    }
}