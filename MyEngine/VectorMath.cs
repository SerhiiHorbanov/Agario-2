using SFML.System;

namespace MyEngine;

public static class VectorMath
{
    public static float LengthSquared(this Vector2f vector)
        => vector.SquaredDistanceTo(new Vector2f(0, 0));
    
    public static float Length(this Vector2f vector)
        => float.Sqrt(vector.LengthSquared());
    
    public static float SquaredDistanceTo(this Vector2f from, Vector2f to)
    {
        Vector2f diff = from - to;
        return (diff.X * diff.X) + (diff.Y * diff.Y);
    }

    public static Vector2f Lerp(this Vector2f from, Vector2f to, float interpolation)
        => from + ((to - from) * interpolation);
}