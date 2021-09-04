﻿using CXUtils.Common;
using UnityEngine;

namespace CXUtils.Components
{
    [AddComponentMenu( "CXUtils/Objects/ObjectFollower" )]
    public class ObjectFollower : MonoBehaviour
    {
        #region Enums

        /// <summary> Option flags for the object to follow the position </summary>
        public enum ObjectFollowPositionOptions
        {
            None, All, HasOffsetOnly, HasLerpOnly
        }

        /// <summary> Option flags for the object to follow the rotation </summary>
        public enum ObjectFollowRotationOptions
        {
            None, HasLerp, NoLerp
        }

        /// <summary> Option flags for the update mode for the follower </summary>
        public enum ObjectUpdateOptions
        {
            Update, FixedUpdate, LateUpdate
        }

        public enum ObjectDeltaTimeOptions
        {
            None, Normal, Fixed
        }

        #endregion

        #region Fields

        [Header( "Configuration" )]
        public Transform transformTo;

        public ObjectFollowPositionOptions positionOptions = ObjectFollowPositionOptions.All;
        public ObjectFollowRotationOptions rotationOptions = ObjectFollowRotationOptions.None;
        public ObjectUpdateOptions updateOptions = ObjectUpdateOptions.LateUpdate;
        public ObjectDeltaTimeOptions deltaTimeOptions = ObjectDeltaTimeOptions.Normal;

        [Range( 0f, 100f )] [Tooltip( "The lerp speed of the follower" )]
        public float movingSpeed = 2f;

        [Range( 0f, 100f )] [Tooltip( "The rotation speed of the follower" )]
        public float rotationSpeed = 2f;

        public Vector3 offSet = Vector3.zero;

        #endregion

        #region Main Thread

        void Update()
        {
            if ( updateOptions == ObjectUpdateOptions.Update ) FollowObject();
        }

        void FixedUpdate()
        {
            if ( updateOptions == ObjectUpdateOptions.FixedUpdate ) FollowObject();
        }

        void LateUpdate()
        {
            if ( updateOptions == ObjectUpdateOptions.LateUpdate ) FollowObject();
        }

        #endregion

        #region Main Methods

        void FollowObject()
        {
            FollowPos();
            FollowRotation();
        }

        void FollowPos()
        {
            var newPos = transformTo.position;

            //check the has off set (if has then add)
            if ( positionOptions == ObjectFollowPositionOptions.All || positionOptions == ObjectFollowPositionOptions.HasOffsetOnly )
                newPos += offSet;

            if ( positionOptions == ObjectFollowPositionOptions.All || positionOptions == ObjectFollowPositionOptions.HasLerpOnly )
                newPos = Vector3.Lerp( transform.position, newPos, GetDeltaTime() * movingSpeed );

            //then just set it
            transform.position = newPos;
        }

        void FollowRotation()
        {
            //if none then just don't do anything
            if ( rotationOptions == ObjectFollowRotationOptions.None )
                return;

            //stores the target rotation
            var newRot = transformTo.rotation;

            //if (objectFollowRotationOptions == ObjectFollowRotationOptions.NoLerp) then do nothing

            if ( rotationOptions == ObjectFollowRotationOptions.HasLerp )
                newRot = Quaternion.Lerp( transform.rotation, newRot, GetDeltaTime() * rotationSpeed );

            if ( newRot != default )
                transform.rotation = newRot;
        }

        float GetDeltaTime()
        {
            switch ( deltaTimeOptions )
            {
                case ObjectDeltaTimeOptions.None: return 1f;
                case ObjectDeltaTimeOptions.Normal: return Time.deltaTime;
                case ObjectDeltaTimeOptions.Fixed: return Time.fixedDeltaTime;

                default: throw ExceptionUtils.NotAccessible;
            }
        }

        #endregion
    }
}
