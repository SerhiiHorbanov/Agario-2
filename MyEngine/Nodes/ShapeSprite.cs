using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes;

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