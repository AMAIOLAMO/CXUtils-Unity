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
        ///     Options for camera anchors
        /// </summary>
        public enum AnchorType
        {
            TopLeft, BottomLeft, TopRight,
            BottomRight, LeftCenter, RightCenter,
            TopCenter, BottomCenter, Center
        }

        #region MousePosition

        ///<summary> This method will get the mouse position on the scene position on the camera </summary>
        public static Vector3 GetMouseGlobal( this Camera camera ) => camera.ScreenToWorldPoint( Input.mousePosition );

        /// <inheritdoc cref="GetMouseViewport" />
        public static Vector3 GetMouseGlobal() => GetMouseGlobal( Camera.main );

        ///<summary> This method will get the mouse position on the viewport pos on the camera </summary>
        public static Vector3 GetMouseViewport( this Camera camera ) => camera.ScreenToViewportPoint( Input.mousePosition );

        /// <summary> Get's the Ray that shoots out with the current mouse position </summary>
        public static Ray GetRayByMouse( this Camera camera ) => camera.ScreenPointToRay( Input.mousePosition );

        /// <summary> Receives the raycastHit info with the mouse position using the given camera </summary>
        public static bool GetRaycastHitByMouse( this Camera camera, out RaycastHit raycastHit, float maxDistance = float.PositiveInfinity,
            int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default ) =>
            Physics.Raycast( camera.GetRayByMouse(), out raycastHit, maxDistance, layerMask, queryTriggerInteraction );

        #endregion

        #region CameraOtherHelperMethods

        //corners
        static readonly Float2 bottomLeft  = Float2.Zero;
        static readonly Float2 topLeft     = new Float2( 0, 1 );
        static readonly Float2 rightTop    = new Float2( 1, 1 );
        static readonly Float2 rightBottom = new Float2( 1 );

        //centers
        static readonly Float2 centerTop   = new Float2( .5f, 1f );
        static readonly Float2 centerDown  = new Float2( .5f );
        static readonly Float2 centerLeft  = new Float2( 0, .5f );
        static readonly Float2 centerRight = new Float2( 1f, .5f );

        static readonly Float2 center = new Float2( .5f, .5f );

        ///<summary> This method will get the edges of the camera and return the edge camera pos (only for ortho) </summary>
        public static Vector3 GlobalOrthoByPort( this Camera camera, AnchorType anchor )
        {
            switch ( anchor )
            {
                case AnchorType.TopLeft:    return camera.ViewportToWorldPoint( topLeft.ToUnity() );
                case AnchorType.BottomLeft: return camera.ViewportToWorldPoint( bottomLeft.ToUnity() );

                case AnchorType.TopRight:    return camera.ViewportToWorldPoint( rightTop.ToUnity() );
                case AnchorType.BottomRight: return camera.ViewportToWorldPoint( rightBottom.ToUnity() );


                case AnchorType.TopCenter:    return camera.ViewportToWorldPoint( centerTop.ToUnity() );
                case AnchorType.BottomCenter: return camera.ViewportToWorldPoint( centerDown.ToUnity() );

                case AnchorType.LeftCenter:  return camera.ViewportToWorldPoint( centerLeft.ToUnity() );
                case AnchorType.RightCenter: return camera.ViewportToWorldPoint( centerRight.ToUnity() );

                case AnchorType.Center: return camera.ViewportToWorldPoint( center.ToUnity() );
            }

            throw ExceptionUtils.NotAccessible;
        }

        ///<summary> Get's the Vector2 border in world space </summary>
        public static Bounds GetBoundsOrtho( this Camera camera )
        {
            if ( !camera.orthographic )
                throw new ArgumentException( $"{camera.name} is not orthographic", nameof( camera.orthographic ) );

            //getting the border of the real world space
            var borderPositive = new Vector2(
                GlobalOrthoByPort( camera, AnchorType.RightCenter ).x,
                GlobalOrthoByPort( camera, AnchorType.TopCenter ).y
            );

            return new Bounds( camera.transform.position, borderPositive );
        }

        #endregion
    }
}
