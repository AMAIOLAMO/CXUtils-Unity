using UnityEngine;
using UnityEngine.SceneManagement;

namespace CXUtils.Manager
{
    ///<summary> Cx's Scene manager </summary>
    public class GameSceneManager : MonoBehaviour
    {

        public int NextSceneIndex { get => SceneManager.GetActiveScene().buildIndex + 1; }

        #region SceneCheck
        /// <summary> Returns if the scene exists </summary>
        public bool SceneExists(int sceneIndex) =>
            sceneIndex < SceneManager.sceneCount;
        #endregion

        #region LoadSceneMethods
        ///<summary> Load The Next Scene and Return if the next scene is valid </summary>
        public bool LoadNextScene(LoadSceneMode loadSceneMode)
        {
            if (!SceneExists(NextSceneIndex))
                return false;

            SceneManager.LoadScene(NextSceneIndex, loadSceneMode);
            return true;
        }

        ///<summary> Load The Next Scene and Return if the next scene is valid if default then use single </summary>
        public bool LoadNextScene() =>
            LoadNextScene(LoadSceneMode.Single);

        /// <summary> Loads the next scene asyncly </summary>
        public bool LoadNextSceneAsync(LoadSceneMode loadSceneMode, out AsyncOperation asyncOperation)
        {
            if (!SceneExists(NextSceneIndex))
            {
                asyncOperation = null;
                return false;
            }

            asyncOperation = SceneManager.LoadSceneAsync(NextSceneIndex, loadSceneMode);
            return true;
        }

        /// <summary> Loads the next scene asyncly </summary>
        public bool LoadNextSceneAsync(out AsyncOperation asyncOperation) =>
            LoadNextSceneAsync(LoadSceneMode.Single, out asyncOperation);

        /// <summary> Load's the current scene </summary>
        /// <param name="loadSceneMode">The loading scene mode</param>
        public void ReloadCurrentScene(LoadSceneMode loadSceneMode = default) =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, loadSceneMode);
        #endregion

    }
}
