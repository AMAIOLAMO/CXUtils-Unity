using System;
using CXUtils.CodeUtils;
using UnityEngine;
using UnityEngine.CXExtensions;

namespace CXUtils.HelperComponents
{
    /// <summary> A Character controller for 2Dimention games </summary>
    [AddComponentMenu( "CXUtils/Player/2D/CharacterController2D" )]
    public class CharacterController2D : MonoBehaviour
    {

        #region ThreadMethods

        void CheckAndMove( MovementUpdateOptions moveUpdateOp )
        {
            if ( moveUpdateOptions == moveUpdateOp )
            {
                GetMovements();
                if ( gamePerspecOptions == GamePerspectiveOptions.Platformer )
                    MovePlayerPlatformer();

                else if ( gamePerspecOptions == GamePerspectiveOptions.TopDown )
                    MovePlayerTopDown();
            }
        }

        #endregion
        #region Enums

        /// <summary> How the Character controller works </summary>
        public enum GamePerspectiveOptions
        {
            Platformer, TopDown
        }

        /// <summary> The options of how the movement will be updated in this character controller </summary>
        public enum MovementUpdateOptions
        {
            Update, FixedUpdate, LateUpdate
        }

        /// <summary> The options of how the movement will be updated using the Time frame delta </summary>
        public enum MovementDeltaTimeOptions
        {
            DeltaTime, UnscaledDeltaTime, FixedDeltaTime, FixedUnscaledDeltaTime
        }

        /// <summary> The modes of how the movmement will be calculated </summary>
        public enum MovementMode
        {
            Position, Velocity, Force
        }

        #endregion

        #region Vars and fields

        #region Configurations

        [Header( "Configuration" )]
        [Header( "Requirements" )]
        [NotNull]
        [SerializeField]
        Rigidbody2D playerRigidBody;

        [HideInInspector]
        [SerializeField]
        CharacterGroundCheck2D characterGroundCheck;

        [Header( "Options" )]
        [SerializeField]
        GamePerspectiveOptions gamePerspecOptions = GamePerspectiveOptions.Platformer;
        [SerializeField] MovementUpdateOptions moveUpdateOptions = MovementUpdateOptions.Update;
        [SerializeField] MovementDeltaTimeOptions moveDeltaTimeOptions = MovementDeltaTimeOptions.DeltaTime;
        [SerializeField] MovementMode moveMode = MovementMode.Position;

        [Header( "Player Settings" )]
        [SerializeField]
        float playerCurrentSpeed = 5f;

        [SerializeField] bool isLocalTransform;

        [HideInInspector]
        [SerializeField]
        float playerCurrentJumpStrength = 5f;

        [SerializeField] bool isMovementNormalized;

        #endregion

        protected Vector2 movementVector_Raw;

        public Rigidbody2D PlayerRigidBody { get => playerRigidBody; set => playerRigidBody = value; }

        public CharacterGroundCheck2D CharacterGroundCheck { get => characterGroundCheck; set => characterGroundCheck = value; }

        public event Action PlayerStartJump;

        #region Option Properties

        public GamePerspectiveOptions GamePerspecOptions { get => gamePerspecOptions; set => gamePerspecOptions = value; }
        public MovementUpdateOptions MoveUpdateOptions { get => moveUpdateOptions; set => moveUpdateOptions = value; }
        public MovementDeltaTimeOptions MoveDeltaTimeOptions { get => moveDeltaTimeOptions; set => moveDeltaTimeOptions = value; }
        public MovementMode MoveMode { get => moveMode; set => moveMode = value; }

        #endregion

        public Vector2 MovementVector
        {
            get
            {
                if ( isMovementNormalized )
                    return movementVector_Raw.normalized;

                return movementVector_Raw;
            }
        }

        public float PlayerCurrentSpeed { get => playerCurrentSpeed; set => playerCurrentSpeed = value; }
        public float PlayerCurrentJumpStrength { get => playerCurrentJumpStrength; set => playerCurrentJumpStrength = value; }

        public bool IsLocalTransform { get => isLocalTransform; set => isLocalTransform = value; }
        public bool IsMovementNormalized { get => isMovementNormalized; set => isMovementNormalized = value; }

        #endregion

        #region MainThread

        void Update()
        {
            CheckAndMove( MovementUpdateOptions.Update );
        }

        void FixedUpdate()
        {
            CheckAndMove( MovementUpdateOptions.FixedUpdate );
        }

        void LateUpdate()
        {
            CheckAndMove( MovementUpdateOptions.LateUpdate );
        }

        #endregion

        #region MainMethods

        #region MovementInputs

        protected virtual void GetMovements()
        {
            movementVector_Raw = new Vector2( Input.GetAxisRaw( "Horizontal" ), Input.GetAxisRaw( "Vertical" ) );
        }

        #endregion

