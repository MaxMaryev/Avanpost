using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
    }

    public static Vector3 DirectionTo(this Vector3 source, Vector3 distination)
    {
        return Vector3.Normalize(distination - source);
    }

    public static float SqrMagnitudeXZ(this Vector3 source)
    {
        return source.x * source.x + source.z * source.z;
    }
}
