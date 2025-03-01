using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using SFML.Graphics;
using SFML.System;

namespace SeaBattle;

public enum ShootingResult
{
    Miss,
    Hit
}

public class MapCell : Node
{
    private SpriteNode _sprite;
    private readonly CellState _state;
    private bool _isHidden;

    private static readonly Texture HiddenTexture = new Texture("Resources/Textures/Cells/Hidden.png");
    private static readonly (CellState, Texture)[] CellTextures =
    [
        (new([CellTag.HasShip, CellTag.Shot]), new("Resources/Textures/Cells/Shot ship.png")),
        (new([CellTag.HasShip]), new("Resources/Textures/Cells/Ship.png")),
        (new([]), new("Resources/Textures/Cells/Empty.png")),
    ];

    private bool IsShot
        => _state.IsShot;

    public bool IsHidden
    {
        set
        {
            if (_isHidden == value)
                return;
            _isHidden = value;
            UpdateTexture();
        }
    }

    public Color Color
    {
        get => _sprite.Sprite.Color;
        set => _sprite.Sprite.Color = value;
    }
    
    private MapCell(CellState state)
    {
        _state = state;
        _isHidden = false;
    }
    
    public static MapCell CreateCell(Vector2f spritePosition)
    {
        MapCell cell = new(new([]));
        
        SpriteNode sprite = SpriteNode.CreateSprite(RenderLayer.NormalLayer);
        sprite.Position = spritePosition;
        
        cell._sprite = sprite;
        cell.AdoptChild(sprite);
        cell.UpdateTexture();
        
        return cell;
    }

    public void AddTag(CellTag tag)
        => _state.Add(tag);
    
    public ShootingResult GetShot()
    {
        if (IsShot)
            return ShootingResult.Miss;
        
        _state.Add(CellTag.Shot);
        UpdateTexture();

        if (_state.Has(CellTag.HasShip))
            return ShootingResult.Hit;
        return ShootingResult.Miss;
    }

    private void UpdateTexture()
    {
        if (_sprite == null)
            return;
        if (_isHidden)
        {
            _sprite.Texture = HiddenTexture;
            return;
        }
        
        _sprite.Texture = GetTexture(_state);
    }

    private static Texture GetTexture(CellState cellState)
    {
        foreach ((CellState otherState, Texture texture) in CellTextures)
        {
            if (cellState == otherState)
                return texture;
        }

        return null;
    }
}