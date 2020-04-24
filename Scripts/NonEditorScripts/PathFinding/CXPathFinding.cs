using System.Collections.Generic;
using UnityEngine;

namespace CXUtils.GridSystem.PathFinding
{
    #region Enum declaration
    /// <summary> Options for finding Paths </summary>
    public enum PathFindingOptions
    {
        /// <summary> The path will not jump diagonally
        /// <para>0: walkable, *: path, (): non walkable, %: end</para>
        /// <para>| % | * | 0 |</para>
        /// <para>| 0 |( )| * |</para>
        /// <para>|( )| * | 0 |</para> </summary>
        Normal,
        /// <summary> The path will ignore side and jump diagonally
        /// <para>*: path, (): non walkable, %: end</para>
        /// <para>| % | ( ) |</para>
        /// <para>| ( ) | * |</para> </summary>
        JumpDiagonal
    }
    #endregion

    /// <summary> A single path node from pathfinding algorithm </summary>
    public class PathNode
    {
        #region Fields
        public CXGrid<PathNode> Grid { get; }

        #region XY
        public readonly int x;
        public readonly int y;
        #endregion

        #region Cost fields
        /// <summary> Distance from starting node </summary>
        public int GCost { get; set; }

        /// <summary> Distance from end node </summary>
        public int HCost { get; set; }

        /// <summary> Combined distance (GCost + HCost) </summary>
        public int FCost { get; set; }
        #endregion

        public bool isWalkable = true;

        /// <summary> The node that this came from (Also called: Parent node) </summary>
        public PathNode CameFromNode { get; set; }
        #endregion

        public PathNode(CXGrid<PathNode> grid, int x, int y, bool isWalkable = true)
        {
            (Grid, this.x, this.y) = (grid, x, y);
            this.isWalkable = isWalkable;
        }

        #region Script Methods
        public void CalculateFCost() =>
            FCost = GCost + FCost;

        public override string ToString() =>
            $"({x}, {y})";
        #endregion
    }

    /// <summary> A library that implements A* path finding </summary>
    public class CXPathFinding
    {
        #region Constants

        // using int for less memory consumption
        private const int STRAIGHT_COST = 10; // 1 * 10 = 10 (straight calculation)
        private const int DIAGONAL_COST = 14; // sqrt(1 * 10^2 + 1 * 10^2) = sqrt(200) = 14 (Diagonal calculation)

        #endregion

        #region Fields
        public CXGrid<PathNode> Grid { get; private set; }

        public readonly int Width;
        public readonly int Height;
        public readonly float CellSize;
        public readonly Vector2 OriginPosition;
        #endregion

        #region Constructors

        public CXPathFinding(int width, int height, float cellSize, Vector2 originPosition)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            OriginPosition = originPosition;

            InitGrid();
        }

        #endregion

        #region Find Path A* Algorithm

        #region Find Vector Paths

