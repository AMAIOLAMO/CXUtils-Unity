using System;
using CXUtils.Types;

namespace CXUtils.GridSystem
{
    /// <summary>
    ///     An axis aligned bounding box using Float2 <br />
    ///     NOTE: the origin is in the center of the bounding box,
    ///     the size is also the full width and full height of the bounding box
    /// </summary>
    public readonly struct AABBFloat2
    {
        public AABBFloat2( Float2 origin, Float2 size ) => ( this.origin, this.size ) = ( origin, size );
        public AABBFloat2( AABBFloat2 other ) => ( origin, size ) = ( other.origin, other.size );

        public readonly Float2 origin, size;

        public float HalfXSize => size.x / 2f;
        public float HalfYSize => size.y / 2f;
        public Float2 HalfSize => size / 2f;

        public float MinXBound => origin.x - HalfXSize;
        public float MinYBound => origin.y - HalfYSize;
        public float MaxXBound => origin.x + HalfXSize;
        public float MaxYBound => origin.y + HalfYSize;

        public Float2 MinBound => origin - HalfSize;
        public Float2 MaxBound => origin + HalfSize;
    }

    public abstract class GridBase<T>
    {
        public GridBase( float cellSize, Float2 origin = default ) =>
            ( Origin, CellSize ) = ( origin, cellSize );
        public float CellSize { get; }
        public Float2 Origin { get; }

        public abstract T this[ int x, int y ] { get; set; }
        public abstract T this[ Int2 gridPosition ] { get; set; }

        public float HalfCellSize => CellSize * .5f;
        public Float2 CellCenterOffset => (Float2)HalfCellSize;

        #region Utilities

        public Float2 CellToWorld( int x, int y ) => new Float2( x, y ) * CellSize + Origin;
        public Float2 CellToWorld( Int2 gridPosition ) => (Float2)gridPosition * CellSize + Origin;

        public Int2 WorldToCell( float x, float y ) => ( ( new Float2( x, y ) - Origin ) / CellSize ).FloorInt;
        public Int2 WorldToCell( Float2 worldPosition ) => WorldToLocal(worldPosition).FloorInt;

        public Float2 LocalToWorld( float x, float y ) => new Float2( x, y ) * CellSize + Origin;
        public Float2 LocalToWorld( Float2 localPosition ) => localPosition * CellSize + Origin;

        public Float2 WorldToLocal( float x, float y ) => ( new Float2( x, y ) - Origin ) / CellSize;
        public Float2 WorldToLocal( Float2 worldPosition ) => ( worldPosition - Origin ) / CellSize;

        #endregion
    }

    /// <summary> A 2D Limited grid system </summary>
    /// <typeparam name="T">The type of the things to store inside each grid</typeparam>
    [Serializable]
    public class LimitedGrid<T>
    {
        #region Fields

        T[,] _gridArray;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public float CellSize { get; private set; }
        public Float2 Origin { get; private set; }

        public T this[ int x, int y ]
        {
            get => _gridArray[x, y];
            set => _gridArray[x, y] = value;
        }

        public T this[ Int2 gridPosition ]
        {
            get => _gridArray[gridPosition.x, gridPosition.y];
            set => _gridArray[gridPosition.x, gridPosition.y] = value;
        }

        /// <summary> A half length of the cell size </summary>
        public float HalfCellSize => CellSize * .5f;

        /// <summary> The total cell count </summary>
        public int CellCount => Width * Height;

        /// <summary> Gets the offset to the cell center from the left down bottom </summary>
        public Float2 CellCenterOffset => (Float2)HalfCellSize;

        /// <summary> the whole Grid size </summary>
        public Int2 GridSize => new Int2( Width, Height );

        #endregion

        #region Constructors

        public LimitedGrid( int width, int height, float cellSize, Float2 origin = default, T initialValue = default )
        {
            InitGrid( width, height, cellSize, origin );

            //sets all the value using the given initial Value
            Fill( initialValue );
        }

