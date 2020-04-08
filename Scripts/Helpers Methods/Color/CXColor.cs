using UnityEngine;
using CXUtils.CXMath;

namespace CXUtils.CXColor{
    ///<summary>
    ///Cx's Color Class
    ///</summary>
    public class CXColor{
        ///<summary>
        ///Mapping A Color In A Range To Another Range
        ///</summary>
        public static Color Map(Color startColor, Color RangeColor_Min, Color RangeColor_Max, Color MapToColor_Min, Color MapToColor_Max){
            Color newColor = new Color();
            newColor.r = CXMath.Mathf.Map(startColor.r, RangeColor_Min.r, RangeColor_Max.r, MapToColor_Min.r, MapToColor_Max.r);
            newColor.g = CXMath.Mathf.Map(startColor.g, RangeColor_Min.g, RangeColor_Max.g, MapToColor_Min.g, MapToColor_Max.g);
            newColor.b = CXMath.Mathf.Map(startColor.b, RangeColor_Min.b, RangeColor_Max.b, MapToColor_Min.b, MapToColor_Max.b);
            newColor.a = CXMath.Mathf.Map(startColor.a, RangeColor_Min.a, RangeColor_Max.a, MapToColor_Min.a, MapToColor_Max.a);
            return newColor;
        }
        
    }
}