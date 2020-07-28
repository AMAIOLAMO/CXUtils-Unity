using UnityEngine;
using UnityEngine.SceneManagement;

using SM = UnityEngine.SceneManagement.SceneManager;

namespace CXUtils.CodeUtils
{
    ///<summary> A helper for scene managing </summary>
    public class SceneUtils
    {
        #region Vars

        public static int NextSceneIndex => SM.GetActiveScene().buildIndex + 1;

        public static int PreviousSceneIndex => SM.GetActiveScene().buildIndex - 1;

        public static int LastSceneIndex => SM.sceneCount - 1;

        #endregion

        #region SceneCheck

        /// <summary>
        /// Returns if the scene exists, checking <paramref name="sceneIndex"/>
        /// </summary>
        public static bool SceneExists(int sceneIndex) => sceneIndex >= 0 && sceneIndex < SM.sceneCount;

        /// <summary>
        /// Returns if the scene exists, checking <paramref name="sceneName"/>
        /// </summary>
        public static bool SceneExists(string sceneName) => SM.GetSceneByName(sceneName).IsValid();

        #endregion

        #region LoadSceneMethods
        
        #region Non-async
        
        /// <inheritdoc cref="TryLoadNextScene(LoadSceneMode)"/>
        public static bool LoadNextScene() => TryLoadNextScene(LoadSceneMode.Single);

        ///<summary> Load The Next Scene and Return if the next scene is valid </summary>
        public static bool TryLoadNextScene(LoadSceneMode loadSceneMode)
        {
            if (!SceneExists(NextSceneIndex)) return false;

            SM.LoadScene(NextSceneIndex, loadSceneMode);
            return true;
        }
        
        #endregion

        #region Async
        
        /// <summary> Loads the next scene asyncronously </summary>
        public static bool LoadNextSceneAsync(out AsyncOperation asyncOperation) => LoadNextSceneAsync(LoadSceneMode.Single, out asyncOperation);

        /// <summary> Loads the next scene asyncronously </summary>
        public static bool LoadNextSceneAsync(LoadSceneMode loadSceneMode, out AsyncOperation asyncOperation)
        {
            if (!SceneExists(NextSceneIndex))
            {
                asyncOperation = null;
                return false;
            }

            asyncOperation = SM.LoadSceneAsync(NextSceneIndex, loadSceneMode);
            return true;
        }
        
        #endregion

        /// <summary> Load's the current scene </summary> 
        public static void ReloadActiveScene(LoadSceneMode loadSceneMode = default) => SM.LoadScene(SM.GetActiveScene().buildIndex, loadSceneMode);

        #endregion

        #region GettingScene

        /// <summary> Tries to get the next scene </summary>
        public static bool TryGetNextScene(out Scene scene) => TryGetSceneAt(NextSceneIndex, out scene);

        /// <summary> Tries to get the previous scene </summary>
        public static bool TryGetPreviousScene(out Scene scene) => TryGetSceneAt(PreviousSceneIndex, out scene);

        /// <summary> Tries to get the given index's scene </summary>
        public static bool TryGetSceneAt(int index, out Scene scene)
        {
            if(!SceneExists(index))
            {
                scene = default;
                return false;
            }

            scene = SM.GetSceneAt(index);
            return true;
        }

        #endregion
    }
}
