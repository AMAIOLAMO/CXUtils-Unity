using System;
using UnityEngine;

namespace CXUtils.CodeUtils
{
    /// <summary>
    /// A helper timer that will auto count for you
    /// </summary>
    public struct CXTimer
    {
        public CXTimer(CXTimer other)
        {
            MaxTimer = other.MaxTimer;
            cycleReset = other.cycleReset;

            currentTimer = 0;
            firstCycleCompleted = false;
            OnTimerTriggered = null;
        }

        public CXTimer(float maxTimer, bool cycleReset = true)
        {
            MaxTimer = maxTimer;
            this.cycleReset = cycleReset;

            currentTimer = 0;
            firstCycleCompleted = false;
            OnTimerTriggered = null;
        }

        public float MaxTimer { get; private set; }

        /// <summary>
        /// this controls when timer counts, will it reset when the timer ended
        /// </summary>
        public readonly bool cycleReset;

        public float currentTimer;

        /// <summary>
        /// This event will fire / invoke when the timer ended
        /// </summary>
        public event Action OnTimerTriggered;

        public bool FirstCycleCompleted => firstCycleCompleted;
        private bool firstCycleCompleted;

        /// <summary>
        /// Updates / Counts the timer
        /// </summary>
        /// <returns>if the timer in this frame triggers a cycle reset</returns>
        public bool Tick(float delta)
        {
            if ( !cycleReset && firstCycleCompleted )
                return false;

            currentTimer += delta;
            
            if ( !( currentTimer >= MaxTimer ) )
                return false;

            OnTimerTriggered?.Invoke();

            currentTimer = 0;

            //if this isn't the first cycle completed, then true it
            if ( !firstCycleCompleted )
                firstCycleCompleted = true;

            return true;

        }

        /// <summary>
        /// Same as <see cref="Tick(float)"/>, but will use <see cref="Time.deltaTime"/> as delta
        /// </summary>
        public bool Tick() => Tick(Time.deltaTime);

        /// <summary>
        /// manual reset of everything in the timer
        /// </summary>
        public void ResetTimer()
        {
            currentTimer = 0;
            firstCycleCompleted = false;
        }
    }
}
