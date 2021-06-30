using System;
using System.Runtime.CompilerServices;
using CXUtils.CodeUtils;

namespace CXUtils.HelperComponents
{
    /// <summary>
    ///     A simple time ticking system for accounting time objects
    /// </summary>
    public class CXTickManager
    {
        readonly CXTimer _baseTimer;

        public readonly float tickTime;

        public CXTickManager( float tickTime )
        {
            this.tickTime = tickTime;

            _baseTimer = new CXTimer( tickTime );
        }

        public int CurrentTick { get; private set; }

        public event Action<int> OnTicked;

        /// <summary>
        ///     Ticks the Tick manager using <paramref name="delta" />
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public bool Tick( float delta )
        {
            if ( !_baseTimer.Tick( delta ) )
                return false;

            CurrentTick++;
            OnTicked?.Invoke( CurrentTick );
            return true;
        }
    }
}
