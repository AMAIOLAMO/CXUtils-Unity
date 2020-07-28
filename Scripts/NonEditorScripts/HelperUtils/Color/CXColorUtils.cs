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
    public static class ColorUtils
    {
        #region Predefine Colors

        /// <summary> Unity doesn't have more colors? Use this! </summary>
        public class PredefColors
        {
            /*
             * Colors all From Wikipedia: Web_Colors => https://en.wikipedia.org/wiki/Web_colors
             */

            //Red Colors
            public static Color mistyRose => new Color(1, .89f, 1);
            public static Color crimson => new Color(.86f, .07f, .23f);

            //Red Blue Colors
            public static Color purple => new Color(.5f, 0, .5f);
            public static Color violet => new Color(.93f, .5f, .93f);
            public static Color darkViolet => new Color(.49f, 0, 1);
            public static Color darkOrchid => new Color(.52f, .19f, .8f);

            //Blue Colors
            public static Color midnightBlue => new Color(.09f, .09f, .43f);
            public static Color teal => new Color(0, .5f, .5f);
            public static Color skyBlue => new Color(.52f, .8f, .92f);
            public static Color deepSkyBlue => new Color(0, .74f, 1);

            //Green Colors
            public static Color lime => new Color(0, 1, 0);

            //Yellow Colors
            public static Color gold => new Color(1f, .84f, 0);
            public static Color greenYellow => new Color(.67f, 1, .18f);

            //White To Black Colors
            public static Color slateGray => new Color(.43f, .5f, .56f);

            //Other Colors
            public static Color copper => new Color(.72f, .45f, .2f);
        }

        #endregion

        #region Script Methods

        #region Manipulate Colors

        ///<summary> Mapping A Color In A Range To Another Range </summary>
        public static Color Map(Color value, Color In_Min, Color In_Max,
            Color Out_Min, Color Out_Max) =>
            new Color
            {
                r = MathUtils.Map(value.r, In_Min.r, In_Max.r, Out_Min.r, Out_Max.r),
                g = MathUtils.Map(value.g, In_Min.g, In_Max.g, Out_Min.g, Out_Max.g),
                b = MathUtils.Map(value.b, In_Min.b, In_Max.b, Out_Min.b, Out_Max.b),
                a = MathUtils.Map(value.a, In_Min.a, In_Max.a, Out_Min.a, Out_Max.a)
            };

        ///<summary> Blends two color's together with a float (Clamps the blend variable to 0 ~ 1)
        ///<para>It's simply another way of saying Color.lerp()</para></summary>
        public static Color BlendColors(Color color1, Color color2, float blend) => Color.Lerp(color1, color2, blend); //learned this from shader coding, "Blend SrcAlpha OneMinusSrcAlpha" :D

        #endregion

        #region GrayScale

        ///<summary> Get's the gray scale of the color </summary>
        public static float GetGrayScale(this Color color, LumaConvertOptions lumaConvertOptions = LumaConvertOptions.Weighted)
        {
            switch (lumaConvertOptions)
            {
                case LumaConvertOptions.Weighted:
                    return GetGrayScale_Weighted(color);

                case LumaConvertOptions.Luminosity_STD:
                    return GetGrayScale_Luma(color);

                //Luminosity_Performance
                default:
                    return GetGrayScale_Luma2(color);
            }
        }

        ///<summary> Get's the gray scale of the color by getting the average of the color </summary>
        public static float GetGrayScale_Weighted(this Color color) => (color.r + color.g + color.b) / 3f;

        ///<summary> Get's the gray scale of the color using the Luminosity method </summary>
        public static float GetGrayScale_Luma(this Color color) => .2126f * color.r + .7152f * color.g + .0722f * color.b;

        ///<summary> Same as the first luma method, but more quicker </summary>
        public static float GetGrayScale_Luma2(this Color color) => (color.r + color.r + color.b + color.g + color.g + color.g) / 6f;

        ///<summary> Get's the color of the GrayScale value with the given GrayScale value </summary>
        public static Color GetGrayScaleColorByGrayScale(this float grayScale, float alpha = 1) => new Color(grayScale, grayScale, grayScale, alpha);

        #endregion

        #region Convert

        /// <summary> Converts all 255 based int Colors into unity Colors </summary>
        public static Color Convert255ToColor(int r, int g, int b, int a) => new Color(r / 255f, g / 255f, b / 255f, a / 255f);

        #endregion

        #endregion
    }
}