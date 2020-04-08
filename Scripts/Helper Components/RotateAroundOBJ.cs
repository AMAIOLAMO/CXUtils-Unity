using UnityEngine;

public class RotateAroundOBJ : MonoBehaviour
{
    public Transform RotateAroundTrans;
    public float RotateSpeedPerAngle = 1f;

    private void Update() =>
        PerfromCircularRotation();

    private void PerfromCircularRotation() =>
        transform.RotateAround(RotateAroundTrans.position, RotateAroundTrans.forward, RotateSpeedPerAngle);
}
