using UnityEngine;

namespace CXUtils.GridSystem
{
    /// <summary> A 2D Grid system </summary>
    /// <typeparam name="T">The type of the things to store inside each grid</typeparam>
    public class CXGrid<T>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public T[,] GridArray { get; private set; }
        public float CellSize { get; private set; }
        public Vector2 Origin { get; private set; }

        //public bool ShowDebug { get; set; }

        public CXGrid(int width, int height, float cellSize,
            Vector2 origin = default, T initialValue = default)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            Origin = origin;

            GridArray = new T[Width, Height];

            SetAllValues(initialValue);
        }

        #region GetPositions

        #region WorldPosition
        /// <summary>Tries to converts the grid position into world position </summary>
        public bool TryGetWorldPosition(int x, int y, out Vector2 worldPosition)
        {
            if (CheckXYValid(x, y))
            {
                worldPosition = new Vector2(x, y) * CellSize + Origin;
                return true;
            }

            worldPosition = default;
            return false;
        }

        /// <summary>Tries to converts the grid position into world position </summary>
        public bool TryGetWorldPosition(Vector2Int gridPosition, out Vector2 worldPosition) =>
            TryGetWorldPosition(gridPosition.x, gridPosition.y, out worldPosition);

        /// <summary> Converts the grid position into world position </summary> 
        public Vector2 GetWorldPosition(int x, int y) =>
            new Vector2(x, y) * CellSize + Origin;

        /// <summary> Converts the grid position into world position </summary> 
        public Vector2 GetWorldPosition(Vector2Int gridPosition) =>
            GetWorldPosition(gridPosition.x, gridPosition.y);
        #endregion

        #region GridPosition
        /// <summary> Converts the world position into grid position </summary>
        public bool TryGetGridPosition(Vector2 worldPosition, out Vector2Int gridPosition)
        {
            Vector2Int Temp = new Vector2Int(
                Mathf.FloorToInt((worldPosition - Origin).x / CellSize),
                Mathf.FloorToInt((worldPosition - Origin).y / CellSize)
                );

            if (CheckXYValid(Temp.x, Temp.y))
            {
                gridPosition = Temp;
                return true;
            }

            gridPosition = default;
            return false;
        }

        public Vector2Int GetGridPosition(Vector2 worldPosition) =>
            new Vector2Int(
                Mathf.FloorToInt((worldPosition - Origin).x / CellSize),
                Mathf.FloorToInt((worldPosition - Origin).y / CellSize)
                );

        #endregion

        #endregion

        #region SetValues
        public bool SetValue(int x, int y, T value)
        {
            if (CheckXYValid(x, y))
            {
                GridArray[x, y] = value;
                return true;
            }
            return false;
        }

        public bool SetValue(Vector2Int gridPosition, T value) =>
            SetValue(gridPosition.x, gridPosition.y, value);

        public bool SetValue(Vector2 worldPosition, T value)
        {
            if (TryGetGridPosition(worldPosition, out Vector2Int gridPos))
            {
                GridArray[gridPos.x, gridPos.y] = value;
                return true;
            }

            return false;
        }
        #endregion

        #region GetValues
        /// <summary> Gets the value using grid position 
        /// <para> Returns if the grid position is valid</para> </summary>
        public bool GetValue(int x, int y, out T value)
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
        public bool GetValue(Vector2Int gridPosition, out T value) =>
            GetValue(gridPosition.x, gridPosition.y, out value);

        /// <summary> Gets the value using the world position 
        /// <para>Returns if the world position is valid</para> </summary> 
        public bool GetValue(Vector2 worldPosition, out T value)
        {
            if (TryGetGridPosition(worldPosition, out Vector2Int gridPos))
            {
                value = GridArray[gridPos.x, gridPos.y];

                return true;
            }

            value = default;
            return false;
        }
        #endregion

        #region Script Methods
        private bool CheckXYValid(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return false;

            return true;
        }

        /// <summary> Sets all the value in the grid to the given value </summary>
        public void SetAllValues(T value)
        {
            for (int i = 0; i < GridArray.GetLength(0); i++)
                for (int j = 0; j < GridArray.GetLength(1); j++)
                    GridArray[i, j] = value;
        }

        public void DrawDebug(Color color)
        {
            Gizmos.color = color;

            DrawDebugPoints();
            DrawDebugLines();
        }

        public void DrawDebug() =>
            DrawDebug(Color.white);

        #region HelperDraw
        private void DrawDebugPoints()
        {
            for (int x = 0; x < Width + 1; x++)
            {
                for (int y = 0; y < Height + 1; y++)
                {
                    //Single cell draw
                    Vector2 LDPosition;
                    LDPosition = GetWorldPosition(x, y);

                    Gizmos.DrawSphere(LDPosition, .08f);
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
                    Vector2 LDPosition;
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

            Vector2 LUPosition = GetWorldPosition(0, Height);
            Vector2 RDPosition = GetWorldPosition(Width, 0);
            Vector2 RUPosition = GetWorldPosition(Width, Height);
            //right up
            {
                Gizmos.DrawLine(LUPosition, RUPosition);
                Gizmos.DrawLine(RDPosition, RUPosition);
            }
        }
        #endregion

        #endregion
    }
}