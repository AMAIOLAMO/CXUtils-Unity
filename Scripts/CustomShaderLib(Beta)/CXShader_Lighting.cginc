/*
    Made by CXRedix
*/

//Creates a simple diffuse lighting
fixed4 DiffuseLighting(fixed3 lightingDirection, fixed3 vertexNormal,
 fixed3 mainColor = fixed3(1, 1, 1), fixed fallOffIntensity = 0, bool doSaturate = false)
{
    //diffuse light (Raw)
    float diffuseLight = dot(lightingDirection, vertexNormal);

    //lighting fall off (the same as diffuse light, but modified)
    float lightFallOff = (doSaturate ? saturate(diffuseLight) : diffuseLight) + fallOffIntensity;
    
    //returns the falling off light intensity blended with the mainColor with the full alpha
    return lightFallOff * fixed4(mainColor.rgb, 1);
}

fixed4 PhongLighting(float3 camPos,
 float3 currentFragWorldPos, fixed3 currentFragNormal, fixed3 worldLightDir, float glossiness = 1)
 {
    /*
    How does this work:
    Think of a camera, a light source and an object,
    When the camera sees each vertex of the mesh, imagine the camera shoots rays to the vertex,
    try to reflect the ray then you'll get a vector that is been reflected,
    when you compare it with the light source direction,
    the closer it is, the lighter that vertex should be
    then because the normals are actually interpolated by Unity (It won't be normalized)
    we normalize it back, so it won't be weird looking.
    and that's how it work! Woohoo~
    */
    //get's the direction vector from the current vertex's world position to camera world position
    float3 fragToCameraDir = camPos - currentFragWorldPos;

    //normalize the fragment to camera direction (since we don't want the magnitude to affect the lighting)
    float3 viewDir = normalize(fragToCameraDir);
    
    //get's the reflect of the direction with the view direction from the camera to the current vertex's normal
    float3 reflectedLightDir = reflect(-viewDir, currentFragNormal);

    //saturates the dot of reflect with the world light to get the lighting value between 0 ~ 1
    float dotOfReflect = saturate(dot(reflectedLightDir, worldLightDir));
    
    //The last is the result
    float specularFallOff = pow(dotOfReflect, glossiness);

    return specularFallOff;
 }

fixed4 PhongLighting(fixed4 diffuseLighting, float3 camPos,
 float3 currentFragWorldPos, fixed3 currentFragNormal, fixed3 worldLightDir, float glossiness = 1)
{
    return diffuseLighting +
    PhongLighting(camPos, currentFragWorldPos, currentFragNormal, worldLightDir, glossiness);
}