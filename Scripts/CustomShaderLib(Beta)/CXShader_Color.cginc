/*
    Made by CXRedix
*/

//Turns the current Color into a luminance color
fixed4 ToLuminanceColor(fixed4 color)
{
    fixed output = Luminance(color);

    return fixed4(output, output, output, color.a);
}
