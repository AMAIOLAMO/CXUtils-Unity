namespace UnityEngine.CXExtensions
{
    /// <summary>
    /// Overrides the original label into a new label
    /// </summary>
    public class LabelAttribute : MultiPropertyAttribute
    {
        public LabelAttribute(string label) => this.label = label;

        public string label;

        public override GUIContent GetLabel(GUIContent label)
        {
            label.text = this.label;
            return label;
        }
    }
}
