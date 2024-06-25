using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine;

public static class CesiumGeoreferenceExtensions
{
    public static Vector3 GeoToCartesian(this CesiumGeoreference georeference, Message data)
    {
        return georeference.GeoToCartesian(data.lon, data.lat, data.alt);
    }
    
    public static Vector3 GeoToCartesian(this CesiumGeoreference georeference, float longitude, float latitude, float altitude)
    {
        // Don't ask how i found those 2 magic spells in the source xD

        // geo (lon, lat, alt) -> earth centered
        double3 earthFixed = CesiumWgs84Ellipsoid.LongitudeLatitudeHeightToEarthCenteredEarthFixed(new double3(longitude, latitude, altitude));
        // earth centered coords -> coords dependend on map (georeference)
        double3 coords = georeference.TransformEarthCenteredEarthFixedPositionToUnity(earthFixed);

        return new Vector3((float)coords.x, (float)coords.y, (float)coords.z);
    }
}