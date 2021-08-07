using UnityEngine;
using UnityEngine.SceneManagement;
using SM = UnityEngine.SceneManagement.SceneManager;

namespace CXUtils.Common
{
    ///<summary> A helper for scene managing </summary>
    public class SceneUtils
    {
        #region Vars

        /// <summary>
        ///     Current Active Scene index
        /// </summary>
        public static int CurrentSceneIndex => SM.GetActiveScene().buildIndex;

        /// <summary>
        ///     Next Scene index
        /// </summary>
        public static int NextSceneIndex => CurrentSceneIndex + 1;

        /// <summary>
        ///     Previous Scene index
        /// </summary>
        public static int PrevSceneIndex => CurrentSceneIndex - 1;

        /// <summary>
        ///     Last Scene index
        /// </summary>
        public static int LastSceneIndex => SM.sceneCount - 1;

        #endregion

        #region SceneCheck

        /// <summary>
        ///     Returns if the scene exists, checking using <paramref name="sceneIndex" />
        /// </summary>
        public static bool SceneExists( int sceneIndex ) => sceneIndex >= 0 && sceneIndex < SM.sceneCount;

        /// <summary>
        ///     Returns if the scene exists, checking using <paramref name="sceneName" />
        /// </summary>
        public static bool SceneExists( string sceneName ) => SM.GetSceneByName( sceneName ).IsValid();

        #endregion

        #region LoadSceneMethods

        #region Non-async

        /// <inheritdoc cref="TryLoadNextScene(LoadSceneMode)" />
        public static bool TryLoadNextScene() => TryLoadNextScene( LoadSceneMode.Single );

        /// <summary>
        ///     Tries to load the next scene.
        ///     <para>Return if the next scene is valid</para>
        /// </summary>
        public static bool TryLoadNextScene( LoadSceneMode loadSceneMode )
        {
            if ( !SceneExists( NextSceneIndex ) ) return false;

            SM.LoadScene( NextSceneIndex, loadSceneMode );
            return true;
        }

        #endregion

        #region Async

        /// <inheritdoc cref="TryLoadNextSceneAsync(UnityEngine.SceneManagement.LoadSceneMode,out UnityEngine.AsyncOperation)" />
        public static bool TryLoadNextSceneAsync( out AsyncOperation asyncOperation ) => TryLoadNextSceneAsync( LoadSceneMode.Single, out asyncOperation );

        /// <summary> Loads the next scene asynchronously </summary>
        public static bool TryLoadNextSceneAsync( LoadSceneMode loadSceneMode, out AsyncOperation asyncOperation )
        {
            if ( !SceneExists( NextSceneIndex ) )
            {
                asyncOperation = null;
                return false;
            }

            asyncOperation = SM.LoadSceneAsync( NextSceneIndex, loadSceneMode );
            return true;
        }

        #endregion

        /// <summary>
        ///     Reloads the current scene
        /// </summary>
        public static void ReloadActiveScene( LoadSceneMode loadSceneMode = LoadSceneMode.Single ) => SM.LoadScene( SM.GetActiveScene().buildIndex, loadSceneMode );

        public static AsyncOperation ReloadActiveSceneAsync( LoadSceneMode loadSceneMode = LoadSceneMode.Single ) => SM.LoadSceneAsync( SM.GetActiveScene().buildIndex, loadSceneMode );

        #endregion

        #region GettingScene

        /// <summary>
        ///     Tries to get the next scene
        /// </summary>
        public static bool TryGetNextScene( out Scene scene ) => TryGetSceneAt( NextSceneIndex, out scene );

        /// <summary>
        ///     Tries to get the previous scene
        /// </summary>
        public static bool TryGetPreviousScene( out Scene scene ) => TryGetSceneAt( PrevSceneIndex, out scene );

        /// <summary>
        ///     Tries to get the given index's scene
        /// </summary>
        public static bool TryGetSceneAt( int index, out Scene scene )
        {
            if ( !SceneExists( index ) )
            {
                scene = default;
                return false;
            }
            
            scene = SM.GetSceneAt( index );
            return true;
        }

        #endregion
    }
}
