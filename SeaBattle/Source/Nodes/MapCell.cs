using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.ResourceLibraries;
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

    private static Texture _hiddenTexture;
    private static readonly List<(CellState, Texture)> CellTextures = [];

    public bool IsHidden
    {
        set
        {
            if (_isHidden == value)
                return;
            
            _isHidden = value && !HasTag(CellTag.Shot);
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

    public static void InitializeLoadedCellTextures()
    {
        _hiddenTexture = TextureLibrary.GetTexture("hidden cell");
        CellTextures.Add((new([CellTag.HasShip, CellTag.Shot]), TextureLibrary.GetTexture("shot ship cell")));
        CellTextures.Add((new([CellTag.HasShip]), TextureLibrary.GetTexture("ship cell")));
        CellTextures.Add((new([CellTag.Shot]), TextureLibrary.GetTexture("empty cell")));
        CellTextures.Add((new([]), TextureLibrary.GetTexture("empty cell")));
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
    {
        _state.Add(tag);
        UpdateTexture();
    }   
    
    public bool HasTag(CellTag tag)
        => _state.Has(tag);

    public ShootingResult GetShot()
    {
        if (HasTag(CellTag.Shot))
            return ShootingResult.Miss;
        
        AddTag(CellTag.Shot);

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
            _sprite.Texture = _hiddenTexture;
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