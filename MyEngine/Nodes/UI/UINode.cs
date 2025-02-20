using MyEngine.Nodes.Graphics;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MyEngine.Nodes.UI;

public abstract class UINode : Node
{
    protected WindowBase Window;

    private Vector2f _anchorOnTarget;
    private Vector2i _offset;
    
    private Vector2i AnchorOffset 
        => (Vector2i)_anchorOnTarget.Scale(Window.Size);

    public Vector2f AnchorOnTarget
    {
        get => _anchorOnTarget;
        set
        {
            _offset += (Vector2i)(value - _anchorOnTarget);
            _anchorOnTarget = value;
        }
    }

    public Vector2i Offset
    {
        get => _offset;
        set
        {
            _offset = value;
            OnPositionSet();
        }
    }
    
    public Vector2i Position
    {
        get => (Vector2i)_anchorOnTarget.Scale(Window.Size) + _offset;
        set
        {
            _offset = value - AnchorOffset;
            OnPositionSet();
        }
    }

    protected UINode(WindowBase window)
    {
        Window = window;
    }

    protected abstract void OnPositionSet();
}