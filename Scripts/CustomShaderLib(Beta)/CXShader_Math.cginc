/*
    Made by CXRedix
*/

//Consts
#define PI 3.14159265358979
#define TWOPI 6.2831853071795

//vectors
#define VEC2_ONE float2(1, 1)
#define VEC2_ZERO float2(0, 0)
#define VEC3_ONE float3(1, 1, 1)
#define VEC3_ZERO float3(0, 0, 0)
#define VEC4_ONE float4(1, 1, 1, 1)
#define Vec4_ZERO float4(0, 0, 0, 0)

//Degrees

#define DEGToRAD 0.174532924
#define RADToDEG 57.29578

//maps one value to another, no safety checks
float Map(float value, float In_Min, float In_Max, float Out_Min, float Out_Max)
{
    return ((value - In_Min) * (Out_Max - Out_Min)) / (In_Max - In_Min) + Out_Min;
}

float2 RotateVector2(float2 vec, float Rad_Angle)
{
    float2 output = float2(vec.x * cos(Rad_Angle) - vec.y * sin(Rad_Angle),
                           vec.x * sin(Rad_Angle) + vec.y * cos(Rad_Angle));
}

//clamps the value between 0 and 1
float Clamp01(float value)
{
    return clamp(value, 0, 1);
}

//checks if two float values are close together
bool IsApproximation(float value1, float value2, float approximateQuality = .5)
{
    return abs(value1 - value2) < approximateQuality;
}

