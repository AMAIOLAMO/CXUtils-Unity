using System;
using CXUtils.Common;

namespace CXUtils.Components
{

    /// <summary>
    ///     A simple time ticking system for accounting time objects
    /// </summary>
    public class TickManager<T> : ITickManager<T>
    {
        readonly ITicker<T> _ticker;

        public TickManager( ITicker<T> ticker )
        {
            _ticker = ticker;
        }

        public int Current { get; private set; }

        public event Action<int> OnTicked;

        public void Set( int tick ) => Current = tick;

        public int Reset()
        {
            int last = Current;
            Current = 0;
            return last;
        }

        public bool Tick( T delta )
        {
            if ( !_ticker.Tick( delta ) )
                return false;

            Current++;
            OnTicked?.Invoke( Current );
            _ticker.Reset();
            return true;
        }
    }
}
