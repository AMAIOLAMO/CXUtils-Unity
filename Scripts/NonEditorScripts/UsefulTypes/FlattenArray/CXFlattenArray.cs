using System;
using System.Collections;

namespace CXUtils.UsefulTypes
{
    /// <summary>
    ///     A multi dimensional flatten array
    /// </summary>
    public class CXFlattenArray<T> : IEnumerable
    {
        readonly T[] _bufferArray;

        public CXFlattenArray( params int[] dimensions )
        {
            int resultSize = dimensions[0];

            for ( int i = 1; i < dimensions.Length; i++ )
                resultSize *= dimensions[i];

            _bufferArray = new T[resultSize];
        }

        /// <summary>
        ///     Clones the other array
        /// </summary>
        public CXFlattenArray( CXFlattenArray<T> other )
        {
            _bufferArray = new T[other.Length];

            for ( int i = 0; i < other.Length; i++ )
                _bufferArray[i] = other[i];
        }

        public int Length => _bufferArray.Length;

        public T this[ int index ]
        {
            get => _bufferArray[index];
            set => _bufferArray[index] = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T[] GetBufferArray()
        {
            return _bufferArray;
        }

        public void ForEach( Action<T> action )
        {
            foreach ( var item in _bufferArray )
                action.Invoke( item );
        }

        public IEnumerator GetEnumerator()
        {
            return _bufferArray.GetEnumerator();
        }
    }
}
