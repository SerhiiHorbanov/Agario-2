using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes;

public class Camera : Node
{
    private View _view;
    private RenderTarget _target;
    
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

    private Camera()
        => _view = new();

    public static Camera CreateCamera(RenderTarget target)
    {
        Camera result = new();

        result._target = target;
        
        return result;
    }
    
    public void Render(Node rootNode)
    {
        Queue<Drawable> drawables = rootNode.GetRenderQueue(this);

        ApplyView();
        
        foreach (Drawable drawable in drawables)
            drawable.Draw(_target, RenderStates.Default);
    }
    
    private void ApplyView()
        => _target.SetView(_view);
}