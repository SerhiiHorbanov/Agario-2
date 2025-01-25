using MyEngine;
using MyEngine.Nodes;
using SFML.System;

namespace Agario_2.Nodes;

public class Player : Node, IUpdatable
{
    private EatableCircle _body;
    public Vector2f WishedDelta;
    private float _maxSpeed;
    private float _maxSpeedSquared;

    private Camera? _draggedCamera;
    
    private const float StartingMaxSpeed = 500;
    private const float StartingRadius = 30;

    private int _dashingFramesLeft;

    private const int DashSpeedMultiplier = 4;
    private const int DashSpanFrames = 20;
    
    private EatableCircle Body
    {
        get => _body;
        set
        {
            if (_body != null)
            {
                DetachChild(_body);
                _body.OnEaten -= Orphan;
            }

            _body = value;
            
            AdoptChild(_body);
            _body.OnEaten += Orphan;
        }
    }
    
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
        get => _body.Radius;
        set => _body.Radius = value;
    }
    
    public Vector2f Position
    {
        get => _body.Position;
        set => _body.Position = value;
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
        
        result.Body = EatableCircle.CreateEatableCircle(StartingRadius, position);
        
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
    
    public void Update(Node root)
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
        Player randomPlayer = players.GetRandomElement();

        (randomPlayer.Body, Body) = (Body, randomPlayer.Body);
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
        Position += delta;
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

        if (!_body.Encloses(eatable))
            return;

        Radius += eatable.Eat() * (1 / float.Log2(Radius));
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