using MyEngine;
using MyEngine.Nodes;
using SFML.Graphics;
using SFML.System;

namespace Agario_2;

public class EatableCircle : Node
{
    private ShapeSprite<CircleShape> _sprite; 
    
    public Vector2f Position
    {
        get => _sprite.Position;
        set => _sprite.Position = value;
    }
    public float Radius
    {
        get => _sprite.UnderlyingShape.Radius;
        set => _sprite.UnderlyingShape.Radius = value;
    }

    public static EatableCircle CreateEatableCircle(float radius, Vector2f position)
    {
        EatableCircle result = new();

        result._sprite = new ShapeSprite<CircleShape>(new CircleShape(radius));
        result.AdoptChild(result._sprite);
        result.Position = position;

        return result;
    }

    public bool Overlaps(EatableCircle other)
    {
        float radiusesSum = Radius + other.Radius;
        float radiusesSumSquared = radiusesSum * radiusesSum;
        
        return Position.SquaredDistanceTo(other.Position) < radiusesSumSquared;
    }
    
    public float Eat()
    {
        Orphan();
        return Radius;
    }
}