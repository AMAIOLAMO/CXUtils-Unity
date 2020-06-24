/*
    Made by CXRedix
*/

//tiles the uv (a simple component)
float2 TransformUV(float2 uv, float2 tiling, float2 offset)
{
    return uv * tiling + offset;
}