using UnityEngine;
using System.Diagnostics;
using CXUtils.CodeUtils.Generic;
using System.Collections.Generic;

namespace CXUtils.GridSystem.PathFinding
{
    #region Enum declaration

    /// <summary> Options for finding Paths </summary>
    public enum PathFindingOptions
    {
        /// <summary>
        /// the path will not jump diagonally and don't cut corners
        /// <para>0: walkable, *: path, (): non walkable, %: end</para>
        /// <para>| % | * | * |</para>
        /// <para>| 0 |( )| * |</para>
        /// <para>|( )| * | * |</para> </summary>
        Normal,

        /// <summary> The path will not jump diagonally
        /// <para>0: walkable, *: path, (): non walkable, %: end</para>
        /// <para>| % | * | 0 |</para>
        /// <para>| 0 |( )| * |</para>
        /// <para>|( )| * | 0 |</para> </summary>
        Normal_CutCorners,

        /// <summary> The path will ignore side and jump diagonally
        /// <para>*: path, (): non walkable, %: end</para>
        /// <para>| % | ( ) |</para>
        /// <para>| ( ) | * |</para> </summary>
        JumpDiagonal
    }

    #endregion

    /// <summary> A single path node from pathfinding algorithm </summary>
    public class PathNode : IHeapItem<PathNode>
    {
        #region Fields

        #region XY
        public readonly int x;
        public readonly int y;

        public Vector2Int GridPosition => new Vector2Int(x, y);
        #endregion

        #region Cost fields
        /// <summary> Distance from starting node </summary>
        public int GCost { get; set; }

        /// <summary> Distance from end node </summary>
        public int HCost { get; set; }

        /// <summary> Combined distance (GCost + HCost) </summary>
        public int FCost => GCost + HCost;
        #endregion

        public bool isWalkable = true;

        /// <summary> The node that this came from (Also called: Parent node) </summary>
        public PathNode CameFromNode { get; set; }

        public int HeapIndex { get; set; }

        #endregion

        #region Constructor
        public PathNode(int x, int y, bool isWalkable = true)
        {
            (this.x, this.y) = (x, y);
            this.isWalkable = isWalkable;
        }
        #endregion

        #region Script Utils

        /// <summary> Converts a path node to get the string position </summary>
        public override string ToString() =>
            $"({x}, {y})";

        /// <summary> Comparing if lower F cost then higher priority (1) </summary>
        public int CompareTo(PathNode other)
        {
            int Compare = FCost.CompareTo(other.FCost);
            if (Compare == 0)
                Compare = HCost.CompareTo(other.HCost);

            return -Compare;
        }
        #endregion
    }

    /// <summary> A library that implements A* path finding </summary>
    public class PathFinding
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
        public readonly Vector3 OriginPosition;
        public readonly GridDimentionOptions GridDimention;

        public bool OnDebug { get; set; } = false;
        #endregion

        #region Constructors

        public PathFinding(int width, int height, float cellSize, Vector3 originPosition,
            GridDimentionOptions gridDimentionOptions = GridDimentionOptions.XY)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            OriginPosition = originPosition;
            GridDimention = gridDimentionOptions;

