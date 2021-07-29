using System;
using UnityEngine;

namespace CXUtils.CodeUtils
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
        static readonly Vector2 leftDown = Vector2.zero;
        static readonly Vector2 leftUp = new Vector2( 0, 1 );
        static readonly Vector2 rightUp = new Vector2( 1, 1 );
        static readonly Vector2 rightDown = new Vector2( 1, 0 );

        //middles
        static readonly Vector2 middleUp = new Vector2( .5f, 1f );
        static readonly Vector2 middleDown = new Vector2( .5f, 0 );
        static readonly Vector2 middleLeft = new Vector2( 0, .5f );
        static readonly Vector2 middleRight = new Vector2( 1f, .5f );

        //center
        static readonly Vector2 MiddleCenter = new Vector2( .5f, .5f );

        ///<summary> This method will get the edges of the camera and return the edge camera pos (only for ortho) </summary>
        public static Vector3 GetCameraPortPosOnWorldPosOrtho( this Camera camera, PortType port )
        {
            switch ( port )
            {
                case PortType.LeftUp: return camera.ViewportToWorldPoint( leftUp );
                case PortType.LeftDown: return camera.ViewportToWorldPoint( leftDown );

                case PortType.RightUp: return camera.ViewportToWorldPoint( rightUp );
                case PortType.RightDown: return camera.ViewportToWorldPoint( rightDown );


                case PortType.UpMiddle: return camera.ViewportToWorldPoint( middleUp );
                case PortType.DownMiddle: return camera.ViewportToWorldPoint( middleDown );

                case PortType.LeftMiddle: return camera.ViewportToWorldPoint( middleLeft );
                case PortType.RightMiddle: return camera.ViewportToWorldPoint( middleRight );

                case PortType.Center: return camera.ViewportToScreenPoint( MiddleCenter );
            }

            throw ExceptionUtils.Error.NotAccessible;
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
