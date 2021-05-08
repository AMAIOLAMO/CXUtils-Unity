using System;
using System.Collections.Generic;

namespace CXUtils.UsefulTypes
{
    /// <summary>
    /// Implements a modifiable for a target value
    /// </summary>
    public interface IModifiable<T>
    {
        T Value { get; }

        Dictionary<int, Func<T, T>> ModifierDict { get; }

        int RegisterModifier(in Func<T, T> modifier);
        void UnRegisterModifier(int value);
        bool TryUnRegisterModifier(int value);

        T GetModified();
    }
}

