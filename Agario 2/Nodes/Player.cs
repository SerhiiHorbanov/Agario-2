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

    private Camera? _draggedCamera;
    
    private const float StartingMaxSpeed = 500;
    private const float StartingRadius = 30;

    private int _dashingFramesLeft;

    private const int DashSpeedMultiplier = 4;
    private const int DashSpanFrames = 20;
    
    public Camera? DraggedCamera
    {
        get => _draggedCamera;
        set
        {
            _draggedCamera = value;
            UpdateСameraSize();
        }
    }

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

    private float CurrentDashSpeedMultiplier
        => _dashingFramesLeft > 0 ? DashSpeedMultiplier : 1;
    
    private Player()
    {
        _dashingFramesLeft = 0;
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

        ProcessDash();
        TryDragCamera();
    }

    private void ProcessDash()
    {
        if (_dashingFramesLeft != 0)
        {
            _dashingFramesLeft--;
        }
    }

    public void Dash()
    {
        _dashingFramesLeft = DashSpanFrames;
    }

    public void SwapBodies()
    {
        Node root = GetRootNode();

        List<Player> players = root.GetChildrenOfType<Player>();

        Player randomPlayer = players[Random.Shared.Next(players.Count)];

        randomPlayer.AdoptChild(_eatableCircle);
        AdoptChild(randomPlayer._eatableCircle);

        (randomPlayer._eatableCircle, _eatableCircle) = (_eatableCircle, randomPlayer._eatableCircle);
        (randomPlayer._eatableCircle.OnEaten, _eatableCircle.OnEaten) = (_eatableCircle.OnEaten, randomPlayer._eatableCircle.OnEaten);
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
        Vector2f delta = CalculateCappedDelta() * FrameTiming.DeltaSeconds * CurrentDashSpeedMultiplier;
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
        UpdateСameraSize();
    }

    private void UpdateMaxSpeed()
        => MaxSpeed = StartingMaxSpeed / float.Max(1, float.Log10(Radius - StartingRadius));

    private float CalculateCameraSizeMultiplier()
        => 1 + (Radius - StartingRadius) / 200;
    
    private void UpdateСameraSize()
    {
        if (DraggedCamera != null)
            DraggedCamera.Size = DraggedCamera.RenderTargetSize * CalculateCameraSizeMultiplier();
    }
}