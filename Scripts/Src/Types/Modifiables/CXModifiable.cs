using System;
using System.Collections.Generic;

namespace CXUtils.UsefulTypes
{
    ///<summary>
    /// A modifiable value that could register values on it to modify the out come
    ///</summary>
    public class CXModifiable<T> : IModifiable<T>
    {
        public T Value { get; set; }

        ///<summary>
        /// A quick way of calling GetModified()
        ///</summary>
        public T Modified => GetModified();

        public Dictionary<int, Func<T, T>> ModifierDict { get; private set; }

        public CXModifiable()
        {
            ModifierDict = new Dictionary<int, Func<T, T>>();
        }

        public CXModifiable(T initialValue)
        {
            Value = initialValue;
            ModifierDict = new Dictionary<int, Func<T, T>>();
        }

        public T GetModified()
        {
            T resultValue = Value;

            foreach(var modifier in ModifierDict.Values)
                resultValue = modifier(resultValue);
            
            return resultValue;
        }

        public int RegisterModifier(in Func<T, T> modifier)
        {
            int hash;
            ModifierDict.Add(hash = modifier.GetHashCode(), modifier);
            return hash;
        }

        public void UnRegisterModifier(int value)
        {
            ModifierDict.Remove(value);
        }

        public bool TryUnRegisterModifier(int value)
        {
            if (!ModifierDict.ContainsKey(value)) return false;

            ModifierDict.Remove(value);
            return true;
        }
    }
}
