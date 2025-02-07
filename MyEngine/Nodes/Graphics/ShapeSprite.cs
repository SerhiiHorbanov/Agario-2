using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.Graphics;

public class ShapeSprite<T> : Node, Drawable where T : Shape
{ 
    public readonly T UnderlyingShape;

    private ShapeSprite(T shape)
        => UnderlyingShape = shape;

    public Vector2f Position
    {
        get => UnderlyingShape.Position;
        set => UnderlyingShape.Position = value;
    }
    public Color FillColor
    {
        get => UnderlyingShape.FillColor;
        set => UnderlyingShape.FillColor = value;
    }

    public static ShapeSprite<CircleShape> CreateCircleSprite(float radius)
        => new(new(radius));

    public static ShapeSprite<RectangleShape> CreateRectangleSprite(Vector2f size)
        => new(new(size));
    
    public void Draw(RenderTarget target, RenderStates states)
        => UnderlyingShape.Draw(target, states);
}