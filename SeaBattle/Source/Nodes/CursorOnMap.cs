using MyEngine.Nodes;
using SFML.Graphics;
using SFML.System;

namespace SeaBattle.Nodes;

public class CursorOnMap : Node
{
    private PlayerMap _map;

    private Vector2i _position;

    public Vector2i Position
    {
        get => _position;
        set
        {
            if (_map.IsOutside(value))
                return;
            _map.GetCell(_position).Color = OffCellColor;
            _map.GetCell(value).Color = OnCellColor;
            
            _position = value;
        }
    }

    private static readonly Color OffCellColor = new(255, 255, 255, 255);
    private static readonly Color OnCellColor = new(200, 200, 200, 255);
    
    private CursorOnMap()
    {
        _position = new(0, 0);
    }

    public static CursorOnMap CreateCursorOnMap(PlayerMap map)
    {
        CursorOnMap cursor = new();

        cursor._map = map;
        cursor.Position = new(0, 0);
        
        return cursor;
    }
}