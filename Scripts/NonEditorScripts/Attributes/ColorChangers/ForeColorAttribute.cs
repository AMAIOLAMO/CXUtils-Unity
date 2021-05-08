namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// Changes the content color of the GUI
    /// </summary>
    public class ForeColorAttribute : ColorAttribute
    {
        public ForeColorAttribute(string hexColor, bool onlyThisField = false) : base(hexColor, onlyThisField) { }

#if UNITY_EDITOR
        public override Color GetColor() => GUI.contentColor;

        public override void SetColor(Color color) => GUI.contentColor = color;
#endif
    }
}
