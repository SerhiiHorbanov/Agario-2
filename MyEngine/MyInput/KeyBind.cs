using SFML.Window;

namespace MyEngine.MyInput;

public class KeyBind : InputAction
{
    private Keyboard.Key _key;
    private bool _wasPressed;

    private Action _onDown;
    
    public KeyBind(string name, Keyboard.Key key) : base(name)
    {
        _key = key;
        _wasPressed = false;
    }

    protected virtual bool IsKeyBindPressed()
        => Keyboard.IsKeyPressed(_key);

    public void AddOnDownCallback(Action callback)
        => _onDown += callback;
    public void ResetOnDownCallbacks()
        => _onDown = null;

    protected override bool ProcessIsActive()
        => Keyboard.IsKeyPressed(_key);

    public override void Update()
    {
        _wasPressed = IsActive;
        IsActive = IsKeyBindPressed();
    }

    public override void Resolve()
    {
        if (!_wasPressed && IsActive)
            _onDown.Invoke();
    }
}