        private List<Vector2> FindVectorPath(Vector2 startPosition, Vector2 endPosition,
            PathFindingOptions pathFindingOptions = PathFindingOptions.Normal, bool couldDiagonal = true)
        {
            if (Grid.TryGetGridPosition(startPosition, out Vector2Int startGridPos) &&
               Grid.TryGetGridPosition(endPosition, out Vector2Int endGridPos))
            {
                List<PathNode> path = FindPath(startGridPos, endGridPos, pathFindingOptions, couldDiagonal);
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

        #endregion

        #region Find Paths

        /// <summary> Finds a path </summary>
        private List<PathNode> FindPath(Vector2Int startPosition, Vector2Int endPosition,
            PathFindingOptions pathFindingOptions = PathFindingOptions.Normal, bool couldDiagonal = true)
        {
            ResetGrid();
            //if there is a value on that position
            if (Grid.TryGetValue(startPosition, out PathNode startNode) &&
                Grid.TryGetValue(endPosition, out PathNode endNode))
            {
                //clear the last things
                List<PathNode> openList = new List<PathNode>() { startNode };
                List<PathNode> closeList = new List<PathNode>();

                //add the starting node
                openList.Add(startNode);

                InitPathNodes();

                //Initialize start Node
                startNode.GCost = 0;
                startNode.HCost = CalculateDistance(startNode, endNode);
                startNode.CalculateFCost();


                while (openList.Count > 0)
                {
                    PathNode currentNode = GetLowestPathNode(openList);

                    //if get to the target path
                    if (currentNode == endNode)
                        return CalculatePath(endNode);

                    //else
                    openList.Remove(currentNode);
                    closeList.Add(currentNode);

                    //looping through the neighbours and find the lowest FCost
                    foreach (var neighbourNode in GetNeighborNodes(currentNode, pathFindingOptions, couldDiagonal))
                    {
                        //That means it is no longer needed to be searched
                        if (closeList.Contains(neighbourNode))
                            continue;

                        int tentativeGCost =
                            currentNode.GCost + CalculateDistance(currentNode, neighbourNode);

                        if (tentativeGCost < neighbourNode.GCost)
                        {
                            //Initialize neighbour Nodes
                            neighbourNode.CameFromNode = currentNode;
                            neighbourNode.GCost = tentativeGCost;
                            neighbourNode.HCost = CalculateDistance(neighbourNode, endNode);
                            neighbourNode.CalculateFCost();

                            if (!openList.Contains(neighbourNode))
                                openList.Add(neighbourNode);
                        }
                    }
                }
            }

            //out of nodes of the open list
            return null;
        }

        /// <summary> Finds a path with diagonal moves </summary>
        public List<PathNode> FindPath_Diagonal(Vector2Int startPosition, Vector2Int endPosition,
            PathFindingOptions pathFindingOptions = PathFindingOptions.Normal) =>
            FindPath(startPosition, endPosition, pathFindingOptions);

        /// <summary> Fins a path with no diagonal moves </summary>
        public List<PathNode> FindPath_Straight(Vector2Int startPosition, Vector2Int endPosition) =>
            FindPath(startPosition, endPosition, PathFindingOptions.Normal, false);

        #endregion

        #endregion

        #region Script Methods

        #region PathNode Caluclations

        private PathNode GetLowestPathNode(List<PathNode> pathNodeList)
        {
            //Initially is the start node
            PathNode lowestPathNode = pathNodeList[0];

            //Bubble find the lowest path node
            for (int i = 1; i < pathNodeList.Count; i++)
                if (pathNodeList[i].FCost < lowestPathNode.FCost)
                    lowestPathNode = pathNodeList[i];

            return lowestPathNode;
        }

        private List<PathNode> GetNeighborNodes(PathNode currentNode,
            PathFindingOptions pathFindingOptions, bool couldDiagonal)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            #region Indexes
            int leftIndex = currentNode.x - 1;
            int rightIndex = currentNode.x + 1;

            int downIndex = currentNode.y - 1;
            int upIndex = currentNode.y + 1;
            #endregion

            if (pathFindingOptions == PathFindingOptions.Normal)
            {
                bool? left = null;
                bool? right = null;
                bool? down = null;
                bool? up = null;

                #region Sides

                #region Horizontal
                //Left
                if (currentNode.x - 1 >= 0)
                    left = AssignIfWalkable(leftIndex, currentNode.y, neighbourList);

                //Right
                if (currentNode.x + 1 < Grid.Width)
                    right = AssignIfWalkable(rightIndex, currentNode.y, neighbourList);
                #endregion

                #region Vertical
                //Down
                if (currentNode.y - 1 >= 0)
                    down = AssignIfWalkable(currentNode.x, downIndex, neighbourList);

                //Up
                if (currentNode.y + 1 < Grid.Height)
                    up = AssignIfWalkable(currentNode.x, upIndex, neighbourList);
                #endregion

                #endregion

                #region Corners

                if (couldDiagonal)
                {
                    //left up
                    if ((left.HasValue && up.HasValue) && (up.Value || left.Value))
                        AssignIfWalkable(leftIndex, upIndex, neighbourList);

                    //left down
                    if ((left.HasValue && down.HasValue) && (down.Value || left.Value))
                        AssignIfWalkable(leftIndex, downIndex, neighbourList);

                    //right up
                    if ((right.HasValue && up.HasValue) && (up.Value || right.Value))
                        AssignIfWalkable(rightIndex, upIndex, neighbourList);

                    //right down
                    if ((right.HasValue && down.HasValue) && (down.Value || right.Value))
                        AssignIfWalkable(rightIndex, downIndex, neighbourList);
                }

                #endregion
            }

            else if (pathFindingOptions == PathFindingOptions.JumpDiagonal)
            {

                #region Sides

                #region Horizontal
                //Left
                if (currentNode.x - 1 >= 0)
                    AssignIfWalkable(leftIndex, currentNode.y, neighbourList);

                //Right
                if (currentNode.x + 1 < Grid.Width)
                    AssignIfWalkable(rightIndex, currentNode.y, neighbourList);
                #endregion

                #region Vertical
                //Down
                if (currentNode.y - 1 >= 0)
                    AssignIfWalkable(currentNode.x, downIndex, neighbourList);
                //Up
                if (currentNode.y + 1 < Grid.Height)
                    AssignIfWalkable(currentNode.x, upIndex, neighbourList);
                #endregion

                #endregion

                #region Corners
                if (couldDiagonal)
                {
                    //Left
                    if (currentNode.x - 1 >= 0)
                    {
                        //Left Up
                        if (currentNode.y + 1 < Grid.Height)
                            AssignIfWalkable(leftIndex, upIndex, neighbourList);

                        //Left Down
                        if (currentNode.y - 1 >= 0)
                            AssignIfWalkable(leftIndex, downIndex, neighbourList);
                    }

                    //Right
                    if (currentNode.x + 1 < Grid.Width)
                    {
                        //Right Up
                        if (currentNode.y + 1 < Grid.Height)
                            AssignIfWalkable(rightIndex, upIndex, neighbourList);

                        //Right Down
                        if (currentNode.y - 1 >= 0)
                            AssignIfWalkable(rightIndex, downIndex, neighbourList);
                    }
                }
                #endregion

            }

            return neighbourList;
        }

        #endregion

        #region Script Simplifiers

        private void InitPathNodes()
        {
            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    PathNode pathNode = Grid.GetValue(x, y);
                    pathNode.GCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.CameFromNode = null;
                }
            }
        }

        private bool AssignIfWalkable(int x, int y, List<PathNode> neightbourList)
        {
            PathNode pathNode = pathNode = Grid.GetValue(x, y);
            if (pathNode.isWalkable)
                neightbourList.Add(pathNode);

            return pathNode.isWalkable;
        }

        private void InitGrid() =>
            Grid = new CXGrid<PathNode>(Width, Height, CellSize, OriginPosition,
                (g, x, y) => new PathNode(g, x, y));

        private void ResetGrid()
        {
            Grid = new CXGrid<PathNode>(Width, Height, CellSize, OriginPosition,
                (g, x, y) => new PathNode(g, x, y, Grid.GridArray[x, y].isWalkable));
        }
        #endregion

        #region Calculations

        private int CalculateDistance(PathNode a, PathNode b)
        {
            //get the distance between two positions
            int xDis = Mathf.Abs(a.x - b.x);
            int yDis = Mathf.Abs(a.y - b.y);

            //get the distance between two distances
            int rem = Mathf.Abs(xDis - yDis);

            return DIAGONAL_COST * Mathf.Min(xDis, yDis) + STRAIGHT_COST * rem;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode> { endNode };
            PathNode currentNode = endNode;

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

        #endregion

        #endregion
    }
}