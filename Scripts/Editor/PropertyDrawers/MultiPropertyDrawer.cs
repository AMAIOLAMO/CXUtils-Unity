using UnityEngine;
using System.Linq;
using UnityEngine.CXExtensions;

//we don't worry to add UnityEditor block here because this is inside Editor file which unity will automatically delete on build

namespace UnityEditor.CXExtentions
{
    [CustomPropertyDrawer( typeof( MultiPropertyAttribute ), true )]
    public class MultiPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            var currAttr = attribute as MultiPropertyAttribute;

            if ( currAttr.attrList == null )
            {
                currAttr.attrList = fieldInfo.GetCustomAttributes( typeof( MultiPropertyAttribute ), false ).
                    OrderBy( attr => ( attr as PropertyAttribute ).order ).ToList();
            }

            //for all attributes that's on this field, we try to get all attributes
            for ( int i = 0; i < currAttr.attrList.Count(); i++ )
            {
                //if the current attr is not multi prop attribute then don't care
                if ( !( currAttr.attrList[i] is MultiPropertyAttribute multiPropAttr ) )
                    continue;

                position = multiPropAttr.BuildPosition( position );
                property = multiPropAttr.BuildProperty( property );
                label = multiPropAttr.BuildLabel( label );

                multiPropAttr.OnGUI( position, property, label, fieldInfo );
            }
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            var currAttr = attribute as MultiPropertyAttribute;

            if ( currAttr.attrList == null )
            {
                currAttr.attrList = fieldInfo.GetCustomAttributes( typeof( MultiPropertyAttribute ), false ).
                    OrderBy( attr => ( attr as PropertyAttribute ).order ).ToList();
            }

            float resultHeight = base.GetPropertyHeight( property, label );

            //for all attributes that's on this field, we try to get all attributes
            for ( int i = 0; i < currAttr.attrList.Count(); i++ )
            {
                //if the current attr is not multi prop attribute then don't care
                if ( !( currAttr.attrList[i] is MultiPropertyAttribute multiPropAttr ) )
                    continue;

                var tempHeight = multiPropAttr.GetPropertyHeight( property, label );

                if ( !tempHeight.HasValue )
                    continue;

                resultHeight = tempHeight.Value;
                break; //we break here because we got an attribute that set's a height value, will ignore others for now
            }

            return resultHeight;
        }
    }
}
