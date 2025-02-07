using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes.Graphics;

public class Camera : Node
{
    private View _view;
    private RenderTarget _target;
    private RenderLayer _renderedLayer;
    
    public Vector2f Position
    {
        get => _view.Center;
        set => _view.Center = value;
    }

    public Vector2f Size
    {
        get => _view.Size;
        set => _view.Size = value;
    }

    private Camera(uint renderedLayer)
    {
        _renderedLayer = renderedLayer;
        _view = new();
    }

    public static Camera CreateCamera(RenderTarget target)
    {
        Camera result = new(1);

        result._target = target;
        
        return result;
    }
    
    public static Camera CreateUICamera(RenderTarget target)
    {
        Camera result = new(2);

        result._target = target;
        
        return result;
    }
    
    public void Render(Node rootNode)
    {
        Queue<RenderedNode> renderQueue = new();
        AddToRenderQueue(rootNode, renderQueue);

        ApplyView();
        
        foreach (RenderedNode renderedNode in renderQueue)
            renderedNode.Draw(_target, RenderStates.Default);
    }
    
    private void AddToRenderQueue(Node node, Queue<RenderedNode> queue)
    {
        if (node is RenderedNode rendered)
            if (rendered.Layer == _renderedLayer)
                queue.Enqueue(rendered);
        
        foreach (Node child in node)
            AddToRenderQueue(child, queue);
    }
    
    private void ApplyView()
        => _target.SetView(_view);
}