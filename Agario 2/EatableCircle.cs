using MyEngine;
using MyEngine.Nodes;
using SFML.Graphics;
using SFML.System;

namespace Agario_2;

public class EatableCircle : Node
{
    private ShapeSprite<CircleShape> _sprite; 
    public Action OnEaten;

    private static readonly Color[] Colors = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Cyan, Color.Magenta, Color.Yellow };
    
    public Vector2f Position
    {
        get => _sprite.Position;
        set => _sprite.Position = value;
    }
    public float Radius
    {
        get => _sprite.UnderlyingShape.Radius;
        set
        {
            _sprite.UnderlyingShape.Radius = value;   
            _sprite.UnderlyingShape.Origin = new(value, value);
        }
    }

    public static EatableCircle CreateEatableCircle(float radius, Vector2f position)
    {
        EatableCircle result = new();

        result._sprite = new ShapeSprite<CircleShape>(new CircleShape(radius));
        result.Radius = radius;
        result.AdoptChild(result._sprite);
        result.Position = position;
        result.OnEaten += result.Orphan;
        result._sprite.FillColor = GetRandomColor();
        
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
        OnEaten();
        return Radius;
    }

    static Color GetRandomColor()
    {
        return Colors[Random.Shared.Next(Colors.Length)];
    }
}