using UnityEngine;

namespace CXUtils.HelperComponents
{
    [AddComponentMenu("CXUtils/Player/2D/CharacterGroundCheck2D")]
    public class CharacterGroundCheck2D : MonoBehaviour
    {
        #region Vars and fields
        /// <summary> The Colliding mode </summary>
        public enum CollideMode { Collision, Trigger }

        /// <summary> Options for collision updates </summary>
        public enum CollisionUpdateOptions { Update, FixedUpdate, LateUpdate }

        //private

        [SerializeField] private Collider2D groundCheckCollision;

        [SerializeField] private string[] tags = default;

        [SerializeField] private bool usingTags = false;
        [SerializeField] private bool isOnGround = false;

        [SerializeField] private CollisionUpdateOptions collideUpdateOption;

        //public

        public Collider2D GroundCheckCollision { get => groundCheckCollision; set => groundCheckCollision = value; }

        public CollisionUpdateOptions CollideUpdateOption { get => collideUpdateOption; set => collideUpdateOption = value; }

        public string[] Tags { get => tags; set => tags = value; }

        public bool IsOnGround { get => isOnGround; set => isOnGround = value; }
        public bool UsingTags { get => usingTags; set => usingTags = value; }
        #endregion

        #region MainThread
        private void Update()
        {
            if (collideUpdateOption == CollisionUpdateOptions.Update)
                CollisionCheck();
        }

        private void FixedUpdate()
        {
            if (collideUpdateOption == CollisionUpdateOptions.FixedUpdate)
                CollisionCheck();
        }

        private void LateUpdate()
        {
            if (collideUpdateOption == CollisionUpdateOptions.LateUpdate)
                CollisionCheck();
        }
        #endregion

        #region Main Methods
        protected void CollisionCheck()
        {
            IsOnGround = CheckGroundCollision();
        }

        protected bool CheckGroundCollision()
        {
            ContactPoint2D[] c = new ContactPoint2D[20];
            int len;
            len = groundCheckCollision.GetContacts(c);
            if (len == 0 || tags.Length == 0)
                return false;

            else
            {
                if (usingTags)
                {
                    for (int index = 0; index < len; index++)
                        foreach (var i in tags)
                        {
                            if (i != null)
                            {
                                if (c[index].collider.CompareTag(i))
                                    return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    return false;
                }
                //else
                return true;
            }
        }
        #endregion
    }
}
