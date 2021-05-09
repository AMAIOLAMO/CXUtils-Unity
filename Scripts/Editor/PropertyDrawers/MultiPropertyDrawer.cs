using UnityEngine;
using UnityEngine.CXExtensions;

namespace UnityEditor.CXExtentions
{
    [CustomPropertyDrawer(typeof(MultiPropertyAttribute), true)]
    public class MultiPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            object[] attributes = fieldInfo.GetCustomAttributes(false);

            //for all attributes that's on this field, we try to get all attributes
            for ( int i = 0; i < attributes.Length; i++ )
            {
                var currentAttribute = attributes[i] as MultiPropertyAttribute;

                if ( currentAttribute != null )
                {
                    position = currentAttribute.GetPosition(position);
                    property = currentAttribute.GetProperty(property);
                    label = currentAttribute.GetLabel(label);

                    currentAttribute.OnGUI(position, property, label, fieldInfo);
                }
            }
        }
    }
}
