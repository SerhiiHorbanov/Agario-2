using SFML.Graphics;
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

    public static Vector2f RandomPositionInside(this FloatRect bounds)
    {
        float x = Random.Shared.NextSingle() * bounds.Width + bounds.Left;
        float y = Random.Shared.NextSingle() * bounds.Height + bounds.Top;
        
        return new Vector2f(x, y);
    }
}