using System;
using System.Collections.Generic;
using CXUtils.Types;

namespace CXUtils.Grid
{
    public abstract class GridBase<T>
    {
        public GridBase( float cellSize, Float2 origin = default ) =>
            ( Origin, CellSize ) = ( origin, cellSize );
        public float CellSize { get; }
        public Float2 Origin { get; }

        public abstract T this[ int x, int y ] { get; set; }
        public abstract T this[ Int2 gridPosition ] { get; set; }

        /// <summary>
        ///     The half length of the cell size
        /// </summary>
        public float HalfCellSize => CellSize * .5f;
        public Float2 CellCenterOffset => (Float2)HalfCellSize;

        #region Getters & Setters

        public abstract void SetValue( int x, int y, T value );
        public abstract void SetValue( Int2 cellPosition, T value );

        public abstract T GetValue( int x, int y );
        public abstract T GetValue( Int2 cellPosition );

        #endregion

        #region Utilities

        public Float2 CellToWorld( int x, int y ) => new Float2( x, y ) * CellSize + Origin;
        public Float2 CellToWorld( Int2 cellPosition ) => (Float2)cellPosition * CellSize + Origin;

        public Int2 WorldToCell( float x, float y ) => ( WorldToLocal( x, y ) / CellSize ).FloorInt;
        public Int2 WorldToCell( Float2 worldPosition ) => ( WorldToLocal( worldPosition ) / CellSize ).FloorInt;

        public Float2 LocalToWorld( float x, float y ) => new Float2( x, y ) * CellSize + Origin;
        public Float2 LocalToWorld( Float2 localPosition ) => localPosition * CellSize + Origin;

        public Float2 WorldToLocal( float x, float y ) => new Float2( x, y ) - Origin;
        public Float2 WorldToLocal( Float2 worldPosition ) => worldPosition - Origin;

        public abstract void Swap( int x1, int y1, int x2, int y2 );
        public abstract void Swap( Int2 cell1, Int2 cell2 );

        public virtual string ToString( int x, int y ) => this[x, y].ToString();
        public virtual string ToString( Int2 cellPosition ) => this[cellPosition.x, cellPosition.y].ToString();

        #endregion
    }

    /// <summary>
    ///     A 2D Limited size grid system
    /// </summary>
    /// <typeparam name="T">The type that each cell stores</typeparam>
    [Serializable]
    public class LimitedGrid<T> : GridBase<T>
    {

        /// <summary>
        ///     Tries to converts the cell position into world position
        /// </summary>
        public bool TryGetWorld( int x, int y, out Float2 worldPosition )
        {
            if ( IsCellValid( x, y ) )
            {
                worldPosition = CellToWorld( x, y );
                return true;
            }

            worldPosition = default;
            return false;
        }

        /// <summary>
        ///     Tries to converts the grid position into world position
        /// </summary>
        public bool TryGetWorld( Int2 gridPosition, out Float2 worldPosition ) =>
            TryGetWorld( gridPosition.x, gridPosition.y, out worldPosition );

        /// <summary> Converts the world position into grid position </summary>
        public bool TryGetCell( Float2 worldPosition, out Int2 gridPosition )
        {
            var result = WorldToCell( worldPosition );

            if ( IsCellValid( result.x, result.y ) )
            {
                gridPosition = result;
                return true;
            }

            gridPosition = default;
            return false;
        }
        #region Fields

        T[,] _gridArray;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public override T this[ int x, int y ]
        {
            get => _gridArray[x, y];
            set => _gridArray[x, y] = value;
        }

        public override T this[ Int2 gridPosition ]
        {
            get => _gridArray[gridPosition.x, gridPosition.y];
            set => _gridArray[gridPosition.x, gridPosition.y] = value;
        }


        /// <summary>
        ///     The total cell count
        /// </summary>
        public int CellCount => Width * Height;

        /// <summary>
        ///     The whole <see cref="LimitedGrid{T}" />'s size
        /// </summary>
        public Int2 GridSize => new Int2( Width, Height );

        #endregion

        #region Constructors

        public LimitedGrid( int width, int height, float cellSize, Float2 origin = default ) : base( cellSize, origin )
        {
            ( Width, Height ) = ( width, height );
            _gridArray = new T[Width, Height];
        }

