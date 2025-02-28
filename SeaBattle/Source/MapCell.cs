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

    private static readonly (CellState, Texture)[] CellTextures = new[]
    {
        ( new CellState([CellTag.HasShip, CellTag.Shot]), new Texture("Resources/Textures/Cells/Shot ship.png") ),
        ( new ([CellTag.HasShip]), new("Resources/Textures/Cells/Ship.png") ),
        ( new ([]), new("Resources/Textures/Cells/Empty.png") ),
    };

    private bool IsShot
        => _state.IsShot;
    
    private MapCell(CellState state)
    {
        _state = state;
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