using UnityEngine;

namespace CXUtils.Common
{
    public static class GameObjectUtils
    {
        /// <summary>
        ///     Toggle the <paramref name="gameObject" />'s activeness <br />
        ///     using either <see cref="GameObject.activeSelf" /> or <see cref="GameObject.activeInHierarchy" /> depending on
        /// </summary>
        public static bool ToggleActive( this GameObject gameObject )
        {
            bool resultActive = !gameObject.activeSelf;
            gameObject.SetActive( resultActive );
            return resultActive;
        }
    }
}
