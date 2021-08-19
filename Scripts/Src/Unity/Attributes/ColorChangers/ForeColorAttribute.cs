namespace UnityEngine.CXExtensions
{
    /// <summary>
    ///     Changes the content color of the GUI
    /// </summary>
    public class ForeColorAttribute : ColorAttribute
    {
        public ForeColorAttribute( string hexColor, bool onlyThisField = false ) : base( hexColor, onlyThisField ) { }

#if UNITY_EDITOR
        public override Color GetOriginColor()
        {
            return GUI.contentColor;
        }

        public override void SetColor( in Color color )
        {
            GUI.contentColor = color;
        }
#endif
    }
}
