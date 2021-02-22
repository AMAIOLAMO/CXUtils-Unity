using System;
using System.Collections.Generic;

namespace CXUtils.CodeUtils
{
    /// <summary>
    /// implements a single pool cycle
    /// </summary>
    public interface IPoolCycleEvent
    {
        /// <summary>
        /// When one pool cycle happened, this will trigger
        /// </summary>
        public event Action OnCycle;
    }

    /// <summary>
    /// A simple pooler base that you could use to pool stuff for performace
    /// </summary>
    /// <typeparam name="T">The type you want to pool</typeparam>
    public class CXPoolerBase<T> : IPoolCycleEvent where T : new()
    {
        public CXPoolerBase(int poolCapacity, Func<int, T> initFunc)
        {
            poolingItems = new List<T>();

            for ( int i = 0; i < poolCapacity; i++ )
                poolingItems.Add(initFunc(i));
        }

        public CXPoolerBase(List<T> pool) =>
            poolingItems = pool;

        protected List<T> poolingItems;

        public int PoolCapacity => poolingItems.Count;

        int currentPoppingCount = 0;

        public event Action OnCycle;

        /// <summary>
        /// Pops an item from the pool
        /// </summary>
        public virtual T PopPool()
        {
            //if the current is already the max, then use the first one
            if ( currentPoppingCount == poolingItems.Count )
            {
                currentPoppingCount = 0;
                OnCycle?.Invoke();
            }

            T poolingItem = poolingItems[currentPoppingCount];

            currentPoppingCount++;

            return poolingItem;
        }
    }
}
