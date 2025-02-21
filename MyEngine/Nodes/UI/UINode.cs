using MyEngine.Nodes.Graphics;
using MyEngine.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MyEngine.Nodes.UI;

public abstract class UINode : Node
{
    protected readonly Camera Camera;

    private Vector2f _anchorOnTarget;
    private Vector2i _offset;
    
    private Vector2i AnchorOffset 
        => (Vector2i)_anchorOnTarget.Scale(Camera.Size);

    public Vector2f AnchorOnTarget
    {
        get => _anchorOnTarget;
        set
        {
            _anchorOnTarget = value;
            OnPositionSet();
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
        get => (Vector2i)_anchorOnTarget.Scale(Camera.Size) + _offset;
        set
        {
            _offset = value - AnchorOffset;
            OnPositionSet();
        }
    }

    protected UINode(Camera camera)
    {
        Camera = camera;
    }

    protected abstract void OnPositionSet();
}