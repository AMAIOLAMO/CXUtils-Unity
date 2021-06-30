using System;
using System.Runtime.CompilerServices;

namespace CXUtils.CodeUtils
{
    /// <summary>
    ///     A timer that ticks using delta
    /// </summary>
    public class CXTimer : ICloneable
    {
        public CXTimer( float maxTimer, bool cycleReset = true )
        {
            MaxTimer = maxTimer;
            CycleReset = cycleReset;
            FirstCycleCompleted = false;
        }

        /// <summary>
        ///     Deep copies (everything, even the current state) the whole timer
        /// </summary>
        public CXTimer( CXTimer other )
        {
            MaxTimer = other.MaxTimer;
            CurrentTimer = other.CurrentTimer;
            CycleReset = other.CycleReset;
            FirstCycleCompleted = other.FirstCycleCompleted;
        }

        public float MaxTimer { get; }

        public float CurrentTimer { get; private set; }
        public bool CycleReset { get; }

        public bool FirstCycleCompleted { get; private set; }

        /// <summary>
        ///     Deep clones the timer
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public object Clone()
        {
            return new CXTimer( this );
        }
        
        /// <summary>
        ///     Ticks the timer using the <see cref="delta" />
        /// </summary>
        public bool Tick( float delta )
        {
            //if not gonna cycle reset and the first cycle is already completed
            if ( !CycleReset && FirstCycleCompleted )
                return false;

            CurrentTimer += delta;

            //if current Timer is not over max timer
            if ( !( CurrentTimer >= MaxTimer ) )
                return false;

            DoCycleCompleted();

            return true;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        void Reset()
        {
            CurrentTimer = 0;
        }

        /// <summary>
        ///     set's the <see cref="CurrentTimer" /> back to initial value and resets the <see cref="FirstCycleCompleted" />
        /// </summary>
        void FullReset()
        {
            CurrentTimer = 0;
            FirstCycleCompleted = false;
        }

        void DoCycleCompleted()
        {
            CurrentTimer = 0;

            OnCycleComplete?.Invoke();

            if ( !FirstCycleCompleted )
                FirstCycleCompleted = true;
        }

        public event Action OnCycleComplete;
    }
}