        public LimitedGrid( int width, int height, float cellSize, Float2 origin = default, Func<int, int, T> createFunc = null )
        {
            InitGrid( width, height, cellSize, origin );

            //sets all the grid value using the given function above
            Map( createFunc );
        }

        public LimitedGrid( Int2 gridSize, float cellSize, Float2 origin = default, Func<int, int, T> createFunc = null )
        {
            InitGrid( gridSize, cellSize, origin );

            Map( createFunc );
        }

        #endregion

        #region GetPositions

        #region WorldPosition

        /// <summary>Tries to converts the grid position into world position </summary>
        public bool TryGetWorldPosition( int x, int y, out Float2 worldPosition )
        {
            if ( CheckXYValid( x, y ) )
            {
                worldPosition = GetWorldPosition( x, y );
                return true;
            }

            worldPosition = default;
            return false;
        }

        /// <summary>Tries to converts the grid position into world position </summary>
        public bool TryGetWorldPosition( Int2 gridPosition, out Float2 worldPosition ) =>
            TryGetWorldPosition( gridPosition.x, gridPosition.y, out worldPosition );

        /// <summary> Converts the grid position into world position </summary>
        public Float2 GetWorldPosition( int x, int y ) =>
            new Float2( x, y ) * CellSize + Origin;

        /// <summary> Converts the grid position into world position </summary>
        public Float2 GetWorldPosition( Int2 gridPosition ) =>
            GetWorldPosition( gridPosition.x, gridPosition.y );

        #endregion

        #region GridPosition

        /// <summary> Converts the world position into grid position </summary>
        public bool TryGetGridPosition( Float2 worldPosition, out Int2 gridPosition )
        {
            var result = GetGridPosition( worldPosition );

            if ( CheckXYValid( result.x, result.y ) )
            {
                gridPosition = result;
                return true;
            }

            gridPosition = default;
            return false;
        }

        public Int2 GetGridPosition( Float2 worldPosition ) =>
            ( ( worldPosition - Origin ) / CellSize ).FloorInt;

        #endregion

        #endregion

        #region Values

        #region SetValues

