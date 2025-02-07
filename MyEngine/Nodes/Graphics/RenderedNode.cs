using SFML.Graphics;

namespace MyEngine.Nodes.Graphics;

public abstract class RenderedNode : Node, Drawable
{
    public uint Layer { get; protected set; }

    protected RenderedNode(uint layer)
        => Layer = layer;
    
    public abstract void Draw(RenderTarget target, RenderStates states);
}