using CXUtils.Common;
using UnityEngine;

namespace CXUtils.Components
{
    [AddComponentMenu( "CXUtils/Objects/Follower" )]
    public class Follower : MonoBehaviour
    {
        void Update()
        {
            if ( updateOptions == UpdateOptions.Update ) FollowObject();
        }

        void FixedUpdate()
        {
            if ( updateOptions == UpdateOptions.FixedUpdate ) FollowObject();
        }

        void LateUpdate()
        {
            if ( updateOptions == UpdateOptions.LateUpdate ) FollowObject();
        }

        [Header( "Configuration" )]
        public Transform transformTo;

        public PositionOptions  positionOptions  = PositionOptions.All;
        public RotationOptions  rotationOptions  = RotationOptions.None;
        public UpdateOptions    updateOptions    = UpdateOptions.LateUpdate;
        public DeltaTimeOptions deltaTimeOptions = DeltaTimeOptions.Normal;

        [Range( 0f, 100f )] [Tooltip( "The lerp speed of the follower" )]
        public float movingSpeed = 2f;

        [Range( 0f, 100f )] [Tooltip( "The rotation speed of the follower" )]
        public float rotationSpeed = 2f;

        public Vector3 offSet = Vector3.zero;

        void FollowObject()
        {
            FollowPos();
            FollowRotation();
        }

        void FollowPos()
        {
            var newPos = transformTo.position;

            //check the has off set (if has then add)
            if ( positionOptions == PositionOptions.All || positionOptions == PositionOptions.HasOffsetOnly )
                newPos += offSet;

            if ( positionOptions == PositionOptions.All || positionOptions == PositionOptions.HasLerpOnly )
                newPos = Vector3.Lerp( transform.position, newPos, GetDeltaTime() * movingSpeed );

            //then just set it
            transform.position = newPos;
        }

        void FollowRotation()
        {
            //if none then just don't do anything
            if ( rotationOptions == RotationOptions.None )
                return;

            //stores the target rotation
            var newRot = transformTo.rotation;

            //if (objectFollowRotationOptions == ObjectFollowRotationOptions.NoLerp) then do nothing

            if ( rotationOptions == RotationOptions.HasLerp )
                newRot = Quaternion.Lerp( transform.rotation, newRot, GetDeltaTime() * rotationSpeed );

            if ( newRot != default )
                transform.rotation = newRot;
        }

        float GetDeltaTime()
        {
            switch ( deltaTimeOptions )
            {
                case DeltaTimeOptions.None:   return 1f;
                case DeltaTimeOptions.Normal: return Time.deltaTime;
                case DeltaTimeOptions.Fixed:  return Time.fixedDeltaTime;

                default: throw ExceptionUtils.NotAccessible;
            }
        }
        
        #region Enums

        /// <summary> Option flags for the object to follow the position </summary>
        public enum PositionOptions
        {
            None, All, HasOffsetOnly, HasLerpOnly
        }

        /// <summary> Option flags for the object to follow the rotation </summary>
        public enum RotationOptions
        {
            None, HasLerp, NoLerp
        }

        /// <summary> Option flags for the update mode for the follower </summary>
        public enum UpdateOptions
        {
            Update, FixedUpdate, LateUpdate
        }

        public enum DeltaTimeOptions
        {
            None, Normal, Fixed
        }

        #endregion
    }
}
