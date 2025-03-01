using MyEngine.Nodes;
using MyEngine.Utils;
using SFML.System;

namespace SeaBattle.Nodes;

public class PlayerMap : Node
{
    private readonly Vector2i _size;
    private readonly MapCell[,] _cells;

    private CursorOnMap _cursor;

    private static readonly Vector2i DefaultSize = new(10, 10);
    private static readonly Vector2f CellSize = new(64, 64);
    private const uint ShipsCount = 10;

    public bool IsHidden
    {
        set
        {
            foreach (MapCell cell in _cells)
                cell.IsHidden = value;
        }
    }
    
    private PlayerMap(Vector2i size)
    {
        _size = size;
        _cells = new MapCell[size.X, size.Y];
    }

    public static PlayerMap CreateMap(Vector2f position)
        => CreateMap(DefaultSize, position);
    
    public static PlayerMap CreateMap(Vector2i size, Vector2f position)
    {
        PlayerMap map = new(size);

        map.InitializeCells(size, position);
        map._cursor = CursorOnMap.CreateCursorOnMap(map);
        map.AdoptChild(map._cursor);
        
        return map;
    }
    
    private void InitializeCells(Vector2i size, Vector2f position)
    {
        for (int y = 0; y < size.Y; y++)
        {
            for (int x = 0; x < size.X; x++)
            {
                Vector2f offset = CellSize.Scale(new Vector2f(x, y));
                
                MapCell cell = MapCell.CreateCell(position + offset);
                AdoptChild(cell);
                
                _cells[y, x] = cell;
            }
        }
        
        for (int i = 0; i < ShipsCount; i++)
        {
            int x = Random.Shared.Next(0, size.X);
            int y = Random.Shared.Next(0, size.Y);
            
            _cells[y, x].AddTag(CellTag.HasShip);
        }
    }
    
    public ShootingResult Shoot(Vector2i position)
    {
        if (IsOutside(position))
            return ShootingResult.Miss;
        
        return _cells[position.Y, position.X].GetShot();
    }

    public ShootingResult ShootAtCursor()
        => Shoot(_cursor.Position);
    
    public MapCell GetCell(Vector2i position)
    {
        if (IsOutside(position))
            return null;
        return _cells[position.Y, position.X];
    }

    public bool IsOutside(Vector2i position)
    {
        bool lessThanZero = position.X < 0 || position.Y < 0;
        bool moreThanSize = position.X >= _size.X || position.Y >= _size.Y;
        
        return moreThanSize || lessThanZero;
    }

    public bool HasNotShotShip()
        => HasCellWithTagAndWithoutTag(CellTag.HasShip, CellTag.Shot);
    
    public bool HasCellWithTagAndWithoutTag(CellTag with, CellTag without)
    {
        foreach (MapCell cell in _cells)
        {
            if (cell.HasTag(with) && !cell.HasTag(without))
                return true;
        }
        
        return false;
    }
    
    public void MoveCursor(Vector2i position)
        => _cursor.Position += position;
}