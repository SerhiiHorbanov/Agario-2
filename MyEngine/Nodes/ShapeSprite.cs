using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes;

using CircleSprite = ShapeSprite<CircleShape>;
using RectangleSprite = ShapeSprite<CircleShape>;
using ConvexSprite = ShapeSprite<CircleShape>;
    
public class ShapeSprite<T> : Node where T : Shape
{ 
    public readonly T UnderlyingShape;

    public ShapeSprite(T shape)
        => UnderlyingShape = shape;

    public Vector2f Position
    {
        get => UnderlyingShape.Position;
        set => UnderlyingShape.Position = value;
    }

    protected override void Render(RenderTarget target)
        => UnderlyingShape.Draw(target, RenderStates.Default);
}