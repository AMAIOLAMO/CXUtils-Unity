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