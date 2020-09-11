using System;

namespace CXUtils.CodeUtils.Generic
{

    /// <summary> A single heap item </summary>
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }

    /// <summary> A simple heap </summary>
    public class Heap<T> : ICloneable where T : IHeapItem<T>
    {
        #region Vars

        public T[] items;

        /// <summary> Total length of this heap </summary>
        public int Count { get; private set; }

        #endregion

        public Heap(int maxHeapSize) => items = new T[maxHeapSize];

        #region Script Methods

        /// <summary> Check if this heap contains this item </summary>
        public bool Contains(T item) => Equals(items[item.HeapIndex], item);

        /// <summary> Adds an item to the bottom and sort it up </summary>
        public void Add(T item)
        {
            item.HeapIndex = Count;
            items[Count] = item;
            SortUp(item);
            Count++;
        }

        /// <summary> Pop the first item out </summary>
        public T RemoveFirst()
        {
            T firstItem = items[0];
            Count--;
            items[0] = items[Count];
            items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        }

        /// <summary> Updates an item if an item needs to be sorted </summary>
        public void UpdateItem(T item)
        {
            SortUp(item);
            SortDown(item);
        }

        /// <summary> Sort the item down </summary>
        public void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
#pragma warning disable IDE0059
                int swapIndex = 0;

                if (childIndexLeft < Count)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < Count)
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) swapIndex = childIndexRight;

                    if (item.CompareTo(items[swapIndex]) < 0) Swap(item, items[swapIndex]);

                    else return;

                }
                //no childs
                else return;
            }
        }

        /// <summary> Sort the item up </summary>
        public void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;
            while (true)
            {
                T parentItem = items[parentIndex];

                if (item.CompareTo(parentItem) > 0) Swap(item, parentItem);
                else break;

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        /// <summary> swaps two items inside the heap </summary>
        public void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;
            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }

        public object Clone() => new Heap<T>(items.Length) { items = items.Clone() as T[] };

        #endregion
    }
}
