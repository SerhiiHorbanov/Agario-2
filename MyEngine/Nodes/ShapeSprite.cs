using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes;

public class ShapeSprite<T> : Node, Drawable where T : Shape
{ 
    public readonly T UnderlyingShape;

    public ShapeSprite(T shape)
        => UnderlyingShape = shape;

    public Vector2f Position
    {
        get => UnderlyingShape.Position;
        set => UnderlyingShape.Position = value;
    }
    
    public void Draw(RenderTarget target, RenderStates states)
        => UnderlyingShape.Draw(target, states);
}