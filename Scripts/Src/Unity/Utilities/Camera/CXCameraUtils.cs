using System;
using CXUtils.Domain.Types;
using CXUtils.Unity;
using UnityEngine;

namespace CXUtils.Common
{
    ///<summary> Cx's Camera Class </summary>
    public static class CameraUtils
    {
        ///<summary> Options for camera ports </summary>
        public enum PortType
        {
            LeftUp, LeftDown, RightUp,
            RightDown, LeftMiddle, RightMiddle,
            UpMiddle, DownMiddle, Center
        }

        #region MousePosition

        ///<summary> This method will get the mouse position on the scene position on the camera </summary>
        public static Vector3 GetMouseOnWorldPos( this Camera camera ) => camera.ScreenToWorldPoint( Input.mousePosition );

        /// <inheritdoc cref="GetMouseOnViewPortPos(Camera)" />
        public static Vector3 GetMouseOnWorldPos() => GetMouseOnWorldPos( Camera.main );

        ///<summary> This method will get the mouse position on the viewport pos on the camera </summary>
        public static Vector3 GetMouseOnViewPortPos( this Camera camera ) => camera.ScreenToViewportPoint( Input.mousePosition );

        /// <summary> Get's the Ray that shoots out with the current mouse position </summary>
        public static Ray GetRayFromMousePos( this Camera camera ) => camera.ScreenPointToRay( Input.mousePosition );

        /// <summary> Receives the raycastHit info with the mouse position using the given camera </summary>
        public static bool GetRaycastHitWithMousePos( this Camera camera, out RaycastHit raycastHit, float maxDistance = float.PositiveInfinity,
            int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default ) =>
            Physics.Raycast( camera.GetRayFromMousePos(), out raycastHit, maxDistance, layerMask, queryTriggerInteraction );

        #endregion

        #region CameraOtherHelperMethods

        //corners
        static readonly Float2 _leftDown  = Float2.Zero;
        static readonly Float2 _leftUp    = new Float2( 0, 1 );
        static readonly Float2 _rightUp   = new Float2( 1, 1 );
        static readonly Float2 _rightDown = new Float2( 1 );
        //middles
        static readonly Float2 _middleUp    = new Float2( .5f, 1f );
        static readonly Float2 _middleDown  = new Float2( .5f );
        static readonly Float2 _middleLeft  = new Float2( 0, .5f );
        static readonly Float2 _middleRight = new Float2( 1f, .5f );
        //center
        static readonly Float2 _middleCenter = new Float2( .5f, .5f );

        ///<summary> This method will get the edges of the camera and return the edge camera pos (only for ortho) </summary>
        public static Vector3 GetCameraPortPosOnWorldPosOrtho( this Camera camera, PortType port )
        {
            switch ( port )
            {
                case PortType.LeftUp:   return camera.ViewportToWorldPoint( _leftUp.ToUnity() );
                case PortType.LeftDown: return camera.ViewportToWorldPoint( _leftDown.ToUnity() );

                case PortType.RightUp:   return camera.ViewportToWorldPoint( _rightUp.ToUnity() );
                case PortType.RightDown: return camera.ViewportToWorldPoint( _rightDown.ToUnity() );


                case PortType.UpMiddle:   return camera.ViewportToWorldPoint( _middleUp.ToUnity() );
                case PortType.DownMiddle: return camera.ViewportToWorldPoint( _middleDown.ToUnity() );

                case PortType.LeftMiddle:  return camera.ViewportToWorldPoint( _middleLeft.ToUnity() );
                case PortType.RightMiddle: return camera.ViewportToWorldPoint( _middleRight.ToUnity() );

                case PortType.Center: return camera.ViewportToWorldPoint( _middleCenter.ToUnity() );
            }

            throw ExceptionUtils.NotAccessible;
        }

        ///<summary> Get's the Vector2 border in world space </summary>
        public static Bounds GetCameraBoundsFloat2Ortho( this Camera camera )
        {
            if ( !camera.orthographic )
                throw new ArgumentException( $"{camera.name} is not orthographic! please turn on orthographic in order to use this method!", nameof( camera.orthographic ) );

            //getting the border of the real world space
            var borderPositive = new Vector2( GetCameraPortPosOnWorldPosOrtho( camera, PortType.RightMiddle ).x, GetCameraPortPosOnWorldPosOrtho( camera, PortType.UpMiddle ).y );

            return new Bounds( camera.transform.position, borderPositive );
        }

        #endregion
    }
}
