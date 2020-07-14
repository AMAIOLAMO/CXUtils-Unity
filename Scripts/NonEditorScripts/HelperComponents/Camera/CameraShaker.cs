using System;
using UnityEngine;
using CXUtils.CodeUtils;

namespace CXUtils.HelperComponents
{
    /// <summary> A simple camera shaker </summary>
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private CameraShake cameraShake;

        #region Main Thread

        private void Awake()
        {
            if (cameraShake.ShakeTransform == null)
                cameraShake = new CameraShake(transform, cameraShake.ShakeMax);
        }

        #endregion

        #region Script Utils

        /// <summary> Starts to shake to a position </summary> 
        public void StartShake(float time)
        {
            cameraShake.StopShake(this);
            cameraShake.StartShake(this, time);
        }

        /// <summary> Starts to shake to a position </summary>
        public void StartShake(Vector3 origin, float time)
        {
            cameraShake.StopShake(this);
            cameraShake.StartShake(this, origin, time);
        }

        #endregion
    }
}