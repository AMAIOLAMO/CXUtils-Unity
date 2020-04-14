using CXUtils.CXCamera;
using UnityEngine;

/// <summary> A simple camera shaker </summary>
public class CameraShaker : MonoBehaviour
{
    [SerializeField] private CameraShake cameraShake;

    private void Awake()
    {
        if (cameraShake.ShakeTransform == null)
            cameraShake = new CameraShake(transform, cameraShake.ShakeMax);
    }

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
}
