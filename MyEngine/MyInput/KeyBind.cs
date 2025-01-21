using SFML.Window;

namespace MyEngine.MyInput;

public class KeyBind
{
    public readonly string Name; 
    
    private Keyboard.Key _key;
    private bool _wasPressed;
    private bool _isPressed;

    private Action _onDown;
    
    public KeyBind(string name, Keyboard.Key key)
    {
        Name = name;
        _key = key;
        _wasPressed = false;
        _isPressed = false;
    }

    protected virtual bool IsKeyBindPressed()
        => Keyboard.IsKeyPressed(_key);

    public void AddOnDownCallback(Action callback)
        => _onDown += callback;
    public void ResetOnDownCallbacks()
        => _onDown = null;
    
    public void Update()
    {
        _wasPressed = _isPressed;
        _isPressed = IsKeyBindPressed();
    }
    
    public void ResolveCallbacks()
    {
        if (!_wasPressed && _isPressed)
            _onDown.Invoke();
    }
}