        /// <summary>
        ///     Tries to set a value using grid position
        ///     <para>Returns if sets correctly</para>
        /// </summary>
        public bool TrySetValue( int x, int y, T value )
        {
            if ( CheckXYValid( x, y ) )
            {
                _gridArray[x, y] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Tries to set a value using grid position
        ///     <para>Returns if sets correctly</para>
        /// </summary>
        public bool TrySetValue( Int2 gridPosition, T value ) =>
            TrySetValue( gridPosition.x, gridPosition.y, value );

        /// <summary>
        ///     Tries to set a value using world position
        ///     <para>Returns if sets correctly</para>
        /// </summary>
        public bool TrySetValue( Float2 worldPosition, T value )
        {
            if ( !TryGetGridPosition( worldPosition, out var gridPos ) )
                return false;

            _gridArray[gridPos.x, gridPos.y] = value;
            return true;

        }


        /// <summary>
        ///     Tries to set a value using grid position
        ///     <para>Not safe</para>
        /// </summary>
        public void SetValue( int x, int y, T value ) =>
            _gridArray[x, y] = value;

        /// <summary>
        ///     Tries to set a value using grid position
        ///     <para>Not safe</para>
        /// </summary>
        public void SetValue( Int2 gridPosition, T value ) =>
            SetValue( gridPosition.x, gridPosition.y, value );

        /// <summary>
        ///     Tries to set a value using world position
        ///     <para>Not safe</para>
        /// </summary>
        public void SetValue( Float2 worldPosition, T value ) =>
            SetValue( GetGridPosition( worldPosition ), value );

        #endregion

        #region GetValues

        /// <summary>
        ///     Gets the value using grid position
        ///     <para> Returns if the grid position is valid</para>
        /// </summary>
        public bool TryGetValue( int x, int y, out T value )
        {
            if ( CheckXYValid( x, y ) )
            {
                value = _gridArray[x, y];
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        ///     Gets the value using grid position
        ///     <para> Returns if the grid position is valid</para>
        /// </summary>
        public bool TryGetValue( Int2 gridPosition, out T value ) =>
            TryGetValue( gridPosition.x, gridPosition.y, out value );

        /// <summary>
        ///     Gets the value using the world position
        ///     <para>Returns if the world position is valid</para>
        /// </summary>
        public bool TryGetValue( Float2 worldPosition, out T value )
        {
            if ( TryGetGridPosition( worldPosition, out var gridPos ) )
            {
                value = _gridArray[gridPos.x, gridPos.y];

                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        ///     Gets the value using the grid position <br />
        ///     Not safe
        /// </summary>
        public T GetValue( int x, int y ) =>
            _gridArray[x, y];

        /// <summary>
        ///     Gets the value using the grid position <br />
        ///     Not safe
        /// </summary>
        public T GetValue( Int2 gridPosition ) =>
            GetValue( gridPosition.x, gridPosition.y );

        /// <summary>
        ///     Gets the value using the world position <br />
        ///     Not safe
        /// </summary>
        public T GetValue( Float2 worldPosition )
        {
            var gridPos = GetGridPosition( worldPosition );
            return GetValue( gridPos.x, gridPos.y );
        }

        #endregion

        #endregion

        #region Script Utils

        bool CheckXYValid( int x, int y ) =>
            x >= 0 && y >= 0 && x < Width && y < Height;

        bool CheckPosValid( Int2 position ) =>
            CheckXYValid( position.x, position.y );

        void InitGrid( int width, int height, float cellSize, Float2 origin )
        {
            ( Width, Height ) = ( width, height );
            ( CellSize, Origin ) = ( cellSize, origin );

            _gridArray = new T[Width, Height];
        }

        void InitGrid( Int2 gridSize, float cellSize, Float2 origin ) =>
            InitGrid( gridSize.x, gridSize.y, cellSize, origin );

        #endregion

        #region Value manipulation

        /// <summary>
        ///     Sets all the value in the grid to the given value
        /// </summary>
        public void Fill( T value = default )
        {
            for ( int i = 0; i < _gridArray.GetLength( 0 ); i++ )
                for ( int j = 0; j < _gridArray.GetLength( 1 ); j++ )
                    _gridArray[i, j] = value;
        }

        /// <summary>
        ///     Uses this function onto all the values on the grid
        /// </summary>
        public void Map( Func<LimitedGrid<T>, int, int, T> mapFunc )
        {
            for ( int x = 0; x < Width; x++ )
                for ( int y = 0; y < Height; y++ )
                    _gridArray[x, y] = mapFunc.Invoke( this, x, y );
        }

        /// <summary>
        ///     Uses this function onto all the values on the grid
        /// </summary>
        public void Map( Func<int, int, T> mapFunc )
        {
            for ( int x = 0; x < Width; x++ )
                for ( int y = 0; y < Height; y++ )
                    _gridArray[x, y] = mapFunc.Invoke( x, y );
        }

        #endregion

        #region Bounds

        /// <summary> Get grid's bounds on world position </summary>
        public AABBFloat2 GetWorldBounds()
        {
            var boundCenter = Origin + (Float2)GridSize * .5f;

            return new AABBFloat2( boundCenter, (Float2)GridSize );
        }

        /// <summary>
        ///     Get grid's bounds on grid position
        /// </summary>
        public AABBFloat2 GetLocalBounds() =>
            new AABBFloat2( new Float2( 0 ), (Float2)GridSize );

        #endregion

        #region Utilities

        /// <summary> Gets the grid value on the given Grid Position and converting it to a string </summary>
        public string ToString( int x, int y ) => _gridArray[x, y].ToString();

        /// <summary> Gets the grid value on the given Grid Position and converting it to a string </summary>
        public string ToString( Int2 gridPosition ) => ToString( gridPosition.x, gridPosition.y );

        #endregion
    }
}
