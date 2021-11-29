using UnityEngine;

namespace CXUtils.Components
{
    [AddComponentMenu( "CXUtils/Player/2D/CharacterGroundCheck2D" )]
    public class CharacterGroundCheck2D : MonoBehaviour
    {
        #region Vars and fields

        /// <summary> The Colliding mode </summary>
        public enum CollideMode { Collision, Trigger }

        /// <summary> Options for collision updates </summary>
        public enum CollisionUpdateOptions { Update, FixedUpdate, LateUpdate }

        //private

        [SerializeField] Collider2D _groundCheckCollision;

        [SerializeField] string[] _tags;

        [SerializeField] bool _usingTags;
        [SerializeField] bool _isOnGround;

        [SerializeField] CollisionUpdateOptions _collideUpdateOption;

        //public

        public Collider2D GroundCheckCollision => _groundCheckCollision;

        public CollisionUpdateOptions CollideUpdateOption { get => _collideUpdateOption; set => _collideUpdateOption = value; }

        public string[] Tags { get => _tags; set => _tags = value; }

        public bool IsOnGround { get => _isOnGround; private set => _isOnGround = value; }
        public bool UsingTags  { get => _usingTags;  set => _usingTags = value; }

        #endregion

        #region MainThread

        void Update()
        {
            if ( _collideUpdateOption == CollisionUpdateOptions.Update )
                CollisionCheck();
        }

        void FixedUpdate()
        {
            if ( _collideUpdateOption == CollisionUpdateOptions.FixedUpdate )
                CollisionCheck();
        }

        void LateUpdate()
        {
            if ( _collideUpdateOption == CollisionUpdateOptions.LateUpdate )
                CollisionCheck();
        }

        #endregion

        #region Main Methods

        void CollisionCheck()
        {
            IsOnGround = CheckGroundCollision();
        }

        bool CheckGroundCollision()
        {
            var contactPoints = new ContactPoint2D[20];

            int length = _groundCheckCollision.GetContacts( contactPoints );

            if ( length == 0 || _tags.Length == 0 || !_usingTags ) return false;

            for ( int index = 0; index < length; index++ )
                foreach ( string i in _tags )
                    if ( i != null )
                    {
                        if ( contactPoints[index].collider.CompareTag( i ) )
                            return true;
                    }
                    else
                    {
                        return false;
                    }

            return false;
        }

        #endregion
    }
}
