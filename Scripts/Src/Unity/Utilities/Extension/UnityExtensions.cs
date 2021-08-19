using UnityEngine;

namespace CXUtils.Common
{
    public static class GMOBJUtils
    {
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
