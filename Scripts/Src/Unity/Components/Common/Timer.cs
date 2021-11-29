using System;
using System.Collections;
using UnityEngine;

namespace CXUtils.Components
{
    /// <summary>
    ///     Basic timer that invokes <see cref="Timeout" />
    /// </summary>
    public class Timer : MonoBehaviour
    {
        public void Begin()
        {
            _tick = _timeout;
            _coroutine = StartCoroutine( TimerInternal() );
        }

        public void Complete()
        {
            StopCoroutine( _coroutine );
            _tick = _timeout;
            Timeout?.Invoke();
        }

        public void End()
        {
            StopCoroutine( _coroutine );
            _tick = _timeout;
        }

        public void SetTimeout( float timeout ) =>
            _timeout = timeout;

        public void Stop() =>
            StopCoroutine( _coroutine );

        void OnValidate() =>
            _timeout = Mathf.Max( 0f, _timeout );

        [SerializeField] bool  _once;
        [SerializeField] float _timeout;

        Coroutine _coroutine;

        // current tick of timer
        float _tick;

        IEnumerator TimerInternal()
        {
            do
            {
                _tick -= Time.deltaTime;

                if ( _tick > .0f )
                    yield return null;

                _tick = _timeout;
                Timeout?.Invoke();
            } while ( _once );
        }

        public event Action Timeout;
    }
}
