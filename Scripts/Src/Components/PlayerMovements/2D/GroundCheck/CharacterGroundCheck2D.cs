using UnityEngine;

namespace CXUtils.HelperComponents
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

        [SerializeField] Collider2D groundCheckCollision;

        [SerializeField] string[] tags;

        [SerializeField] bool usingTags;
        [SerializeField] bool isOnGround;

        [SerializeField] CollisionUpdateOptions collideUpdateOption;

        //public

        public Collider2D GroundCheckCollision => groundCheckCollision;

        public CollisionUpdateOptions CollideUpdateOption { get => collideUpdateOption; set => collideUpdateOption = value; }

        public string[] Tags { get => tags; set => tags = value; }

        public bool IsOnGround { get => isOnGround; private set => isOnGround = value; }
        public bool UsingTags { get => usingTags; set => usingTags = value; }

        #endregion

        #region MainThread

        void Update()
        {
            if ( collideUpdateOption == CollisionUpdateOptions.Update )
                CollisionCheck();
        }

        void FixedUpdate()
        {
            if ( collideUpdateOption == CollisionUpdateOptions.FixedUpdate )
                CollisionCheck();
        }

        void LateUpdate()
        {
            if ( collideUpdateOption == CollisionUpdateOptions.LateUpdate )
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

            int length = groundCheckCollision.GetContacts( contactPoints );

            if ( length == 0 || tags.Length == 0 || !usingTags ) return false;

            for ( int index = 0; index < length; index++ )
                foreach ( string i in tags )
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
