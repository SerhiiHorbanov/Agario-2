using MyEngine;
using MyEngine.Nodes;
using SFML.System;

namespace Agario_2.Nodes;

public class AiController : Node, IUpdatable
{
    private Player _player;
    private Vector2f _currentWayPoint;

    private const float MaxDistanceToWayPoint = 200;
    private const float DistanceSquaredForNewWayPoint = 100;
    
    private AiController(Player player)
    {
        _player = player;
        SetNewWayPoint();
    }

    public static AiController CreateAiController(Player player)
        => new(player);

    private void SetNewWayPoint()
    {
        float x = Random.Shared.NextSingle() * MaxDistanceToWayPoint * 2 - MaxDistanceToWayPoint;
        float y = Random.Shared.NextSingle() * MaxDistanceToWayPoint * 2 - MaxDistanceToWayPoint;
        
        _currentWayPoint = _player.Position + new Vector2f(x, y);
    }
    
    public void Update(Node root)
    {
        UpdateDelta();
        
        if (TooCloseToWayPoint())
            SetNewWayPoint();
    }

    private bool TooCloseToWayPoint()
        => _player.Position.SquaredDistanceTo(_currentWayPoint) < DistanceSquaredForNewWayPoint;
    
    private void UpdateDelta()
    {
        Vector2f delta = _currentWayPoint - _player.Position;
        delta /= delta.Length();
        delta *= _player.MaxSpeed;
        
        _player.WishedDelta = delta;
    }
}