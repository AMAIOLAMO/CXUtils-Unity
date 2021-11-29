using System;
using CXUtils.Common;
using UnityEngine;
using UnityEngine.CXExtensions;
using UnityEngine.Serialization;

namespace CXUtils.Components
{
    /// <summary>
    ///     A Character controller for 2 Dimension games
    /// </summary>
    [AddComponentMenu( "CXUtils/Player/2D/CharacterController2D" )]
    public class CharacterController2D : MonoBehaviour
    {
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
        void CheckAndMove( MovementUpdateOptions moveUpdateOp )
        {
            if ( _moveUpdateOptions != moveUpdateOp ) return;

            GetMovements();

            switch ( _perspective )
            {
                case PerspectiveMode.Platformer:
                    MovePlayerPlatformer();
                    break;

                case PerspectiveMode.TopDown:
                    MovePlayerTopDown();
                    break;

                default: throw ExceptionUtils.NotAccessible;
            }
        }

        #region MovementInputs

        protected virtual void GetMovements()
        {
            MovementVectorRaw = new Vector2( Input.GetAxisRaw( "Horizontal" ), Input.GetAxisRaw( "Vertical" ) );
        }

        #endregion

        #region ScriptMethods(private)

        /// <summary> This method will check all the delta times and returns the matched delta Time </summary>
        float CurrentUsingDeltaTime()
        {
            switch ( _moveDeltaTimeOptions )
            {
                case MovementDeltaTimeOptions.DeltaTime:              return Time.deltaTime;
                case MovementDeltaTimeOptions.UnscaledDeltaTime:      return Time.unscaledDeltaTime;
                case MovementDeltaTimeOptions.FixedDeltaTime:         return Time.fixedDeltaTime;
                case MovementDeltaTimeOptions.FixedUnscaledDeltaTime: return Time.fixedUnscaledDeltaTime;

                default: throw ExceptionUtils.NotAccessible;
            }
        }

        #endregion

        #region Enums

        /// <summary>
        ///     How the Character controller works
        /// </summary>
        public enum PerspectiveMode
        {
            Platformer, TopDown
        }

        /// <summary>
        ///     The options of how the movement will be updated in this character controller
        /// </summary>
        public enum MovementUpdateOptions
        {
            Update, FixedUpdate, LateUpdate
        }

        /// <summary>
        ///     The options of how the movement will be updated using the Time frame delta
        /// </summary>
        public enum MovementDeltaTimeOptions
        {
            DeltaTime, UnscaledDeltaTime, FixedDeltaTime, FixedUnscaledDeltaTime
        }

        /// <summary>
        ///     The modes of how the movement will be calculated
        /// </summary>
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
        Rigidbody2D _playerRigidBody;

        [HideInInspector]
        [SerializeField]
        CharacterGroundCheck2D _characterGroundCheck;

        [Header( "Options" )]
        [SerializeField] PerspectiveMode _perspective = PerspectiveMode.Platformer;
        [SerializeField] MovementUpdateOptions    _moveUpdateOptions    = MovementUpdateOptions.Update;
        [SerializeField] MovementDeltaTimeOptions _moveDeltaTimeOptions = MovementDeltaTimeOptions.DeltaTime;
        [SerializeField] MovementMode             _moveMode             = MovementMode.Position;

        [Header( "Player Settings" )]
        [SerializeField]
        float playerCurrentSpeed = 5f;

        [SerializeField] bool _isLocalTransform;

        [HideInInspector]
        [SerializeField] float _jumpStrength = 5f;

        [SerializeField] bool _isMovementNormalized;

        #endregion

        protected Vector2 MovementVectorRaw { get; private set; }

        public Rigidbody2D PlayerRigidBody { get => _playerRigidBody; set => _playerRigidBody = value; }

        public CharacterGroundCheck2D CharacterGroundCheck { get => _characterGroundCheck; set => _characterGroundCheck = value; }

        public event Action PlayerStartJump;

        #region Option Properties

        public PerspectiveMode          Perspec              { get => _perspective;          set => _perspective = value; }
        public MovementUpdateOptions    MoveUpdateOptions    { get => _moveUpdateOptions;    set => _moveUpdateOptions = value; }
        public MovementDeltaTimeOptions MoveDeltaTimeOptions { get => _moveDeltaTimeOptions; set => _moveDeltaTimeOptions = value; }
        public MovementMode             MoveMode             { get => _moveMode;             set => _moveMode = value; }

        #endregion

        public Vector2 MovementVector =>
            _isMovementNormalized ? MovementVectorRaw.normalized : MovementVectorRaw;

        public float PlayerCurrentSpeed        { get => playerCurrentSpeed;        set => playerCurrentSpeed = value; }
        public float PlayerCurrentJumpStrength { get => _jumpStrength; set => _jumpStrength = value; }

        public bool IsLocalTransform     { get => _isLocalTransform;     set => _isLocalTransform = value; }
        public bool IsMovementNormalized { get => _isMovementNormalized; set => _isMovementNormalized = value; }

        #endregion

        #region Movements

        protected virtual void MovePlayerPlatformer()
        {
            switch ( _moveMode )
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

                default: throw ExceptionUtils.NotAccessible;
            }

            JumpMovements();
        }

        protected virtual void MovePlayerTopDown()
        {
            switch ( _moveMode )
            {
                case MovementMode.Position:
                    TopDown_Position();
                    break;
                case MovementMode.Force:
                    TopDownForce();
                    break;
                case MovementMode.Velocity:
                    TopDownVelocity();
                    break;

                default: throw ExceptionUtils.NotAccessible;
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

            _playerRigidBody.AddForce( newForce, ForceMode2D.Impulse );
        }

        protected virtual void Platformer_Velocity()
        {
            var newVelocity = _playerRigidBody.velocity;

            newVelocity.x = MovementVector.x * playerCurrentSpeed;

            _playerRigidBody.velocity = newVelocity;
        }

        #endregion

        #region TopDownMovement

        protected virtual void TopDown_Position()
        {
            var thisTransform = transform;

            var newOffset = _isLocalTransform ?
                thisTransform.right * MovementVector.x + thisTransform.up * MovementVector.y :
                Vector3.right * MovementVector.x + Vector3.up * MovementVector.y;

            newOffset *= playerCurrentSpeed * CurrentUsingDeltaTime();
            transform.position += newOffset;
        }

        protected virtual void TopDownForce()
        {
            var thisTransform = transform;

            var newForce = _isLocalTransform ?
                (Vector2)( thisTransform.right * MovementVector.x + thisTransform.up * MovementVector.y ) :
                Vector2.right * MovementVector.x + Vector2.up * MovementVector.y;

            newForce *= playerCurrentSpeed;

            _playerRigidBody.AddForce( newForce, ForceMode2D.Impulse );
        }

        protected virtual void TopDownVelocity() =>
            _playerRigidBody.velocity = MovementVector * playerCurrentSpeed;

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
            if ( CharacterGroundCheck == null || _playerRigidBody == null )
                return;

            if ( canJump )
            {
                if ( !Input.GetKey( KeyCode.Space ) || !_characterGroundCheck.IsOnGround )
                    return;

                lastJumpTime = Time.time;
                canJump = false;

                //Invokes the event when the player is jumping
                PlayerStartJump?.Invoke();

                _playerRigidBody.velocity += _isLocalTransform ?
                    (Vector2)transform.up * _jumpStrength :
                    Vector2.up * _jumpStrength;
            }
            else
            {
                currentJumpDelta = Time.time - lastJumpTime;

                if ( currentJumpDelta > jumpDelta )
                    canJump = true;
            }
        }

        #endregion
    }
}
