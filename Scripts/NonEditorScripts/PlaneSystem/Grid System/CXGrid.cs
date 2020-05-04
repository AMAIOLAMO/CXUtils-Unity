using System;
using UnityEngine;
using CXUtils.CodeUtils;

namespace CXUtils.GridSystem
{
    /// <summary> The Options for grid debugging </summary>
    public enum GridDebugDrawOptions
    {
        Origins, Lines, All, None
    }

    /// <summary> The Options for grid positioning </summary>
    public enum GridDimentionOptions
    {
        XY, XZ, YZ
    }

    /// <summary> A 2D Grid system </summary>
    [Serializable]
    public class CXGrid : CXGrid<object>
    {
        public CXGrid(Vector2Int gridSize, float cellSize, Vector3 origin = default,
            object initialValue = null, GridDimentionOptions gridDimention = GridDimentionOptions.XY) :
            base(gridSize, cellSize, origin, initialValue, gridDimention)
        { }

        public CXGrid(Vector2Int gridSize, float cellSize, Vector3 origin = default, Func<int, int, object> createGridOBJ = null,
            GridDimentionOptions gridDimention = GridDimentionOptions.XY) :
            base(gridSize, cellSize, origin, createGridOBJ, gridDimention)
        { }

        public CXGrid(int width, int height, float cellSize, Vector3 origin = default, object initialValue = null,
            GridDimentionOptions gridDimention = GridDimentionOptions.XY) :
            base(width, height, cellSize, origin, initialValue, gridDimention)
        { }

        public CXGrid(int width, int height, float cellSize, Vector3 origin = default, Func<int, int, object> createGridOBJ = null,
            GridDimentionOptions gridDimention = GridDimentionOptions.XY) :
            base(width, height, cellSize, origin, createGridOBJ, gridDimention)
        { }
    }

    /// <summary> A 2D Grid system </summary>
    /// <typeparam name="T">The type of the things to store inside each grid</typeparam>
    [Serializable]
    public class CXGrid<T>
    {
        #region Fields

        public int Width { get; private set; }
        public int Height { get; private set; }

        public T[,] GridArray { get; private set; }
        public float CellSize { get; private set; }
        public Vector3 Origin { get; private set; }

        public T this[int x, int y]
        {
            get => GridArray[x, y];
            set => GridArray[x, y] = value;
        }

        public GridDimentionOptions GridDimention { get; private set; }

        /// <summary> A half length of the cell size </summary>
        public float HalfCellSize => CellSize * .5f;

        /// <summary> The total cell count </summary>
        public int CellCount => Width * Height;

        /// <summary> Gets the offset to the cell center from the left down bottom </summary>
        public Vector3 CellCenterOffset => Vector3.one * HalfCellSize;

        /// <summary> the whole Grid size </summary>
        public Vector2Int GridSize => new Vector2Int(Width, Height);

        #endregion

        #region Constructors

        public CXGrid(int width, int height, float cellSize,
            Vector3 origin = default, T initialValue = default,
            GridDimentionOptions gridDimention = GridDimentionOptions.XY)
        {
            InitGrid(width, height, cellSize, origin, gridDimention);

            //sets all the value using the given initial Value
            SetAllValues(initialValue);
        }

        public CXGrid(int width, int height, float cellSize,
            Vector3 origin = default, Func<int, int, T> createGridOBJ = null,
            GridDimentionOptions gridDimention = GridDimentionOptions.XY)
        {
            InitGrid(width, height, cellSize, origin, gridDimention);

            //sets all the grid value using the given function above
            MapValues(createGridOBJ);
        }

        public CXGrid(Vector2Int gridSize, float cellSize,
            Vector3 origin = default, T initialValue = default,
            GridDimentionOptions gridDimention = GridDimentionOptions.XY)
        {
            InitGrid(gridSize, cellSize, origin, gridDimention);

            //sets all the value using the given initial Value
            SetAllValues(initialValue);
        }

        public CXGrid(Vector2Int gridSize, float cellSize,
            Vector3 origin = default, Func<int, int, T> createGridOBJ = null,
            GridDimentionOptions gridDimention = GridDimentionOptions.XY)
        {
            InitGrid(gridSize, cellSize, origin, gridDimention);

            MapValues(createGridOBJ);
        }

        #endregion

        #region GetPositions

