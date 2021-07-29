using System;
using System.Collections.Generic;

namespace CXUtils.CodeUtils
{
    /// <summary>
    ///     Implements a capacity for pool
    /// </summary>
    public interface IPoolCapacity
    {
        int PoolCapacity { get; }
    }

    /// <summary>
    ///     Implements a pool that is expandable
    /// </summary>
    public interface IPoolExpandable<in T> : IPoolCapacity where T : new()
    {
        /// <summary>
        ///     Expands the pool with more items
        /// </summary>
        /// <returns>The new capacity of the expanded pool</returns>
        int ExpandPool( int expandAmount, Func<int, T> initFunc );
    }

    /// <summary>
    ///     implements a single pool cycle
    /// </summary>
    public interface IPoolCycleEvent
    {
        /// <summary>
        ///     When one pool cycle happened, this will trigger
        /// </summary>
        event Action OnCycle;
    }

    /// <summary>
    ///     Implements a wait-able item in the pool
    /// </summary>
    public interface IPoolOccupiedItem
    {
        public bool IsOccupied { get; }
    }

    /// <summary>
    ///     A simple pooler base that you could use to pool stuff for performace
    /// </summary>
    /// <typeparam name="T">The type you want to pool</typeparam>
    public class CxPoolerBase<T> : IPoolCapacity, IPoolCycleEvent where T : new()
    {
        int _currentPoppingCount;

        protected readonly List<T> poolItems;
        public CxPoolerBase( int poolCapacity, Func<int, T> initFunc )
        {
            poolItems = new List<T>();

            PoolCapacity = poolCapacity;

            for ( int i = 0; i < poolCapacity; i++ )
                poolItems.Add( initFunc( i ) );
        }

        public CxPoolerBase( List<T> pool )
        {
            poolItems = pool;
            PoolCapacity = pool.Count;
        }

        public int PoolCapacity { get; }

        public event Action OnCycle;

        /// <summary>
        ///     Pops an item from the pool
        /// </summary>
        public virtual T PopPool()
        {
            //if the current is already the max, then use the first one
            if ( _currentPoppingCount == poolItems.Count )
            {
                _currentPoppingCount = 0;
                InvokeOnCycle();
            }

            var poolingItem = poolItems[_currentPoppingCount];

            _currentPoppingCount++;

            return poolingItem;
        }

        /// <summary>
        ///     Invokes the method <see cref="OnCycle" />
        /// </summary>
        protected void InvokeOnCycle()
        {
            OnCycle?.Invoke();
        }
    }

    /// <summary>
    ///     A pool that auto queues stuff from the pool
    /// </summary>
    public class CxStackPoolerBase<T> : IPoolCapacity, IPoolExpandable<T> where T : IPoolOccupiedItem, new()
    {
        protected readonly Stack<T> poolingItems;
        public CxStackPoolerBase( Stack<T> queuePool )
        {
            poolingItems = queuePool;

            PoolCapacity = queuePool.Count;
        }

        public CxStackPoolerBase( int poolCapacity, Func<int, T> initFunc )
        {
            poolingItems = new Stack<T>();

            PoolCapacity = poolCapacity;

            for ( int i = 0; i < poolCapacity; i++ )
                poolingItems.Push( initFunc( i ) );
        }

        /// <summary>
        ///     The current pool item count
        /// </summary>
        public int Count => poolingItems.Count;

        /// <summary>
        ///     Just checks if the pool is empty or not
        /// </summary>
        public bool IsPoolEmpty => poolingItems.Count == 0;

        public int PoolCapacity { get; private set; }

        public int ExpandPool( int expandAmount, Func<int, T> initFunc )
        {
            PoolCapacity += expandAmount;

            //just push item into the expanded amount :D
            for ( int i = 0; i < expandAmount; i++ )
                poolingItems.Push( initFunc( i ) );

            return PoolCapacity;
        }

        /// <summary>
        ///     Tries to pop an item from the pool <br />
        ///     return true if could pop an item else false
        /// </summary>
        public virtual bool TryPopPool( out T item )
        {
            //if no items to pop out from the pool then return false
            if ( IsPoolEmpty )
            {
                item = default;
                return false;
            }

            item = PopPool();

            return true;
        }

        /// <summary>
        ///     Pops from the pool (Non safe)
        /// </summary>
        public virtual T PopPool()
        {
            //adds a dispose trigger ->
            throw new NotImplementedException();
        }
    }
}
