using UnityEngine;
using System.Linq;
using UnityEngine.CXExtensions;

//we don't worry to add UnityEditor block here because this is inside Editor file which unity will automatically delete on build

namespace UnityEditor.CXExtensions
{
    [CustomPropertyDrawer( typeof( MultiPropertyAttribute ), true )]
    public class MultiPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            var attributes = attribute as MultiPropertyAttribute;

            if ( attributes.attrList == null )
            {
                attributes.attrList = fieldInfo.GetCustomAttributes( typeof( MultiPropertyAttribute ), false ).
                    OrderBy( attribute => ( (PropertyAttribute)attribute ).order ).ToList();
            }

            //for all attributes that's on this field, we try to get all attributes
            for ( int i = 0; i < attributes.attrList.Count(); i++ )
            {
                //if the current attr is not multi prop attribute then don't care
                if ( !( attributes.attrList[i] is MultiPropertyAttribute multiPropAttribute ) ) continue;

                position = multiPropAttribute.BuildPosition( position );
                property = multiPropAttribute.BuildProperty( property );
                label = multiPropAttribute.BuildLabel( label );

                multiPropAttribute.OnGUI( position, property, label, fieldInfo );
            }
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            var attributes = attribute as MultiPropertyAttribute;

            if ( attributes.attrList == null )
            {
                attributes.attrList = fieldInfo.GetCustomAttributes( typeof( MultiPropertyAttribute ), false ).
                    OrderBy( attribute => ( (PropertyAttribute)attribute ).order ).ToList();
            }

            float resultHeight = base.GetPropertyHeight( property, label );

            //for all attributes that's on this field, we try to get all attributes
            for ( int i = 0; i < attributes.attrList.Count(); i++ )
            {
                //if the current attr is not multi prop attribute then don't care
                if ( !( attributes.attrList[i] is MultiPropertyAttribute multiPropAttribute ) ) continue;

                float? tempHeight = multiPropAttribute.GetPropertyHeight( property, label );

                if ( !tempHeight.HasValue ) continue;

                resultHeight = tempHeight.Value;
                break; //we break here because we got an attribute that set's a height value, will ignore others for now
            }

            return resultHeight;
        }
    }
}
