using UnityEngine;
using UnityEngine.UI;

namespace CXUtils.Experimental.UI.Texts
{
    /// <summary> Helps manage the UI text </summary>
    public class UITextHelper : MonoBehaviour
    {
        #region Vars and Fields
        public static UITextHelper Instance { get; private set; }
        [SerializeField] public static Transform emptyTextTrans;
        public Text emptyText;
        public RectTransform defaultRectTrans;
        #endregion

        #region MainRuntimeInitializeMethod
        [RuntimeInitializeOnLoadMethod]
        private void Awake()
        {
            (emptyText, defaultRectTrans) =
            (emptyTextTrans.GetComponent<Text>(), emptyTextTrans.GetComponent<RectTransform>());
            Instance = this;
        }
        #endregion

        #region SpawnText
        /// <summary> Instantiates a text on the world </summary>
        public Text SpawnTextOnCanvas(Canvas canvas)
        {
            if (canvas == null)
                return SpawnTextOnCanvas(emptyTextTrans.GetComponent<Text>(), defaultRectTrans,
                FindObjectOfType<Canvas>().GetComponent<Canvas>());

            return SpawnTextOnCanvas(emptyTextTrans.GetComponent<Text>(), defaultRectTrans, canvas);
        }

        /// <summary> Instantiates a text on the world </summary>
        public Text SpawnTextOnCanvas(RectTransform rectTrans, Canvas canvas) =>
            SpawnTextOnCanvas(emptyTextTrans.GetComponent<Text>(), rectTrans, canvas);

        /// <summary> Instantiates a text on the world </summary>
        public Text SpawnTextOnCanvas(Text textPrefab, RectTransform rectTrans, Canvas canvas) =>
            Instantiate(textPrefab, rectTrans.position, rectTrans.rotation, canvas.transform) as Text;
        #endregion
    }
}
