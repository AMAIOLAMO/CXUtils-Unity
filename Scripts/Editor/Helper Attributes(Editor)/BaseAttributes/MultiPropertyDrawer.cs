using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
namespace CXUtils.HelperAttributes
{
    /// <summary> The property drawer for the multi property attribute </summary>
    [CustomPropertyDrawer(typeof(MultiPropertyAttribute), true)]
    public class MultiPropertyDrawer : PropertyDrawer
    {
        #region drawer code
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MultiPropertyAttribute _Attribute = attribute as MultiPropertyAttribute;

            //get stored attributes
            //if(_Attribute.storedAttributes == null)  ---> there is no chance that this will become null... or do it?

            if (_Attribute.storedAttributes.Count == 0)
                _Attribute.storedAttributes = GetAttributes();

            //Drawing code
            DrawMultiOnGUI(_Attribute.storedAttributes, position, property, label);

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            MultiPropertyAttribute _Attribute = attribute as MultiPropertyAttribute;

            if (LoopAndGetHeight(_Attribute.storedAttributes, property, label, out float returnedHeight))
                return returnedHeight;

            return base.GetPropertyHeight(property, label);
        }
        #endregion

        #region Script Methods
        //get's the attributes of this field info
        private List<object> GetAttributes()
        {
            object[] os = fieldInfo.GetCustomAttributes(typeof(MultiPropertyAttribute), false);
            return os.OrderBy((o) => ((PropertyAttribute)o).order).ToList();
        }

        //loops and get the height
        private bool LoopAndGetHeight(List<object> storedAttributes, SerializedProperty property,
        GUIContent label, out float result)
        {
            result = default;
            foreach (object atr in storedAttributes)
            {
                if ((atr as MultiPropertyAttribute) != null)
                {
                    var currentHeight = ((MultiPropertyAttribute)atr).GetPropertyHeight(property, label);

                    if (currentHeight.HasValue)
                    {
                        result = currentHeight.Value;
                        return true;
                    }
                }
            }
            return false;
        }

        //drawing the multi property (Important)
        private void DrawMultiOnGUI(List<object> storedAttributes, Rect position,
        SerializedProperty property, GUIContent label)
        {
            var OriginGUIColor = GUI.color;
            var CurrentLabel = label;
            var CurrentPosition = position;

            //the magic
            for(int i = 0; i < storedAttributes.Count; i ++)
            {
                if(storedAttributes[i] as MultiPropertyAttribute != null)
                {
                    MultiPropertyAttribute Matr = (MultiPropertyAttribute)storedAttributes[i];

                    //first construct label
                    CurrentLabel = Matr.ConstructLabel(CurrentLabel);
                    //second construct position
                    CurrentPosition = Matr.ConstructPosition(CurrentPosition);
                    //draw current GUI
                    Matr.Multi_OnGUI(CurrentPosition, property, CurrentLabel, (i == storedAttributes.Count - 1));

                    //check if you could let the next one draw
                    if (!Matr.CanLetNextDraw)
                        break;
                }
            }

            #region old Code
            //foreach (object atr in storedAttributes)
            //{
            //    if (atr as MultiPropertyAttribute != null)
            //    {
            //        MultiPropertyAttribute Matr = (MultiPropertyAttribute)atr;
            //        //first construct label
            //        CurrentLabel = Matr.ConstructLabel(CurrentLabel);
            //        //second construct position
            //        CurrentPosition = Matr.ConstructPosition(CurrentPosition);

            //        Matr.Multi_OnGUI(CurrentPosition, property, CurrentLabel);

            //        //if this multi property didn't draw (then use default)
            //        if (Matr.IsOnGUIBaseCalled)
            //            guiBaseCalled = true;
            //    }
            //}

            #endregion


            //The magic Ends~~~~~

            //preventing color leak(because other color might just color the whole thing)
            GUI.color = OriginGUIColor;
        }
        #endregion
    }
}
#endif