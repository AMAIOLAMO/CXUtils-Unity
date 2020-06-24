using UnityEngine;

namespace CXUtils.CodeUtils
{
    ///<summary> Options for calculating luminance of the color </summary>
    public enum LumaConvertOptions
    {
        ///<summary> Get's the average of the Colors </summary>
        Weighted,

        ///<summary> Using the standard Luminosity method </summary>
        Luminosity_STD,

        ///<summary> For the Sake of performance!!! (Lose of accuracy) </summary>
        Luminosity_Performance
    }

    ///<summary> Cx's Color Class </summary>
    public struct ColorUtils
    {
        #region Script Methods

        ///<summary> Mapping A Color In A Range To Another Range </summary>
        public static Color Map(Color startColor, Color RangeColor_Min, Color RangeColor_Max,
            Color MapToColor_Min, Color MapToColor_Max) =>
            new Color
            {
                r = MathUtils.Map(startColor.r, RangeColor_Min.r, RangeColor_Max.r, MapToColor_Min.r, MapToColor_Max.r),
                g = MathUtils.Map(startColor.g, RangeColor_Min.g, RangeColor_Max.g, MapToColor_Min.g, MapToColor_Max.g),
                b = MathUtils.Map(startColor.b, RangeColor_Min.b, RangeColor_Max.b, MapToColor_Min.b, MapToColor_Max.b),
                a = MathUtils.Map(startColor.a, RangeColor_Min.a, RangeColor_Max.a, MapToColor_Min.a, MapToColor_Max.a)
            };

        ///<summary> Blends two color's together with a float (Clamps the blend variable to 0 ~ 1)</summary>
        public static Color BlendColors(Color color1, Color color2, float blend)
        {
            blend = Mathf.Clamp01(blend);

            color1 *= 1 - blend;
            color2 *= blend;

            return color1 + color2;

            //learned this from shader coding, "Blend SrcAlpha OneMinusSrcAlpha" :D
        }

        #region GrayScale
        
        ///<summary> Get's the gray scale of the color </summary>
        public static float GetGrayScale(Color color,
         LumaConvertOptions lumaConvertOptions = LumaConvertOptions.Weighted)
        {
            switch(lumaConvertOptions)
            {
                case LumaConvertOptions.Weighted:
                    return GetGrayScale_Weighted(color);

                case LumaConvertOptions.Luminosity_STD:
                    return GetGrayScale_Luma(color);
                default:
                    return  GetGrayScale_Luma2(color);
            }
        }

        ///<summary> Get's the gray scale of the color by getting the average of the color </summary>
        public static float GetGrayScale_Weighted(Color color) =>
            (color.r + color.g + color.b) / 3;

        ///<summary> Get's the gray scale of the color using the Luminosity method </summary>
        public static float GetGrayScale_Luma(Color color) =>
            .2126f * color.r + .7152f * color.g + .0722f * color.b;

        ///<summary> Same as the first luma method, but more quicker </summary>
        public static float GetGrayScale_Luma2(Color color) =>
            (color.r + color.r + color.b + color.g + color.g + color.g) / 6f;

        ///<summary> Get's the color of the GrayScale value with the given GrayScale value </summary>
        public static Color GetGrayScaleColorByGrayScale(float grayScale, float alpha = 1) =>
            new Color(grayScale, grayScale, grayScale, alpha);
            
        #endregion

        #endregion
    }
}