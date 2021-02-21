using UnityEngine;
using System.Collections.Generic;
using System;

namespace CXUtils.CodeUtils
{
    /// <summary>
    /// A simple pooler base that you could use to pool stuff for performace
    /// </summary>
    /// <typeparam name="T">The type you want to pool</typeparam>
    public abstract class CXPoolerBase<T> where T : new()
    {
        public CXPoolerBase(int poolCapacity, Func<int, T> initFunc)
        {
            poolingItems = new List<T>();

            for ( int i = 0; i < poolCapacity; i++ )
                poolingItems.Add(initFunc(i));
        }

        public CXPoolerBase(List<T> newPool) =>
            poolingItems = newPool;

        protected List<T> poolingItems;

        /// <summary>
        /// Pops an item from the pool
        /// </summary>
        public abstract T PopPool();
    }

    public class CXGameObjectPooler : CXPoolerBase<GameObject>
    {
        public CXGameObjectPooler(List<GameObject> newPool) : base(newPool) { }

        public CXGameObjectPooler(int poolCapacity, Func<int, GameObject> initFunc) : base(poolCapacity, initFunc) { }

        int currentPoppingCount = 0;

        public override GameObject PopPool()
        {
            //if the current is already the max, then use the first one
            if ( currentPoppingCount == poolingItems.Count )
                currentPoppingCount = 0;

            GameObject poolingItem = poolingItems[currentPoppingCount];

            currentPoppingCount++;

            return poolingItem;
        }
    }
}
