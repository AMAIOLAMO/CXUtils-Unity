using CXUtils.CodeUtils;
using UnityEngine;

namespace CXUtils.HelperComponents
{
    /// <summary> Options fields to destoy objects </summary>
    public enum ObjectDestroyOptions { InstantOnStart, TimerOnStart }

    /// <summary>
    /// A simple helper component for destroying an object
    /// </summary>
    [AddComponentMenu( "CXUtils/Objects/ObjectDestroyer" )]
    public class ObjectDestroyer : MonoBehaviour
    {
        [Header( "Configuration" )]
        [SerializeField] private bool otherTargets;
        [SerializeField] private GameObject target = default;

        [SerializeField] private ObjectDestroyOptions objectDestroyOption = default;

        //show if object destroy option
        [SerializeField] private float time;


        public bool OtherTargets { get => otherTargets; set => otherTargets = value; }
        public GameObject Target { get => target; set => target = value; }

        public ObjectDestroyOptions ObjectDestroyOption => objectDestroyOption;

        public float Time { get => time; set => time = value; }

        void Start()
        {
            switch ( objectDestroyOption )
            {
                case ObjectDestroyOptions.InstantOnStart: Destroy( GetTarget() ); break;
                case ObjectDestroyOptions.TimerOnStart:   Destroy( GetTarget(), time ); break;

                default: throw ExceptionUtils.GetException(ErrorType.NotAccessible);
            }
        }

        private GameObject GetTarget() =>
            otherTargets ? target : gameObject;
    }
}