            InitGrid();
        }

        public PathFinding(Vector2Int Size, float cellSize, Vector3 originPosition,
            GridDimentionOptions gridDimentionOptions = GridDimentionOptions.XY)
        {
            Width = Size.x;
            Height = Size.y;
            CellSize = cellSize;
            OriginPosition = originPosition;
            GridDimention = gridDimentionOptions;

            InitGrid();
        }

        #endregion

        #region Find Path A* Algorithm

        #region Find Vector Paths
        /// <summary> Finds a path and returns a list of world vectors </summary>
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

        /// <summary> Finds a path with diagonal moves and returns a list of world vectors </summary>
        public List<Vector2> FindVectorPath_Diagonal(Vector2Int startPosition, Vector2Int endPosition,
            PathFindingOptions pathFindingOptions = PathFindingOptions.Normal) =>
            FindVectorPath(startPosition, endPosition, pathFindingOptions);

        /// <summary> Finds a path with straight moves and returns a list of world vectors </summary>
        public List<Vector2> FindVectorPath_Straight(Vector2Int startPosition, Vector2Int endPosition) =>
            FindVectorPath(startPosition, endPosition, PathFindingOptions.Normal, false);

        #endregion

        #region Find Paths

        /// <summary> Finds a path </summary>
        private List<PathNode> FindPath(Vector2Int startPosition, Vector2Int endPosition,
            PathFindingOptions pathFindingOptions = PathFindingOptions.Normal, bool couldDiagonal = true)
        {
            Stopwatch sw = new Stopwatch();

            if (OnDebug)
                sw.Start();

            //Resetting the grid but puts the is walkable there
            ResetGrid();
            //if there is a value on that position
            if (Grid.TryGetValue(startPosition, out PathNode startNode) &&
                Grid.TryGetValue(endPosition, out PathNode endNode))
            {
                //if both is walkable
                if (startNode.isWalkable && endNode.isWalkable)
                {
                    //clear the last things
                    Heap<PathNode> openList = new Heap<PathNode>(Grid.CellCount);
                    HashSet<PathNode> closeList = new HashSet<PathNode>();

                    //add the starting node
                    openList.Add(startNode);

                    InitPathNodes();

                    //Initialize start Node
                    startNode.GCost = 0;
                    startNode.HCost = CalculateDistance(startNode, endNode, couldDiagonal);
                    //f cost is calculated already


                    while (openList.Count > 0)
                    {
                        PathNode currentNode = openList.RemoveFirst();

                        //if get to the target path
                        if (currentNode == endNode)
                        {
                            if (OnDebug)
                            {
                                //check elapse MS
                                sw.Stop();
                                UnityEngine.Debug.Log($"Path found in: {sw.ElapsedMilliseconds} / ms");
                            }

                            return CalculatePath(endNode);
                        }

                        //else
                        //openList.Remove(currentNode); //when using list
                        closeList.Add(currentNode);

                        //looping through the neighbours and find the lowest FCost
                        foreach (var neighbourNode in GetNeighborNodes(currentNode, pathFindingOptions, couldDiagonal))
                        {
                            //That means it is no longer needed to be searched
                            if (closeList.Contains(neighbourNode))
                                continue;

                            int tentativeGCost =
                                currentNode.GCost + CalculateDistance(currentNode, neighbourNode, couldDiagonal);

                            if (tentativeGCost < neighbourNode.GCost)
                            {
                                //Initialize neighbour Nodes
                                neighbourNode.CameFromNode = currentNode;
                                neighbourNode.GCost = tentativeGCost;
                                neighbourNode.HCost = CalculateDistance(neighbourNode, endNode, couldDiagonal);
                                //f cost is calculated already

                                if (!openList.Contains(neighbourNode))
                                    openList.Add(neighbourNode);
                                else
                                    openList.UpdateItem(neighbourNode);
                            }
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

        #region Script Utils

        #region PathNode Caluclations

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

            #region Check sides 

            bool? left = null;
            bool? right = null;
            bool? down = null;
            bool? up = null;

            #endregion

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

            #region Corner calculations

            switch (pathFindingOptions)
            {
                case PathFindingOptions.Normal:

                    #region Corners

                    if (couldDiagonal)
                    {
                        //check if that exists and check for any of the neighbours
                        //check if both are clear (else only check for straight neighbours)

                        //left up
                        if ((left.HasValue && up.HasValue) && (up.Value && left.Value))
                            AssignIfWalkable(leftIndex, upIndex, neighbourList);

                        //left down
                        if ((left.HasValue && down.HasValue) && (down.Value && left.Value))
                            AssignIfWalkable(leftIndex, downIndex, neighbourList);

                        //right up
                        if ((right.HasValue && up.HasValue) && (up.Value && right.Value))
                            AssignIfWalkable(rightIndex, upIndex, neighbourList);

                        //right down
                        if ((right.HasValue && down.HasValue) && (down.Value && right.Value))
                            AssignIfWalkable(rightIndex, downIndex, neighbourList);
                    }

                    #endregion

                    break;
                case PathFindingOptions.Normal_CutCorners:

                    #region Corners

                    if (couldDiagonal)
                    {
                        //checking if that neighbour exist and if that neighbour is allowed to walk through
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

                    break;
                case PathFindingOptions.JumpDiagonal:

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

                    break;
            }

            #endregion

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

                    //f cost calculated already
                    pathNode.CameFromNode = null;
                }
            }
        }

        private bool AssignIfWalkable(int x, int y, List<PathNode> neightbourList)
        {
            PathNode pathNode = Grid.GetValue(x, y);

            if (pathNode.isWalkable)
                neightbourList.Add(pathNode);

            return pathNode.isWalkable;
        }

        private void InitGrid() =>
            Grid = new CXGrid<PathNode>(Width, Height, CellSize, OriginPosition,
                (x, y) => new PathNode(x, y), GridDimention);

        private void ResetGrid()
        {
            Grid = new CXGrid<PathNode>(Width, Height, CellSize, OriginPosition,
                (x, y) => new PathNode(x, y, Grid.GridArray[x, y].isWalkable), GridDimention);
        }

        #endregion

        #region Calculations

        private int CalculateDistance(PathNode a, PathNode b, bool couldDiagonal) =>
            couldDiagonal ? CalculateDistance_Diagonal(a, b) : CalculateDistance_Straight(a, b);

        private int CalculateDistance_Diagonal(PathNode a, PathNode b)
        {
            //get the distance between two positions
            int xDis = Mathf.Abs(a.x - b.x);
            int yDis = Mathf.Abs(a.y - b.y);

            //get the distance between two distances
            int rem = Mathf.Abs(xDis - yDis);

            return DIAGONAL_COST * Mathf.Min(xDis, yDis) + STRAIGHT_COST * rem;
        }

        private int CalculateDistance_Straight(PathNode a, PathNode b)
        {
            //get the distance between two positions
            int xDis = Mathf.Abs(a.x - b.x);
            int yDis = Mathf.Abs(a.y - b.y);

            return (xDis + yDis) * STRAIGHT_COST;
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

        #region Helper Utils

        /// <summary> Gets if that grid is walkable or not </summary>
        public bool GetIsWalkable(int x, int y) =>
            Grid.GetValue(x, y).isWalkable;

        /// <summary> Gets if that grid is walkable or not </summary>
        public bool GetIsWalkable(Vector2Int gridPosition) =>
            GetIsWalkable(gridPosition.x, gridPosition.y);

        /// <summary> Tries to get if that grid is walkable or not </summary>
        public bool TryGetIsWalkable(int x, int y, out bool isWalkable)
        {
            if (Grid.TryGetValue(x, y, out PathNode pathNode))
            {
                isWalkable = pathNode.isWalkable;
                return true;
            }
            isWalkable = false;
            return false;
        }

        /// <summary> Tries to get if that grid is walkable or not </summary>
        public bool TryGetIsWalkable(Vector2Int gridPosition, out bool isWalkable) =>
            TryGetIsWalkable(gridPosition.x, gridPosition.y, out isWalkable);

        /// <summary> Simplifies the path </summary>
        public List<PathNode> SimplifyPath(List<PathNode> path)
        {
            List<PathNode> SimplifiedPath = new List<PathNode>();
            Vector3 DirOld = Vector3.zero;

            for (int i = 0; i < path.Count - 1; i++)
            {

                Vector3 directionNew = new Vector3(
                    path[i + 1].x - path[i].x,
                    path[i + 1].y - path[i].y
                    );

                //if direction new is not equal to the old direction (changed directions)
                if (!directionNew.Equals(DirOld))
                {
                    SimplifiedPath.Add(path[i]);
                    DirOld = directionNew;
                }
            }

            SimplifiedPath.Add(path[path.Count - 1]);

            return SimplifiedPath;
        }

        /// <summary> Simplifies the path </summary>
        public List<Vector3> SimplifyPath(List<Vector3> path)
        {
            List<Vector3> SimplifiedPath = new List<Vector3>();
            Vector3 DirOld = Vector3.zero;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 directionNew = path[i + 1] - path[i];

                //check and change if direction has changed
                if (!directionNew.Equals(DirOld))
                    SimplifiedPath.Add(path[i]);
            }

            return SimplifiedPath;
        }

        #endregion

        #region Debug
        /// <summary> Draws a debug line on gizmos </summary>
        public void DrawLineDebug(List<PathNode> path)
        {

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 From = Grid.GetWorldPosition(path[i].x, path[i].y) + Grid.CellCenterOffset;
                Vector3 To = Grid.GetWorldPosition(path[i + 1].x, path[i + 1].y) + Grid.CellCenterOffset;

                Gizmos.DrawLine(From, To);
            }
        }

        /// <summary> Draws a debug line on gizmos </summary>
        public void DrawLineDebug(List<PathNode> path, Color color)
        {
            Color originColor = Gizmos.color;
            Gizmos.color = color;

            DrawLineDebug(path);

            Gizmos.color = originColor;
        }

        /// <summary> Draws a debug line on gizmos with a certain duration </summary>
        public void DrawLineDebug(List<PathNode> path, float time) =>
            DrawLineDebug(path, time, Color.white);

        /// <summary> Draws a debug line on gizmos with a certain duration </summary>
        public void DrawLineDebug(List<PathNode> path, float time, Color color)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 From = Grid.GetWorldPosition(path[i].x, path[i].y) + Grid.CellCenterOffset;
                Vector3 To = Grid.GetWorldPosition(path[i + 1].x, path[i + 1].y) + Grid.CellCenterOffset;

                UnityEngine.Debug.DrawLine(From, To, color, time);
            }
        }
        #endregion

        #endregion
    }
}