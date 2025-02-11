using MyEngine.Nodes;
using MyEngine.Nodes.Controllers;
using MyEngine.Utils;
using SFML.System;

namespace Agario_2.Nodes;

public class AiController : Controller<Player>
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
        float x = MyRandom.GetFloatInDistance(_currentWayPoint.X, MaxDistanceToWayPoint);
        float y = MyRandom.GetFloatInDistance(_currentWayPoint.Y, MaxDistanceToWayPoint);
        
        _currentWayPoint = new(x, y);
    }
    
    protected override void Update(in UpdateInfo info)
    {
        if (Controlled == null)
            return;
        
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