        #region WorldPosition
        /// <summary>Tries to converts the grid position into world position </summary>
        public bool TryGetWorldPosition(int x, int y, out Vector3 worldPosition)
        {
            if (CheckXYValid(x, y))
            {
                worldPosition = XYToXYZPlanePos(x, y) * CellSize + Origin;
                return true;
            }

            worldPosition = default;
            return false;
        }

        /// <summary>Tries to converts the grid position into world position </summary>
        public bool TryGetWorldPosition(Vector2Int gridPosition, out Vector3 worldPosition) =>
            TryGetWorldPosition(gridPosition.x, gridPosition.y, out worldPosition);

        /// <summary> Converts the grid position into world position </summary> 
        public Vector3 GetWorldPosition(int x, int y) =>
            XYToXYZPlanePos(x, y) * CellSize + Origin;

        /// <summary> Converts the grid position into world position </summary> 
        public Vector3 GetWorldPosition(Vector2Int gridPosition) =>
            GetWorldPosition(gridPosition.x, gridPosition.y);
        #endregion

        #region GridPosition
        /// <summary> Converts the world position into grid position </summary>
        public bool TryGetGridPosition(Vector3 worldPosition, out Vector2Int gridPosition)
        {
            Vector2 newWorldPos = PlanePosToXY(worldPosition);

            Vector2Int Temp = new Vector2Int(
                Mathf.FloorToInt((newWorldPos - (Vector2)Origin).x / CellSize),
                Mathf.FloorToInt((newWorldPos - (Vector2)Origin).y / CellSize)
                );

            if (CheckXYValid(Temp.x, Temp.y))
            {
                gridPosition = Temp;
                return true;
            }

            gridPosition = default;
            return false;
        }

        public Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            Vector2 newWorldPos = PlanePosToXY(worldPosition);

            return new Vector2Int(
                        Mathf.FloorToInt((newWorldPos - (Vector2)Origin).x / CellSize),
                        Mathf.FloorToInt((newWorldPos - (Vector2)Origin).y / CellSize)
                        );
        }

        #endregion

        #endregion
        
        #region Values

        #region SetValues
        /// <summary> Tries to set a value using grid position 
        /// <para>Returns if sets correctly</para> </summary>
        public bool TrySetValue(int x, int y, T value)
        {
            if (CheckXYValid(x, y))
            {
                GridArray[x, y] = value;
                return true;
            }
            return false;
        }

        /// <summary> Tries to set a value using grid position 
        /// <para>Returns if sets correctly</para> </summary>
        public bool TrySetValue(Vector2Int gridPosition, T value) =>
            TrySetValue(gridPosition.x, gridPosition.y, value);

        /// <summary> Tries to set a value using world position
        /// <para>Returns if sets correctly</para> </summary>
        public bool TrySetValue(Vector3 worldPosition, T value)
        {
            if (TryGetGridPosition(worldPosition, out Vector2Int gridPos))
            {
                GridArray[gridPos.x, gridPos.y] = value;
                return true;
            }

            return false;
        }


        /// <summary> Tries to set a value using grid position 
        /// <para>Not safe</para> </summary>
        public void SetValue(int x, int y, T value) =>
            GridArray[x, y] = value;

        /// <summary> Tries to set a value using grid position 
        /// <para>Not safe</para> </summary>
        public void SetValue(Vector2Int gridPosition, T value) =>
            SetValue(gridPosition.x, gridPosition.y, value);

        /// <summary> Tries to set a value using world position 
        /// <para>Not safe</para> </summary>
        public void SetValue(Vector3 worldPosition, T value) =>
            SetValue(GetGridPosition(worldPosition), value);
        #endregion

        #region GetValues
        /// <summary> Gets the value using grid position 
        /// <para> Returns if the grid position is valid</para> </summary>
        public bool TryGetValue(int x, int y, out T value)
        {
            if (CheckXYValid(x, y))
            {
                value = GridArray[x, y];
                return true;
            }
            value = default;
            return false;
        }

        /// <summary> Gets the value using grid position
        /// <para> Returns if the grid position is valid</para> </summary>
        public bool TryGetValue(Vector2Int gridPosition, out T value) =>
            TryGetValue(gridPosition.x, gridPosition.y, out value);

        /// <summary> Gets the value using the world position 
        /// <para>Returns if the world position is valid</para> </summary> 
        public bool TryGetValue(Vector3 worldPosition, out T value)
        {
            if (TryGetGridPosition(worldPosition, out Vector2Int gridPos))
            {
                value = GridArray[gridPos.x, gridPos.y];

                return true;
            }

            value = default;
            return false;
        }

