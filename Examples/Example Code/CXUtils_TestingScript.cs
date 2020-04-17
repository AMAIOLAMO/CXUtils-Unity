using UnityEngine;
/*
 * Made by CXRedix
 * Free tool for unity.
 */
namespace CXUtils.HelperAttributes.test
{
    public class CXUtils_TestingScript : MonoBehaviour
    {
        [OverrideLabel("Override the name")]
        public string str1 = "This is a name!";

        [ColorModifier(EnumAttributeColor.aqua)]
        public string str2 = "We Color it";

        [OverrideLabel("Enabler")]
        public bool bool1 = true;

        [ActiveIf("bool1")]
        public string str3 = "We Enable / Active it~";

        [InActiveIf("bool1")]
        public string str4 = "And this is reverse";

        public Texture texture1;

        [Icon("texture1")]
        public string str5 = "We even add a cute little Icon On the left!";

        public Rect rect1;

        [OverrideLabel("Hi! I'm a name!")]
        [ColorModifier(EnumAttributeColor.cyan)]
        [Icon("texture1")]
        [ColorModifier(EnumAttributeColor.white)]
        [DrawRect("rect1", EnumAttributeColor.silver)]
        public string str6 = "Oh and the most of the part is... You could do it all at the same time!";

    }
}

