/*
    Made by CXRedix
*/

//blends two colors
fixed4 BlendTwoColors(fixed4 color1, fixed4 color2, fixed blend)
{
    color1 *= 1 - blend;
    color2 *= blend;

    return color1 + color2;
}

//A simple function for manipulating a Color
fixed4 ManipulateColor(fixed4 mainColor, fixed4 shiftingColor, fixed4 tintColor)
{
    return mainColor * shiftingColor + tintColor;
}

//Turns the current Color into a luminance color
fixed4 ToLuminanceColor(fixed4 color)
{
    fixed output = Luminance(color);

    return fixed4(output, output, output, color.a);
}
