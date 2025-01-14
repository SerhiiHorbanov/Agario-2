using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MyEngine.Nodes;

public class Camera : Node
{
    private View _view;
    private RenderTarget _target;
    
    public Vector2f Position
    {
        get => _view.Center;
        set
        {
            _target.SetView(_view);
            _view.Center = value;
        }
    }

    private Camera()
    {
        _view = new();
    }

    public static Camera CreateCamera(RenderTarget target)
    {
        Camera result = new();

        result._target = target;
        
        return result;
    }
    
    public void Render(Node rootNode)
    {
        Queue<Drawable> drawables = rootNode.GetRenderQueue(this);

        _target.Clear(Color.Black);
        foreach (Drawable drawable in drawables)
            drawable.Draw(_target, RenderStates.Default);
    }
}