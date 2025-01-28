using SFML.Window;

namespace MyEngine.MyInput;

public class KeyBind : InputAction
{
    private Keyboard.Key _key;
    private bool _wasPressed;

    private Action _onDown;
    private Action _onPressed;

    private bool IsDown
        => !_wasPressed && IsActive;
    private bool IsPressed
        => _wasPressed && IsActive;
    
    public KeyBind(string name, Keyboard.Key key) : base(name)
    {
        _key = key;
        _wasPressed = false;
    }

    protected virtual bool IsKeyBindPressed()
        => Keyboard.IsKeyPressed(_key);

    public void AddOnDownCallback(Action callback)
        => _onDown += callback;
    public void ResetOnDownCallbacks(Action newValue = null)
        => _onDown = newValue;

    public void AddOnPressedCallback(Action callback)
        => _onPressed += callback;
    public void ResetOnPressedCallbacks(Action newValue = null)
        => _onPressed = newValue;
    
    protected override bool ProcessIsActive()
        => Keyboard.IsKeyPressed(_key);

    public override void Update()
    {
        _wasPressed = IsActive;
        IsActive = IsKeyBindPressed();
    }

    public override void Resolve()
    {
        if (IsDown)
            _onDown?.Invoke();
        if (IsPressed)
            _onPressed?.Invoke();
    }
}