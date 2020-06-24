/*
    Made by CXRedix
    Currently in test
*/

//---------------------------------------------------------

//Random function (Randoms between 0 ~ 1)
float Random(float2 seed) // very bad implementation
{
    return frac(sin(dot(seed, float2(12.9898, 78.233))) * 43758.5453);
}

float Random2(float2 seed) // very bad implementation
{
    return frac(dot(seed, float2(12.9898, 78.233)) * 43758.5453);
}

//---------------------------------------------------------

// NOISE:

//A simple noise
float SimpleNoise(float2 pos, float2 seed)
{
    return Random(seed + pos);
}

//A simple noise
float SimpleNoise2(float2 pos, float2 seed)
{
    return Random2(seed + pos);
}

//---------------------------------------------------------

//returns a random generated boolean
bool FlipCoin(float2 seed, float threashHold = .5)
{
    return (seed > threashHold) ? true : false;
}

