using System;
using CXUtils.Domain.Types;
using CXUtils.Unity;
using UnityEngine;

namespace CXUtils.Common
{
    /// <summary>
    ///     Camera utilities for unity
    /// </summary>
    public static class CameraUtils
    {
        /// <summary>
        ///     Options for camera ports
        /// </summary>
        public enum PortAnchorType
        {
            TopLeft, BottomLeft, TopRight,
            BottomRight, LeftCenter, RightCenter,
            TopCenter, BottomCenter, Center
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
        static readonly Float2 _bottomLeft  = Float2.Zero;
        static readonly Float2 _topLeft     = new Float2( 0, 1 );
        static readonly Float2 _rightTop    = new Float2( 1, 1 );
        static readonly Float2 _rightBottom = new Float2( 1 );

        //centers
        static readonly Float2 _centerTop   = new Float2( .5f, 1f );
        static readonly Float2 _centerDown  = new Float2( .5f );
        static readonly Float2 _centerLeft  = new Float2( 0, .5f );
        static readonly Float2 _centerRight = new Float2( 1f, .5f );

        static readonly Float2 _center = new Float2( .5f, .5f );

        ///<summary> This method will get the edges of the camera and return the edge camera pos (only for ortho) </summary>
        public static Vector3 GetCameraPortPosOnWorldPosOrtho( this Camera camera, PortAnchorType portAnchor )
        {
            switch ( portAnchor )
            {
                case PortAnchorType.TopLeft:    return camera.ViewportToWorldPoint( _topLeft.ToUnity() );
                case PortAnchorType.BottomLeft: return camera.ViewportToWorldPoint( _bottomLeft.ToUnity() );

                case PortAnchorType.TopRight:    return camera.ViewportToWorldPoint( _rightTop.ToUnity() );
                case PortAnchorType.BottomRight: return camera.ViewportToWorldPoint( _rightBottom.ToUnity() );


                case PortAnchorType.TopCenter:    return camera.ViewportToWorldPoint( _centerTop.ToUnity() );
                case PortAnchorType.BottomCenter: return camera.ViewportToWorldPoint( _centerDown.ToUnity() );

                case PortAnchorType.LeftCenter:  return camera.ViewportToWorldPoint( _centerLeft.ToUnity() );
                case PortAnchorType.RightCenter: return camera.ViewportToWorldPoint( _centerRight.ToUnity() );

                case PortAnchorType.Center: return camera.ViewportToWorldPoint( _center.ToUnity() );
            }

            throw ExceptionUtils.NotAccessible;
        }

        ///<summary> Get's the Vector2 border in world space </summary>
        public static Bounds GetCameraBoundsOrtho( this Camera camera )
        {
            if ( !camera.orthographic )
                throw new ArgumentException( $"{camera.name} is not orthographic! please turn on orthographic in order to use this method!", nameof( camera.orthographic ) );

            //getting the border of the real world space
            var borderPositive = new Vector2(
                GetCameraPortPosOnWorldPosOrtho( camera, PortAnchorType.RightCenter ).x,
                GetCameraPortPosOnWorldPosOrtho( camera, PortAnchorType.TopCenter ).y
            );

            return new Bounds( camera.transform.position, borderPositive );
        }

        #endregion
    }
}
