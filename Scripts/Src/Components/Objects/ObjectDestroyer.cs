using CXUtils.CodeUtils;
using UnityEngine;
using UnityEngine.Serialization;

namespace CXUtils.HelperComponents
{
    /// <summary>
    /// Options fields to destroy objects
    /// </summary>
    public enum ObjectDestroyOptions { InstantOnStart, TimerOnStart }

    /// <summary>
    ///     A simple helper component for destroying an object
    /// </summary>
    [AddComponentMenu( "CXUtils/Objects/ObjectDestroyer" )]
    public class ObjectDestroyer : MonoBehaviour
    {
        [Header( "Configuration" )]
        [SerializeField] bool useOtherTargets;
        [SerializeField] GameObject target;

        [SerializeField] ObjectDestroyOptions destroyOption;

        //show if object destroy option
        [SerializeField] float time;

        public bool OtherTargets { get => useOtherTargets; set => useOtherTargets = value; }
        public GameObject Target { get => target; set => target = value; }

        public ObjectDestroyOptions DestroyOption => destroyOption;

        public float Time { get => time; set => time = value; }

        void Start()
        {
            switch ( destroyOption )
            {
                case ObjectDestroyOptions.InstantOnStart: Destroy( GetTarget() ); break;
                case ObjectDestroyOptions.TimerOnStart: Destroy( GetTarget(), time ); break;

                default: throw ExceptionUtils.Error.NotAccessible;
            }
        }

        GameObject GetTarget() => useOtherTargets ? target : gameObject;
    }
}
