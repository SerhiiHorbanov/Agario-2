using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.Graphics;

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
    public Color FillColor
    {
        get => UnderlyingShape.FillColor;
        set => UnderlyingShape.FillColor = value;
    }
    
    public void Draw(RenderTarget target, RenderStates states)
        => UnderlyingShape.Draw(target, states);
}