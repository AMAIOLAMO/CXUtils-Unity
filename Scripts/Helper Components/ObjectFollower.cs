using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    #region Vars and enums
    /// <summary> The object moving mode </summary>
    public enum ObjectFollowMode
    { All, HasOffsetOnly, HasLerpOnly, None }

    /// <summary> The update mode for this object </summary>
    public enum ObjectUpdateMode
    { Update, FixedUpdate, LateUpdate }

    [Header("Configuration")]
    public Transform transformTo;
    public ObjectFollowMode objectMoveWithMode;
    public ObjectUpdateMode objectUpdateMode;

    [Range(1f, 100f), Tooltip("The lerp speed of the object")]
    public float MovingSpeed = 1f;

    [HideInInspector]
    public Vector3 offSet = Vector3.zero;
    #endregion

    #region UnityMethods
    private void Start() =>
        offSet = transform.position - transformTo.position;

    private void Update()
    {
        if (objectUpdateMode == ObjectUpdateMode.Update)
            PerformMoveToOBJ();
    }
    private void FixedUpdate()
    {
        if (objectUpdateMode == ObjectUpdateMode.FixedUpdate)
            PerformMoveToOBJ();
    }
    private void LateUpdate()
    {
        if (objectUpdateMode == ObjectUpdateMode.LateUpdate)
            PerformMoveToOBJ();
    }
    #endregion

    #region MainMethods
    public void PerformMoveToOBJ()
    {
        //this method performs moves to the object using vector3.lerp
        Vector3 newPos = transformTo.position;
        //check the has off set (if has then add)
        if (objectMoveWithMode == ObjectFollowMode.All || objectMoveWithMode == ObjectFollowMode.HasOffsetOnly)
            newPos += offSet;
        //check if there is lerp
        if (objectMoveWithMode == ObjectFollowMode.All || objectMoveWithMode == ObjectFollowMode.HasLerpOnly)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, 1 / (100f - MovingSpeed));
            return;
        }

        //then just set it
        transform.position = newPos;
    }
    #endregion
}
