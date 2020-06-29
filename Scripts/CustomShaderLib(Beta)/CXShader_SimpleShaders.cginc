/*
    Made by CXRedix
*/

///<summary> Get's a blended outline on the mesh </summary>
float4 BlendOutline_Spherical(float3 fragWorldPos, float3 camWorldPos, fixed3 fragNormal, float intensity = 1)
{
    fixed3 fragToCamDir = normalize(camWorldPos - fragWorldPos);
    float dotOfFragToCamDirAndFragNorm = saturate(dot(fragToCamDir, fragNormal)); // clamps the value to 0 ~ 1 for no negative values in color
    return (1 - dotOfFragToCamDirAndFragNorm) * intensity;
}