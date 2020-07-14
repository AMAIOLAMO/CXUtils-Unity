using UnityEngine;
using UnityEngine.SceneManagement;

namespace CXUtils.CodeUtils
{
    ///<summary> A helper for scene managing </summary>
    public class SceneUtils : CXBaseUtils
    {
        #region Vars

        public static int NextSceneIndex => SceneManager.GetActiveScene().buildIndex + 1;

        public static int PreviousSceneIndex => SceneManager.GetActiveScene().buildIndex - 1;

        public static int LastSceneIndex => SceneManager.sceneCount - 1;

        #endregion

        #region SceneCheck

        /// <summary> Returns if the scene exists with the given scene index </summary>
        public static bool SceneExists(int sceneIndex) =>
            sceneIndex >= 0 && sceneIndex < SceneManager.sceneCount;

        /// <summary> Returns if the scene exists with the given name </summary>
        public static bool SceneExists(string sceneName) =>
            SceneManager.GetSceneByName(sceneName).IsValid();

        #endregion

        #region LoadSceneMethods
        
        #region Non-async
        
        ///<summary> Load The Next Scene and Return if the next scene is valid if default then use single </summary>
        public static bool LoadNextScene() =>
            TryLoadNextScene(LoadSceneMode.Single);

        ///<summary> Load The Next Scene and Return if the next scene is valid </summary>
        public static bool TryLoadNextScene(LoadSceneMode loadSceneMode)
        {
            if (!SceneExists(NextSceneIndex))
                return false;

            SceneManager.LoadScene(NextSceneIndex, loadSceneMode);
            return true;
        }
        
        #endregion

        #region Async
        
        /// <summary> Loads the next scene asyncronously </summary>
        public static bool LoadNextSceneAsync(out AsyncOperation asyncOperation) =>
            LoadNextSceneAsync(LoadSceneMode.Single, out asyncOperation);

        /// <summary> Loads the next scene asyncronously </summary>
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

        #region GettingScene

        /// <summary> Tries to get the next scene </summary>
        public static bool TryGetNextScene(out Scene scene) =>
            TryGetSceneAt(NextSceneIndex, out scene);

        /// <summary> Tries to get the previous scene </summary>
        public static bool TryGetPreviousScene(out Scene scene) =>
            TryGetSceneAt(PreviousSceneIndex, out scene);

        /// <summary> Tries to get the given index's scene </summary>
        public static bool TryGetSceneAt(int index, out Scene scene)
        {
            if(!SceneExists(index))
            {
                scene = default;
                return false;
            }

            scene = SceneManager.GetSceneAt(index);
            return true;
        }

        #endregion
    }
}
