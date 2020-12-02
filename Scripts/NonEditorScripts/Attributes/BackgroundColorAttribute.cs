namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// Changes the background color of the GUI
    /// </summary>
    public class BackgroundColorAttribute : ColorAttribute
    {
        public BackgroundColorAttribute(string hexColor, bool onlyThisField = false) : base(hexColor, onlyThisField) { }

        public override Color GetColor() => GUI.backgroundColor;

        public override void SetColor(Color color) => GUI.backgroundColor = color;
    }
}
