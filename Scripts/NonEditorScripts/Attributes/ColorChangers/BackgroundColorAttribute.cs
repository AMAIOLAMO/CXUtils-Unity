namespace UnityEngine.CXExtensions
{
    /// <summary>
    ///     Changes the background color of the GUI
    /// </summary>
    public class BackgroundColorAttribute : ColorAttribute
    {
        public BackgroundColorAttribute( string hexColor, bool onlyThisField = false ) : base( hexColor, onlyThisField ) { }

#if UNITY_EDITOR
        public override Color GetOriginColor()
        {
            return GUI.backgroundColor;
        }

        public override void SetColor( in Color color )
        {
            GUI.backgroundColor = color;
        }
#endif
    }
}
