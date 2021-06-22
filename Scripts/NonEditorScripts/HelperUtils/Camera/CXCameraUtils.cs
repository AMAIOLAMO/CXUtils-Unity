using System;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace CXUtils.CodeUtils
{
    ///<summary> Cx's Camera Class </summary>
    public static class CameraUtils
    {

        ///<summary> Options for camera ports </summary>
        public enum PortOptions
        { LeftUp, LeftDown, RightUp, RightDown, LeftMiddle, RightMiddle, UpMiddle, DownMiddle, Center }

        #region MousePosition

        ///<summary> This method will get the mouse position on the scene position on the camera </summary>
        public static Vector3 GetMouseOnWorldPos( this Camera camera ) => camera.ScreenToWorldPoint( Input.mousePosition );

        ///<inheritdoc cref="GetMouseOnViewPortPos(Camera)"/>
        public static Vector3 GetMouseOnWorldPos() => GetMouseOnWorldPos( Camera.main );

        ///<summary> This method will get the mouse position on the viewport pos on the camera </summary>
        public static Vector3 GetMouseOnViewPortPos( this Camera camera ) => camera.ScreenToViewportPoint( Input.mousePosition );

        /// <summary> Get's the Ray that shoots out with the current mouse position </summary>
        public static Ray GetRayWithMousePos( this Camera camera ) => camera.ScreenPointToRay( Input.mousePosition );

        /// <summary> Recieves the raycasthit info with the mouse position using the given camera </summary>
        public static bool GetRaycastHitWithMousePos( this Camera camera, out RaycastHit raycastHit, float maxDistance = float.PositiveInfinity,
            int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default ) =>
            Physics.Raycast( camera.GetRayWithMousePos(), out raycastHit, maxDistance, layerMask, queryTriggerInteraction );

        #endregion

        #region CameraOtherHelperMethods

        //corners
        private static readonly Vector2 LeftDown = Vector2.zero;
        private static readonly Vector2 LeftUp = new Vector2( 0, 1 );
        private static readonly Vector2 RightUp = new Vector2( 1, 1 );
        private static readonly Vector2 RightDown = new Vector2( 1, 0 );

        //middles
        private static readonly Vector2 MiddleUp = new Vector2( .5f, 1f );
        private static readonly Vector2 MiddleDown = new Vector2( .5f, 0 );
        private static readonly Vector2 MiddleLeft = new Vector2( 0, .5f );
        private static readonly Vector2 MiddleRight = new Vector2( 1f, .5f );

        //center
        private static readonly Vector2 MiddleCenter = new Vector2( .5f, .5f );

        ///<summary> This method will get the edges of the camera and return the edge camera pos (only for ortho) </summary>
        public static Vector3 GetCameraPortPosOnWorldPos_Ortho( this Camera camera, PortOptions port )
        {
            switch ( port )
            {
                case PortOptions.LeftUp:
                return camera.ViewportToWorldPoint( LeftUp );
                case PortOptions.LeftDown:
                return camera.ViewportToWorldPoint( LeftDown );

                case PortOptions.RightUp:
                return camera.ViewportToWorldPoint( RightUp );
                case PortOptions.RightDown:
                return camera.ViewportToWorldPoint( RightDown );


                case PortOptions.UpMiddle:
                return camera.ViewportToWorldPoint( MiddleUp );
                case PortOptions.DownMiddle:
                return camera.ViewportToWorldPoint( MiddleDown );

                case PortOptions.LeftMiddle:
                return camera.ViewportToWorldPoint( MiddleLeft );
                case PortOptions.RightMiddle:
                return camera.ViewportToWorldPoint( MiddleRight );


                case PortOptions.Center:
                return camera.ViewportToScreenPoint( MiddleCenter );
            }

            throw ExceptionUtils.Error.NotAccessible;
            //throw ExceptionUtils.GetException( ErrorType.NotAccessible );
        }

        ///<summary> Get's the Vector2 border in world space </summary>
        public static Bounds GetCameraBounds_Vec2_Ortho( this Camera camera )
        {
            if ( !camera.orthographic )
                throw new ArgumentException( $"{camera.name} is not orthographic! please turn on orthographic in order to use this method!", nameof( camera.orthographic ) );

            //getting the border of the real world space
            var BorderPositive = new Vector2( GetCameraPortPosOnWorldPos_Ortho( camera, PortOptions.RightMiddle ).x, GetCameraPortPosOnWorldPos_Ortho( camera, PortOptions.UpMiddle ).y );

            return new Bounds( camera.transform.position, BorderPositive );
        }

        #endregion
    }

    /// <summary>
    /// A helper library for handling camera shake
    /// </summary>
    [Serializable]
    public struct CameraShake
    {
        [SerializeField] private Transform shakeTransform;
        [SerializeField] private float shakeMin, shakeMax;

        /// <summary> Triggers when start shaking, EventArgs: original Position (Center camera shake) </summary>
        public event EventHandler<Vector3> Trigger_StartShake;

        /// <summary> Triggers while shakeing, EventArgs: shaking offset </summary> </summary>
        public event EventHandler<Vector3> Trigger_WhileShake;

        public float ShakeMin => shakeMin;
        public float ShakeMax => shakeMax;
        public Transform ShakeTransform => shakeTransform;

        #region Constructors

        public CameraShake( Transform shakeTransform, float shakeRadius,
            EventHandler<Vector3> startShakeEvent = null, EventHandler<Vector3> whileShakeEvent = null )
        {
            this.shakeTransform = shakeTransform;

            if ( shakeRadius <= 0 )
                throw new ArgumentOutOfRangeException( "Shake radius must be bigger than 0!", nameof( shakeRadius ) );

            shakeMin = -shakeRadius;
            shakeMax = shakeRadius;

            //events
            Trigger_StartShake = startShakeEvent;
            Trigger_WhileShake = whileShakeEvent;
        }

        public CameraShake( Transform shakeTransform, float shakeMin, float shakeMax,
            EventHandler<Vector3> startShakeEvent = null, EventHandler<Vector3> whileShakeEvent = null )
        {
            this.shakeTransform = shakeTransform;
            this.shakeMin = shakeMin;
            this.shakeMax = shakeMax;

            Trigger_StartShake = startShakeEvent;
            Trigger_WhileShake = whileShakeEvent;
        }

        #endregion

        #region MainScript methods

        /// <summary> Starts to shake the given transform </summary>
        public void StartShake( MonoBehaviour monBhav, Vector3 origin, float time ) =>
            monBhav.StartCoroutine( Shake( origin, time ) );

        /// <summary> Starts to shake the given transform </summary>
        public void StartShake( MonoBehaviour monBhav, Transform origin, float time ) =>
            monBhav.StartCoroutine( Shake( origin, time ) );

        /// <summary> Starts to shake the given transform </summary>
        public void StartShake( MonoBehaviour monBhav, float time ) =>
            StartShake( monBhav, monBhav.transform.position, time );

        /// <summary> Shakes once </summary>
        public void Shake( Vector3 origin ) =>
            ShakeTransform.position = origin + GenerateShakeVec();

        /// <summary> Shakes once </summary>
        public void Shake( Transform origin ) =>
            Shake( origin.position );

        /// <summary> Stop the shake of the given transform </summary>
        public void StopShake( MonoBehaviour monBhav ) =>
            monBhav.StopCoroutine( "Shake" );

        #endregion

        #region Private Methods

        //Transform origin
        private IEnumerator Shake( Transform origin, float time )
        {
            float currentTime = time;
            Vector3 newOffset;

            Trigger_StartShake?.Invoke( this, origin.position );

            while ( currentTime > 0 )
            {
                newOffset = GenerateShakeVec();
                ShakeTransform.position = origin.position + newOffset;
                Trigger_WhileShake?.Invoke( this, newOffset );

                currentTime -= Time.deltaTime;
                yield return null;
            }

            ShakeTransform.position = origin.position;
        }

        //Vector origin
        private IEnumerator Shake( Vector3 origin, float time )
        {
            float currentTime = time;
            Vector3 newOffset;

            Trigger_StartShake?.Invoke( this, origin );

            while ( currentTime > 0 )
            {
                newOffset = GenerateShakeVec();
                ShakeTransform.position = origin + newOffset;
                Trigger_WhileShake?.Invoke( this, newOffset );

                currentTime -= Time.deltaTime;
                yield return null;
            }

            ShakeTransform.position = origin;
        }

        private Vector3 GenerateShakeVec() =>
            VectorUtils.RandomVec3( ShakeMin, ShakeMax );

        #endregion
    }
}