using CXUtils.Common;
using UnityEngine;

namespace CXUtils.Components
{
    [AddComponentMenu( "CXUtils/Objects/Destroyer" )]
    public class Destroyer : MonoBehaviour
    {
        void Awake()
        {
            if ( _options == DestroyOptions.TimerOnAwake )
                Destroy( GetTarget(), _time );
        }

        void Start()
        {
            switch ( _options )
            {
                case DestroyOptions.OnStart:
                    Destroy( GetTarget() );
                    break;
                case DestroyOptions.TimerOnStart:
                    Destroy( GetTarget(), _time );
                    break;

                default: throw ExceptionUtils.NotAccessible;
            }
        }

        /// <summary>
        ///     Options fields to destroy objects
        /// </summary>
        public enum DestroyOptions
        {
            TimerOnAwake,
            OnStart, TimerOnStart
        }

        public GameObject Target { get => _target; set => _target = value; }

        public DestroyOptions Options => _options;

        public float Time { get => _time; set => _time = value; }

        [SerializeField] DestroyOptions _options;

        [SerializeField] GameObject _target;

        [SerializeField] float _time;

        GameObject GetTarget() => _target ? _target : gameObject;
    }
}
