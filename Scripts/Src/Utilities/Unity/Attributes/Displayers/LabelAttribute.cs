namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// Overrides the original label into a new label
    /// </summary>
    public class LabelAttribute : MultiPropertyAttribute
    {
        public LabelAttribute( string label ) => _label = label;

        private readonly string _label;
#if UNITY_EDITOR
        public override GUIContent BuildLabel( GUIContent label )
        {
            label.text = _label;
            return label;
        }
#endif
    }
}
