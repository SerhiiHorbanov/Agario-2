using SFML.Graphics;
using SFML.System;

namespace MyEngine.Nodes;

public class Camera : Node
{
    private View _view;
    private RenderTarget _target;


    private bool changedView;
    
    public Vector2f Position
    {
        get => _view.Center;
        set
        {
            _view.Center = value;
            changedView = true;
        }
    }

    public Vector2f Size
    {
        get => _view.Size;
        set
        {
            _view.Size = value;
            changedView = true;
        }
    }

    public Vector2f RenderTargetSize
        => (Vector2f)_target.Size;
    
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
        UpdateView();
        
        Queue<Drawable> drawables = rootNode.GetRenderQueue(this);

        _target.Clear(Color.Black);
        foreach (Drawable drawable in drawables)
            drawable.Draw(_target, RenderStates.Default);
    }

    private void UpdateView()
    {
        if (changedView)
            _target.SetView(_view);
    }
}