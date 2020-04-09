using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    #region Vars and enums
    /// <summary> Option flags for the object to follow </summary>
    public enum ObjectFollowOptions
    { All, HasOffsetOnly, HasLerpOnly, None }

    /// <summary> Option flags for the update mode for the follower </summary>
    public enum ObjectUpdateOptions
    { Update, FixedUpdate, LateUpdate }

    [Header("Configuration")]
    public Transform transformTo;
    public ObjectFollowOptions objectMoveWithMode;
    public ObjectUpdateOptions objectUpdateMode;

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
        if (objectUpdateMode == ObjectUpdateOptions.Update)
            FollowObject();
    }
    private void FixedUpdate()
    {
        if (objectUpdateMode == ObjectUpdateOptions.FixedUpdate)
            FollowObject();
    }
    private void LateUpdate()
    {
        if (objectUpdateMode == ObjectUpdateOptions.LateUpdate)
            FollowObject();
    }
    #endregion

    #region MainMethods
    public void FollowObject()
    {
        //this method performs moves to the object using vector3.lerp
        Vector3 newPos = transformTo.position;
        //check the has off set (if has then add)
        if (objectMoveWithMode == ObjectFollowOptions.All || objectMoveWithMode == ObjectFollowOptions.HasOffsetOnly)
            newPos += offSet;

        if (objectMoveWithMode == ObjectFollowOptions.All || objectMoveWithMode == ObjectFollowOptions.HasLerpOnly)
            newPos = Vector3.Lerp(transform.position, newPos, 1 / (100f - MovingSpeed));

        //then just set it
        transform.position = newPos;
    }
    #endregion

}
