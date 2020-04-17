using UnityEngine;
using CXUtils.CXMath;
/*
 * Made by CXRedix
 * Free tool for unity.
 */
namespace CXUtils.HelperAttributes
{
    /// <summary> Pre defined colors </summary>
    public enum EnumAttributeColor
    {
        white, black, gray,
        blue, red, green,
        cyan, yellow, purple,
        orange, aqua, silver,
        gold, magenta, brown,
        fuchsia, lime, clear
    }

    /// <summary> Recieves the color that the EnumAttributeColor will get </summary>
    public static class CXEColorReciever
    {
        private static Color silver =
            new Color(MathFunc.Map(192, 0, 255, 0, 1),
                    MathFunc.Map(192, 0, 255, 0, 1),
                    MathFunc.Map(192, 0, 255, 0, 1));

        private static Color purple =
            new Color(MathFunc.Map(128, 0, 255, 0, 1), 0,
                    MathFunc.Map(128, 0, 255, 0, 1));

        private static Color orange =
            new Color(1,
                    MathFunc.Map(165, 0, 255, 0, 1), 0);

        private static Color gold =
            new Color(1,
                    MathFunc.Map(215, 0, 255, 0, 1), 0);

        private static Color aqua =
            new Color(0, 1, 1);

        private static Color brown =
            new Color(MathFunc.Map(150, 0, 255, 0, 1),
                    MathFunc.Map(75, 0, 255, 0, 1), 0);

        private static Color fuchsia =
            new Color(1, 0, 1);

        private static Color lime =
            new Color(0, 1, 0);


        public static Color GetColor(this EnumAttributeColor enumColor)
        {
            switch (enumColor)
            {
                case EnumAttributeColor.white:
                return Color.white;
                case EnumAttributeColor.black:
                return Color.black;
                case EnumAttributeColor.gray:
                return Color.gray;
                case EnumAttributeColor.blue:
                return Color.blue;
                case EnumAttributeColor.red:
                return Color.red;
                case EnumAttributeColor.green:
                return Color.green;
                case EnumAttributeColor.cyan:
                return Color.cyan;
                case EnumAttributeColor.yellow:
                return Color.yellow;
                case EnumAttributeColor.purple:
                return purple;
                case EnumAttributeColor.magenta:
                return Color.magenta;
                case EnumAttributeColor.orange:
                return orange;
                case EnumAttributeColor.silver:
                return silver;
                case EnumAttributeColor.gold:
                return gold;
                case EnumAttributeColor.aqua:
                return aqua;
                case EnumAttributeColor.brown:
                return brown;
                case EnumAttributeColor.fuchsia:
                return fuchsia;
                case EnumAttributeColor.lime:
                return lime;
                case EnumAttributeColor.clear:
                return Color.clear;
            }
            Debug.LogError($"This \"{enumColor}\" Color is not implemented!");
            return default;
        }
    }
}
