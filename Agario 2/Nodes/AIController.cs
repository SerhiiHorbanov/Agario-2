using MyEngine;
using MyEngine.Nodes;
using MyEngine.Nodes.Controllers;
using MyEngine.Utils;
using SFML.System;

namespace Agario_2.Nodes;

public class AiController : Controller<Player>, IUpdatable
{
    private Vector2f _currentWayPoint;

    private const float MaxDistanceToWayPoint = 200;
    private const float DistanceSquaredForNewWayPoint = 100;

    private AiController()
    { }
    
    protected override void SetControlled(Player newControlled)
    {
        base.SetControlled(newControlled);
        
        SetNewWayPoint();
    }

    public static AiController CreateAiController(Player player)
    {
        AiController result = new();
        
        result.Controlled = player;
        result.SetNewWayPoint();

        return result;
    }

    private void SetNewWayPoint()
    {
        float x = Random.Shared.NextSingle() * MaxDistanceToWayPoint * 2 - MaxDistanceToWayPoint;
        float y = Random.Shared.NextSingle() * MaxDistanceToWayPoint * 2 - MaxDistanceToWayPoint;
        
        _currentWayPoint = Controlled.Position + new Vector2f(x, y);
    }
    
    public void Update(Node root)
    {
        UpdateDelta();
        
        if (TooCloseToWayPoint())
            SetNewWayPoint();
    }

    private bool TooCloseToWayPoint()
        => Controlled.Position.SquaredDistanceTo(_currentWayPoint) < DistanceSquaredForNewWayPoint;
    
    private void UpdateDelta()
    {
        Vector2f delta = _currentWayPoint - Controlled.Position;
        delta /= delta.Length();
        delta *= Controlled.MaxSpeed;
        
        Controlled.WishedDelta = delta;
    }
}