using System;
using UnityEditor;
using UnityEngine;

namespace CXUtils.Unity
{
    //NOT FINISHED!
    //[CustomPropertyDrawer( typeof( SceneBundle ) )]
    public class SceneBundleDrawer : PropertyDrawer
    {
        const string REFERENCES_PROPERTY_IDENTIFIER = "_references";

        float _extraPropertyHeight;

        bool _foldout;

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            _extraPropertyHeight = 0f;
            var referencesProperty = property.FindPropertyRelative( REFERENCES_PROPERTY_IDENTIFIER );

            int referenceArraySize = referencesProperty.arraySize;
            
            int resultArraySize = EditorGUI.IntField( position, referenceArraySize );

            resultArraySize = Math.Max( resultArraySize, 0 );
            
            _extraPropertyHeight += EditorGUIUtility.singleLineHeight;
            var intFieldRect = new Rect( position.x, position.y + EditorGUIUtility.singleLineHeight, position.width / 2f, position.height );
            _foldout = EditorGUI.Foldout( intFieldRect, _foldout, label );

            if ( resultArraySize != referenceArraySize )
            {
                int resultDifference = resultArraySize - referenceArraySize;
                if ( resultDifference < 0 )
                    for ( int i = 0; i > resultDifference; --i )
                        //NOTE: here we add the resultDifference because result difference is already negative, thus decreasing reference array size
                        referencesProperty.DeleteArrayElementAtIndex( referenceArraySize + resultDifference - 1 );
                else // > 0
                    for ( int i = 0; i < resultDifference; ++i )
                        referencesProperty.DeleteArrayElementAtIndex( referenceArraySize );
                referenceArraySize = resultArraySize;
            }

            if ( _foldout )
            {
                ++EditorGUI.indentLevel;

                if ( referencesProperty.arraySize != 0 )
                {
                    float propertyHeight = EditorGUI.GetPropertyHeight( referencesProperty, true );

                    _extraPropertyHeight += propertyHeight * referenceArraySize;

                    for ( int i = 0; i < referenceArraySize; ++i )
                    {
                        var newPosition = new Rect( 0f, position.y + propertyHeight * i + EditorGUIUtility.singleLineHeight * 2f, position.width, propertyHeight );
                        EditorGUI.PropertyField( newPosition, referencesProperty.GetArrayElementAtIndex( i ) );
                    }
                }

                --EditorGUI.indentLevel;
            }

            if ( referenceArraySize != 0 ) return;

            //else
            EditorGUILayout.HelpBox( nameof( SceneBundle ) + " cannot have 0 elements!", MessageType.Error );
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => base.GetPropertyHeight( property, label ) + _extraPropertyHeight;
    }
}