        #region Movements

        protected virtual void MovePlayerPlatformer()
        {
            switch ( moveMode )
            {
                case MovementMode.Position:
                    Platformer_Position();
                    break;
                case MovementMode.Force:
                    Platformer_Force();
                    break;
                case MovementMode.Velocity:
                    Platformer_Velocity();
                    break;

                default: throw ExceptionUtils.Error.NotAccessible;
            }

            JumpMovements();
        }

        protected virtual void MovePlayerTopDown()
        {
            switch ( moveMode )
            {
                case MovementMode.Position:
                    TopDown_Position();
                    break;
                case MovementMode.Force:
                    TopDown_Force();
                    break;
                case MovementMode.Velocity:
                    TopDown_Velocity();
                    break;

                default: throw ExceptionUtils.Error.NotAccessible;
            }
        }

        #endregion

        #region PlatformerMovement

        protected virtual void Platformer_Position()
        {
            var newOffset = ( IsLocalTransform ? transform.right : Vector3.right ) * MovementVector.x;

            newOffset *= playerCurrentSpeed * CurrentUsingDeltaTime();
            transform.position += newOffset;
        }

        protected virtual void Platformer_Force()
        {
            Vector2 newForce = ( IsLocalTransform ? transform.right : Vector3.right ) * MovementVector.x;

            newForce *= playerCurrentSpeed;

            playerRigidBody.AddForce( newForce, ForceMode2D.Impulse );
        }

        protected virtual void Platformer_Velocity()
        {
            var newVelocity = playerRigidBody.velocity;

            newVelocity.x = MovementVector.x * playerCurrentSpeed;

            playerRigidBody.velocity = newVelocity;
        }

        #endregion

        #region TopDownMovement

        protected virtual void TopDown_Position()
        {
            // up down left right
            Vector3 newOffset;

            if ( IsLocalTransform )
                newOffset = transform.right * MovementVector.x + transform.up * MovementVector.y;
            else
                newOffset = Vector3.right * MovementVector.x + Vector3.up * MovementVector.y;

            newOffset *= playerCurrentSpeed * CurrentUsingDeltaTime();
            transform.position += newOffset;
        }

        protected virtual void TopDown_Force()
        {
            Vector2 newForce;

            if ( isLocalTransform )
                newForce = transform.right * MovementVector.x + transform.up * MovementVector.y;
            else
                newForce = Vector2.right * MovementVector.x + Vector2.up * MovementVector.y;

            newForce *= playerCurrentSpeed;

            playerRigidBody.AddForce( newForce, ForceMode2D.Impulse );
        }

        protected virtual void TopDown_Velocity()
        {
            Vector2 newVelocity;

            newVelocity = MovementVector * playerCurrentSpeed;

            playerRigidBody.velocity = newVelocity;
        }

        #endregion

        #region Jump

        //Jump Variables
        //For more better jump
        protected bool canJump = true;
        [Header( "Jump Settings" )]
        [SerializeField] protected bool advancedSettings;

        [DisableWhen( "advancedSettings" )]
        [SerializeField] protected float jumpDelta = .05f;

        protected float lastJumpTime;
        protected float currentJumpDelta;

        protected virtual void JumpMovements()
        {
            //Jump method
            if ( CharacterGroundCheck != null && playerRigidBody != null )
            {
                if ( canJump )
                {
                    if ( Input.GetKey( KeyCode.Space ) && characterGroundCheck.IsOnGround )
                    {
                        lastJumpTime = Time.time;
                        canJump = false;

                        //Invokes the event when the player is jumping
                        PlayerStartJump?.Invoke();

                        if ( isLocalTransform )
                            playerRigidBody.velocity += ( Vector2 )transform.up * playerCurrentJumpStrength;
                        else
                            playerRigidBody.velocity += Vector2.up * playerCurrentJumpStrength;
                    }
                }
                else
                {
                    currentJumpDelta = Time.time - lastJumpTime;

                    if ( currentJumpDelta > jumpDelta )
                        canJump = true;
                }
            }
        }

        #endregion

        #region ScriptMethods(private)

        /// <summary> This method will check all the delta times and returns the matched delta Time </summary>
        float CurrentUsingDeltaTime()
        {
            switch ( moveDeltaTimeOptions )
            {
                case MovementDeltaTimeOptions.DeltaTime: return Time.deltaTime;
                case MovementDeltaTimeOptions.UnscaledDeltaTime: return Time.unscaledDeltaTime;
                case MovementDeltaTimeOptions.FixedDeltaTime: return Time.fixedDeltaTime;
                case MovementDeltaTimeOptions.FixedUnscaledDeltaTime: return Time.fixedUnscaledDeltaTime;

                default:
                    throw ExceptionUtils.Error.NotAccessible;
            }
        }

        #endregion

        #endregion
    }

}
