namespace MyEngine.Nodes.Graphics;

public readonly record struct RenderLayer(uint Value)
{
    private static readonly RenderLayer NotRenderedLayer = 0;
    private static readonly RenderLayer NormalLayer = 1;
    private static readonly RenderLayer UILayer = 2;
    
    public bool IsRendered
        => Value != NotRenderedLayer;
    public bool IsNormalLayer
        => Value == NormalLayer;
    public bool IsUI
        => Value == UILayer;
    
    public static implicit operator RenderLayer(uint layer)
        => new(layer);
}