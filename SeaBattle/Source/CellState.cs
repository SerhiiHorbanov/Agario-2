using System.Collections;

namespace SeaBattle;

public enum CellTag
{
    HasShip,
    HasMine,
    Shot,
}

public readonly struct CellState(List<CellTag> cellTags) : IEnumerable<CellTag>, IEquatable<CellState>
{
    private readonly List<CellTag> _cellTags = cellTags;
    public bool Has(CellTag tag)
        => _cellTags.Contains(tag);
    
    public bool IsShot
        => _cellTags.Contains(CellTag.Shot);
    
    public void Add(CellTag tag)
    {
        if (!Has(tag))
            _cellTags.Add(tag);  
    }

    public IEnumerator<CellTag> GetEnumerator()
        => _cellTags.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => _cellTags.GetEnumerator();
    
    // only works when all the tags are arranged the same way
    public bool Equals(CellState other)
        => _cellTags.SequenceEqual(other._cellTags);
    public static bool operator ==(CellState left, CellState right)
        => left.Equals(right);
    public static bool operator !=(CellState left, CellState right)
        => !left.Equals(right);
}