        /// <summary> Gets the value using the grid position 
        /// <para>Not safe</para></summary>
        public T GetValue(int x, int y) =>
            GridArray[x, y];

        /// <summary> Gets the value using the grid position 
        /// <para>Not safe</para></summary>
        public T GetValue(Vector2Int gridPosition) =>
            GetValue(gridPosition.x, gridPosition.y);

        /// <summary> Gets the value using the world position
        /// <para>Not safe</para></summary>
        public T GetValue(Vector3 worldPosition)
        {
            Vector2Int gridPos = GetGridPosition(worldPosition);
            return GetValue(gridPos.x, gridPos.y);
        }
        #endregion

        #endregion

        #region Script Utils

        #region Simplifying Script

        private bool CheckXYValid(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return false;

            return true;
        }

        private Vector3 XYToXYZPlanePos(int x, int y)
        {
            switch (GridDimention)
            {
                case GridDimentionOptions.XY:
                return new Vector3(x, y, 0);

                case GridDimentionOptions.XZ:
                return new Vector3(x, 0, y);

                // YZ
                default:
                return new Vector3(0, x, y);
            }
        }

        private Vector2 PlanePosToXY(Vector3 PlaneCoords)
        {
            switch (GridDimention)
            {
                case GridDimentionOptions.XY:
                return PlaneCoords;

                case GridDimentionOptions.XZ:
                return new Vector2(PlaneCoords.x, PlaneCoords.z);

                // YZ
                default:
                return new Vector2(PlaneCoords.y, PlaneCoords.z);
            }
        }

        private void InitGrid(int width, int height, float cellSize, Vector3 origin, GridDimentionOptions gridDimentionOptions)
        {
            (Width, Height) = (width, height);
            (CellSize, Origin) = (cellSize, origin);

            GridDimention = gridDimentionOptions;
            GridArray = new T[Width, Height];
        }

        private void InitGrid(Vector2Int gridSize, float cellSize, Vector3 origin, GridDimentionOptions gridDimentionOptions) =>
            InitGrid(gridSize.x, gridSize.y, cellSize, origin, gridDimentionOptions);

        #endregion

        #region Value manipulation

        /// <summary> Sets all the value in the grid to the given value </summary>
        public void SetAllValues(T value = default)
        {
            for (int i = 0; i < GridArray.GetLength(0); i++)
                for (int j = 0; j < GridArray.GetLength(1); j++)
                    GridArray[i, j] = value;
        }

        /// <summary> Resets all the values </summary>
        public void ResetAllValues() =>
            SetAllValues();

