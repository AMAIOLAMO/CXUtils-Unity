using UnityEngine;
using UnityEngine.SceneManagement;

namespace CXUtils.CodeUtils
{
    ///<summary> A helper for scene managing </summary>
    public struct SceneUtils
    {
        /// <summary> The next Scene Index </summary>
        public static int NextSceneIndex => SceneManager.GetActiveScene().buildIndex + 1;

        /// <summary> The Previous Scene Index </summary>
        public static int PreviousSceneIndex => SceneManager.GetActiveScene().buildIndex - 1;

        #region SceneCheck
        /// <summary> Returns if the scene exists </summary>
        public static bool SceneExists(int sceneIndex) =>
            sceneIndex > 0 && sceneIndex < SceneManager.sceneCount;
        #endregion

        #region LoadSceneMethods
        #region Non-async
        ///<summary> Load The Next Scene and Return if the next scene is valid if default then use single </summary>
        public static bool LoadNextScene() =>
            LoadNextScene(LoadSceneMode.Single);

        ///<summary> Load The Next Scene and Return if the next scene is valid </summary>
        public static bool LoadNextScene(LoadSceneMode loadSceneMode)
        {
            if (!SceneExists(NextSceneIndex))
                return false;

            SceneManager.LoadScene(NextSceneIndex, loadSceneMode);
            return true;
        }
        #endregion

        #region Async
        /// <summary> Loads the next scene asyncly </summary>
        public static bool LoadNextSceneAsync(out AsyncOperation asyncOperation) =>
            LoadNextSceneAsync(LoadSceneMode.Single, out asyncOperation);

        /// <summary> Loads the next scene asyncly </summary>
        public static bool LoadNextSceneAsync(LoadSceneMode loadSceneMode, out AsyncOperation asyncOperation)
        {
            if (!SceneExists(NextSceneIndex))
            {
                asyncOperation = null;
                return false;
            }

            asyncOperation = SceneManager.LoadSceneAsync(NextSceneIndex, loadSceneMode);
            return true;
        }
        #endregion

        /// <summary> Load's the current scene </summary> 
        public static void ReloadActiveScene(LoadSceneMode loadSceneMode = default) =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, loadSceneMode);
        #endregion

    }
}
