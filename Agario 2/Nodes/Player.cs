using MyEngine;
using MyEngine.Nodes;
using SFML.System;

namespace Agario_2.Nodes;

public class Player : Node
{
    private EatableCircle _eatableCircle;
    public Vector2f WishedDelta;
    private float _maxSpeed;
    private float _maxSpeedSquared;

    private const float StartingMaxSpeed = 500;
    private const float StartingRadius = 30;

    public float MaxSpeed
    {
        get => _maxSpeed;
        private set
        {
            _maxSpeed = value;
            _maxSpeedSquared = value * value;
        }
    }

    private float Radius
    {
        get => _eatableCircle.Radius;
        set => _eatableCircle.Radius = value;
    }
    
    public Vector2f Position
    {
        get => _eatableCircle.Position;
        set => _eatableCircle.Position = value;
    }
    
    public Camera? DraggedCamera;
    private Player()
    {
        WishedDelta = new(0, 0);
        _maxSpeed = StartingMaxSpeed;
        _maxSpeedSquared = StartingMaxSpeed * StartingMaxSpeed;
    }

    private static Player CreatePlayerWithNoController(Vector2f position)
    {
        Player result = new();
        
        result._eatableCircle = EatableCircle.CreateEatableCircle(StartingRadius, position);
        result.AdoptChild(result._eatableCircle);
        result._eatableCircle.OnEaten += result.Orphan;
        
        return result;
    }

    public static Player CreatePlayer(Vector2f position)
    {
        Player result = CreatePlayerWithNoController(position);
        result.AdoptChild(PlayerController.CreatePlayerController(result));
        
        return result;
    }
    
    public static Player CreateAiPlayer(Vector2f position)
    {
        Player result = CreatePlayerWithNoController(position);
        result.AdoptChild(AiController.CreateAiController(result));
        
        return result;
    }
    
    private Vector2f CalculateCappedDelta()
    {
        float wishedDeltaLengthSquared = WishedDelta.LengthSquared();

        if (wishedDeltaLengthSquared < _maxSpeedSquared) 
            return WishedDelta;
        
        float wishedDeltaLength = float.Sqrt(wishedDeltaLengthSquared);
        return WishedDelta / wishedDeltaLength * _maxSpeed;
    }
    
    protected override void Update(Node root)
    {
        CheckForEatingInNode(root);
        
        DoMovement();
        
        TryDragCamera();
    }

    private void TryDragCamera()
    {
        if (DraggedCamera == null)
            return;

        float interpolation = FrameTiming.DeltaSeconds * 5;
        DraggedCamera.Position = DraggedCamera.Position.Lerp(Position, interpolation);
    }
    
    private void DoMovement()
    {
        Vector2f delta = CalculateCappedDelta() * FrameTiming.DeltaSeconds;
        _eatableCircle.Position += delta;
    }

    private void CheckForEatingInNode(Node root)
    {
        foreach (Node child in root)
        {
            if (child is EatableCircle eatableCircle)
                TryEat(eatableCircle);
            else
                CheckForEatingInNode(child);
        }
    }

    private void TryEat(EatableCircle eatable)
    {
        if (eatable.Radius >= Radius)
            return;

        if (!_eatableCircle.Encloses(eatable))
            return;

        _eatableCircle.Radius += eatable.Eat() * (1 / float.Log2(Radius));
        UpdateMaxSpeed();
    }
    
    private void UpdateMaxSpeed()
        => MaxSpeed = StartingMaxSpeed / Radius * StartingRadius;
}