        public LimitedGrid( Int2 gridSize, float cellSize, Float2 origin = default ) : base( cellSize, origin )
        {
            ( Width, Height ) = ( gridSize.x, gridSize.y );
            _gridArray = new T[Width, Height];
        }

        #endregion

        #region Values

        #region SetValues

        /// <summary>
        ///     Tries to set a value using grid position
        ///     <para>Returns if sets correctly</para>
        /// </summary>
        public bool TrySetValue( int x, int y, T value )
        {
            if ( !IsCellValid( x, y ) )
                return false;

            _gridArray[x, y] = value;
            return true;
        }

        /// <summary>
        ///     Tries to set a value using grid position
        ///     <para>Returns if sets correctly</para>
        /// </summary>
        public bool TrySetValue( Int2 cellPosition, T value ) =>
            TrySetValue( cellPosition.x, cellPosition.y, value );

        public override void SetValue( int x, int y, T value ) =>
            _gridArray[x, y] = value;

        /// <summary>
        ///     Tries to set a value using grid position
        ///     <para>Not safe</para>
        /// </summary>
        public override void SetValue( Int2 cellPosition, T value ) =>
            _gridArray[cellPosition.x, cellPosition.y] = value;

        #endregion

        #region GetValues

        /// <summary>
        ///     Gets the value using grid position
        ///     <para> Returns if the grid position is valid</para>
        /// </summary>
        public bool TryGetValue( int x, int y, out T value )
        {
            if ( IsCellValid( x, y ) )
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
        ///     Gets the value using the grid position <br />
        ///     Not safe
        /// </summary>
        public override T GetValue( int x, int y ) => _gridArray[x, y];

        /// <summary>
        ///     Gets the value using the grid position <br />
        ///     Not safe
        /// </summary>
        public override T GetValue( Int2 gridPosition ) => GetValue( gridPosition.x, gridPosition.y );

        #endregion

        #endregion

        #region Script Utils

        bool IsCellValid( int x, int y ) =>
            x >= 0 && y >= 0 && x < Width && y < Height;

        bool IsCellValid( Int2 cellPosition ) =>
            cellPosition.x >= 0 && cellPosition.y >= 0 && cellPosition.x < Width && cellPosition.y < Height;

        #endregion

        #region Value manipulation

        /// <summary>
        ///     Sets all the value in the grid to the given value
        /// </summary>
        public void Fill( T value = default )
        {
            for ( int x = 0; x < Width; ++x )
                for ( int y = 0; y < Height; ++y )
                    _gridArray[x, y] = value;
        }

        public void Fill( Func<int, int, T> fillFunction )
        {
            for ( int x = 0; x < Width; ++x )
                for ( int y = 0; y < Height; ++y )
                    _gridArray[x, y] = fillFunction( x, y );
        }

        /// <summary>
        ///     Uses this function onto all the values on the grid
        /// </summary>
        public void Map( Func<LimitedGrid<T>, int, int, T> mapFunction )
        {
            for ( int x = 0; x < Width; ++x )
                for ( int y = 0; y < Height; ++y )
                    _gridArray[x, y] = mapFunction.Invoke( this, x, y );
        }

        #endregion

        #region Bounds

        /// <summary>
        ///     Get grid's bounds on world position
        /// </summary>
        public AABBFloat2 GetWorldBounds()
        {
            var boundCenter = Origin + (Float2)GridSize * .5f;

            return new AABBFloat2( boundCenter, (Float2)GridSize );
        }

        /// <summary>
        ///     Get grid's bounds on grid position
        /// </summary>
        public AABBFloat2 GetLocalBounds() =>
            new AABBFloat2( (Float2)0f, (Float2)GridSize );

        #endregion

        #region Utilities

        public override void Swap( int x1, int y1, int x2, int y2 ) =>
            ( _gridArray[x1, y1], _gridArray[x2, y2] ) = ( _gridArray[x2, y2], _gridArray[x1, y1] );

        public override void Swap( Int2 cell1, Int2 cell2 ) =>
            ( _gridArray[cell1.x, cell1.y], _gridArray[cell1.x, cell1.y] ) = ( _gridArray[cell1.x, cell1.y], _gridArray[cell1.x, cell1.y] );
        
        #endregion
    }
}
