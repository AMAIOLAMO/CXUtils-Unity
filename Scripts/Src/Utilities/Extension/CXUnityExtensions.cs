using UnityEngine;

namespace CXUtils.CodeUtils
{
    public static class CXGMOBJUtils
    {
        /// <summary>
        /// Toggle the <paramref name="gameObject"/>'s activeness in the <see cref="GameObject.activeSelf"/>
        /// </summary>
        public static bool ToggleActive( this GameObject gameObject )
        {
            bool resultActive = !gameObject.activeSelf;
            gameObject.SetActive( resultActive );
            return resultActive;
        }

        /// <summary>
        /// Toggle the <paramref name="gameObject"/>'s activeness <br/>
        /// using either <see cref="GameObject.activeSelf"/> or <see cref="GameObject.activeInHierarchy"/> depending on <paramref name="useSelf"/>
        /// </summary>
        public static bool ToggleActive( this GameObject gameObject, bool useSelf = true )
        {
            bool resultActive = !( useSelf ? gameObject.activeSelf : gameObject.activeInHierarchy );
            gameObject.SetActive( resultActive );
            return resultActive;
        }
    }
}
