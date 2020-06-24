using UnityEngine;
using System;
using System.Collections;

namespace CXUtils.CodeUtils
{
    ///<summary> Cx's Camera Class </summary>
    public class CameraUtils
    {
        ///<summary> Options for camera ports </summary>
        public enum PortOptions
        {
            LeftUp, LeftDown, RightUp, RightDown, LeftMiddle, RightMiddle, UpMiddle, DownMiddle
        }

        #region MousePosition
        ///<summary> This method will get the mouse position on the scene position on the camera </summary>
        public static Vector3 GetMouseOnWorldPos() => GetMouseOnWorldPos(Camera.main);

        ///<summary> This method will get the mouse position on the scene position on the camera </summary>
        public static Vector3 GetMouseOnWorldPos(Camera camera) => camera.ScreenToWorldPoint(Input.mousePosition);

        ///<summary> This method will get the mouse position on the viewport pos on the camera </summary>
        public static Vector3 GetMouseOnViewPortPos(Camera camera) => camera.ScreenToViewportPoint(Input.mousePosition);
        
        #endregion


        #region CameraOtherHelperMethods
        
        ///<summary> This method will get the edges of the camera and return the edge camera pos </summary>
        public static Vector3 GetCameraPortPosOnWorldPos(Camera camera, PortOptions port)
        {
            #region postions

            //positions
            Vector2 LU = new Vector2(0, camera.pixelHeight);
            Vector2 LD = new Vector2(0, 0);
            Vector2 RU = new Vector2(camera.pixelWidth, camera.pixelHeight);
            Vector2 RD = new Vector2(camera.pixelWidth, 0);
            //middles
            Vector2 MU = new Vector2(camera.pixelWidth / 2, camera.pixelHeight);
            Vector2 MD = new Vector2(camera.pixelWidth / 2, 0);
            Vector2 ML = new Vector2(0, camera.pixelHeight / 2);
            Vector2 MR = new Vector2(camera.pixelWidth, camera.pixelHeight / 2);
            
            #endregion

            switch (port)
            {
                case PortOptions.LeftUp:
                return camera.ScreenToWorldPoint(LU);
                
                case PortOptions.LeftDown:
                return camera.ScreenToWorldPoint(LD);
                
                case PortOptions.RightUp:
                return camera.ScreenToWorldPoint(RU);
                
                case PortOptions.RightDown:
                return camera.ScreenToWorldPoint(RD);
                
                case PortOptions.UpMiddle:
                return camera.ScreenToWorldPoint(MU);
                
                case PortOptions.DownMiddle:
                return camera.ScreenToWorldPoint(MD);
                
                case PortOptions.LeftMiddle:
                return camera.ScreenToWorldPoint(ML);
                
                default: // PortOptions.RightMiddle
                return camera.ScreenToWorldPoint(MR);
            }
        }

        ///<summary> Get's the Vector2 border in world space </summary>
        public static Vector2 GetCameraBounds_Vec2_Ortho(Camera camera)
        {
            if (!camera.orthographic)
                throw new Exception($"{camera.name} is not orthographic! please turn on orthographic in order to use this method!");

            //getting the border of the real world space
            Vector2 BorderPositive = new Vector2(GetCameraPortPosOnWorldPos(camera, PortOptions.RightMiddle).x, GetCameraPortPosOnWorldPos(camera, PortOptions.UpMiddle).y);
            return BorderPositive - (Vector2)camera.transform.position;
        }

        ///<summary> Get's the border in world space </summary>
        public static Bounds GetCameraBorders_Ortho(Camera camera) =>
            new Bounds(camera.transform.position, GetCameraBounds_Vec2_Ortho(camera) * 2);

        #endregion
    }

    /// <summary> A helper library for handling camera shake </summary>
    [Serializable]
    struct CameraShake : IDebugDescribable
    {
        #region Properties
        [SerializeField] private readonly Transform shakeTransform;
        [SerializeField] private readonly float shakeMin, shakeMax;
        public Transform ShakeTransform => shakeTransform;
        public float ShakeMin => shakeMin;
        public float ShakeMax => shakeMax;

        /// <summary> Triggers when start shaking, EventArgs: original Position (Center camera shake) </summary>
        public event EventHandler<Vector3> Trigger_StartShake;

        /// <summary> Triggers while shakeing, EventArgs: shaking offset </summary> </summary>
        public event EventHandler<Vector3> Trigger_WhileShake;

        #endregion

        #region Constructors
        
        public CameraShake(Transform shakeTransform, float shakeRadius,
            EventHandler<Vector3> startShakeEvent = null, EventHandler<Vector3> whileShakeEvent = null)
        {
            this.shakeTransform = shakeTransform;

            if (shakeRadius < 0)
                DebugUtils.LogError<Exception>(shakeTransform, "Value Invalid!");
            
            shakeMin = -shakeRadius;
            shakeMax = shakeRadius;
            
            //events
            Trigger_StartShake = startShakeEvent;
            Trigger_WhileShake = whileShakeEvent;
        }

        public CameraShake(Transform shakeTransform, float shakeMin, float shakeMax,
            EventHandler<Vector3> startShakeEvent = null, EventHandler<Vector3> whileShakeEvent = null)
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
        public void StartShake(MonoBehaviour monBhav, Vector3 origin, float time) =>
            monBhav.StartCoroutine(Shake(origin, time));

        /// <summary> Starts to shake the given transform </summary> 
        public void StartShake(MonoBehaviour monBhav, Transform origin, float time) =>
            monBhav.StartCoroutine(Shake(origin, time));

        /// <summary> Starts to shake the given transform </summary>
        public void StartShake(MonoBehaviour monBhav, float time) =>
            StartShake(monBhav, monBhav.transform.position, time);

        /// <summary> Stop the shake of the given transform </summary>
        public void StopShake(MonoBehaviour monBhav) =>
            monBhav.StopCoroutine("Shake");
            
        #endregion

        #region Struct Methods
        //Transform origin
        private IEnumerator Shake(Transform origin, float time)
        {
            float currentTime = time;
            Vector3 newOffset;

            Trigger_StartShake?.Invoke(this, origin.position);

            while (currentTime > 0)
            {
                newOffset = GenerateShakeVec();
                ShakeTransform.position = origin.position + newOffset;
                Trigger_WhileShake?.Invoke(this, newOffset);

                currentTime -= Time.deltaTime;
                yield return null;
            }

            ShakeTransform.position = origin.position;
        }

        //Vector origin
        private IEnumerator Shake(Vector3 origin, float time)
        {
            float currentTime = time;
            Vector3 newOffset;

            Trigger_StartShake?.Invoke(this, origin);

            while (currentTime > 0)
            {
                newOffset = GenerateShakeVec();
                ShakeTransform.position = origin + newOffset;
                Trigger_WhileShake?.Invoke(this, newOffset);

                currentTime -= Time.deltaTime;
                yield return null;
            }

            ShakeTransform.position = origin;
        }

        private Vector3 GenerateShakeVec() =>
            new Vector3(UnityEngine.Random.Range(ShakeMin, ShakeMax),
                UnityEngine.Random.Range(ShakeMin, ShakeMax),
                UnityEngine.Random.Range(ShakeMin, ShakeMax));

        public string DebugDescribe() =>
            $"Camera Shake: shaking: {shakeTransform.ToString() }\nShake Range: {shakeMin} ~ {shakeMax}";
        #endregion

    }
}