        /// <summary> Uses this function onto all the values on the grid </summary>
        public void MapValues(Func<CXGrid<T>, int, int, T> mapFunc)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    GridArray[x, y] = mapFunc.Invoke(this, x, y);
        }

        /// <summary> Uses this function onto all the values on the grid </summary>
        public void MapValues(Func<int, int, T> mapFunc)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    GridArray[x, y] = mapFunc.Invoke(x, y);
        }

        #endregion

        #region Bounds
        /// <summary> Get grid's bounds on world position </summary>
        public Bounds GetWorldBounds()
        {
            Vector3 boundCenter = Origin + XYToXYZPlanePos(Width, Height) * .5f;
            Vector3 boundSize = XYToXYZPlanePos(Width, Height);

            return new Bounds(boundCenter, boundSize);
        }

        /// <summary> Get grid's bounds on grid position </summary>
        public Bounds GetGridBounds()
        {
            Vector2 boundCenter = new Vector2(Width, Height) * .5f;
            Vector2 boundSize = new Vector2(Width, Height);

            return new Bounds(boundCenter, boundSize);
        }
        #endregion

        #region Iterators

        /// <summary> Iterates through the whole grid </summary>
        public void Iterate(Action<T, int, int> y_action)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    y_action(GridArray[x, y], x, y);
        }

        /// <summary> Iterates through the whole grid </summary>
        public void Iterate(Action<T, int, int> y_action, Action<int> x_action)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                    y_action(GridArray[x, y], x, y);

                x_action(x);
            }
        }

        #endregion

        #region Other Utils

        /// <summary> Gets the grid value on the given Grid Position and converting it to a string </summary>
        public string ToString(int x, int y) =>
            GridArray[x, y].ToString();

        /// <summary> Gets the grid value on the given Grid Position and converting it to a string </summary>
        public string ToString(Vector2Int gridPosition) =>
            ToString(gridPosition.x, gridPosition.y);

        #endregion

        #region Debug

        #region Drawing Grid

        /// <summary> draws a debug gizmos for the grid </summary>
        public void DrawDebug(Color color,
            GridDebugDrawOptions gridDebugDrawOptions, float originRadius = .08f) =>
            Ddebug(color, color, gridDebugDrawOptions, originRadius);

        /// <summary> draws a debug gizmos for the grid </summary>
        public void DrawDebug(GridDebugDrawOptions gridDebugDrawOptions = GridDebugDrawOptions.All,
            float originRadius = .08f) =>
            DrawDebug(Color.white, gridDebugDrawOptions, originRadius);

        /// <summary> draws a debug gizmos for the grid </summary>
        public void DrawDebug(Color originColor, Color lineColor, float originRadius = .08f) =>
            Ddebug(originColor, lineColor, GridDebugDrawOptions.All, originRadius);

        /// <summary> draws a debug gizmos for the grid </summary>
        private void Ddebug(Color originColor, Color lineColor,
            GridDebugDrawOptions gridDebugDrawOptions, float originRadius = .08f)
        {

            if (gridDebugDrawOptions == GridDebugDrawOptions.All || gridDebugDrawOptions == GridDebugDrawOptions.Lines)
            {
                Gizmos.color = lineColor;
                DrawDebugLines();
            }

            if (gridDebugDrawOptions == GridDebugDrawOptions.All || gridDebugDrawOptions == GridDebugDrawOptions.Origins)
            {
                Gizmos.color = originColor;
                DrawDebugPoints(originRadius);
            }

        }

        #endregion

        #region Draw Text

        /// <summary> Draws a text on world position </summary>
        public TextMesh[,] DrawText(Transform parent, int fontSize) =>
            DrawText(parent, fontSize, Color.white);

        /// <summary> Draws a text on world position </summary>
        public TextMesh[,] DrawText(Transform parent, int fontSize, Color color)
        {
            TextMesh[,] textMeshes = new TextMesh[Width, Height];

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    Vector3 newPosition = GetWorldPosition(x, y) + CellCenterOffset;
                    T value = GetValue(x, y);

                    textMeshes[x, y] = TextUtils.SpawnTextOnWorld(
                        parent, newPosition, value.ToString(),
                        fontSize, color, TextAnchor.MiddleCenter, TextAlignment.Center, 0
                        );
                }

            return textMeshes;
        }

        /// <summary> Updates the debug texts </summary>
        public void UpdateTexts(TextMesh[,] textMeshes) =>
            Iterate((value, x, y) => textMeshes[x, y].text = value.ToString());

        /// <summary> Updates the debug texts </summary>
        public void UpdateTexts(TextMesh[,] textMeshes, int fontSize, Color color)
        {
            TextMesh thisTextMesh;

            Iterate((value, x, y) =>
            {
                thisTextMesh = textMeshes[x, y];

                thisTextMesh.text = value.ToString();
                thisTextMesh.fontSize = fontSize;
                thisTextMesh.color = color;
            });
        }

        #endregion

        #region HelperDraw
        private void DrawDebugPoints(float originRadius = .08f)
        {
            for (int x = 0; x < Width + 1; x++)
            {
                for (int y = 0; y < Height + 1; y++)
                {
                    //Single cell draw
                    Vector3 LDPosition;
                    LDPosition = GetWorldPosition(x, y);

                    Gizmos.DrawSphere(LDPosition, originRadius);
                }
            }
        }

        private void DrawDebugLines()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    //Single cell draw
                    Vector3 LDPosition;
                    LDPosition = GetWorldPosition(x, y);

                    //left down
                    {
                        //vertical
                        Gizmos.DrawLine(LDPosition, GetWorldPosition(x, y + 1));

                        //horizontal
                        Gizmos.DrawLine(LDPosition, GetWorldPosition(x + 1, y));
                    }
                }
            }

            Vector3 LUPosition = GetWorldPosition(0, Height);
            Vector3 RDPosition = GetWorldPosition(Width, 0);
            Vector3 RUPosition = GetWorldPosition(Width, Height);

            //right up
            {
                Gizmos.DrawLine(LUPosition, RUPosition);
                Gizmos.DrawLine(RDPosition, RUPosition);
            }
        }
        #endregion

        #endregion

        #endregion
    }
}