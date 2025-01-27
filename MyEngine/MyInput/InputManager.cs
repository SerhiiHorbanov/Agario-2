using SFML.Window;

namespace MyEngine.MyInput;

public class InputManager
{
    private List<InputAction> _inputActions;
    
    public InputManager()
        => _inputActions = new List<InputAction>();

    public void UpdateInputActions()
    {
        foreach(InputAction each in _inputActions)
            each.Update();
    }

    public void ResolveCallbacks()
    {
        foreach (InputAction each in _inputActions)
            each.Resolve();
    }

    public T GetAction<T>(string name) where T : InputAction
        => _inputActions.Single(x => x is T && x.Name == name) as T;

    public T AddAction<T>(T action) where T : InputAction
    {
        _inputActions.Add(action);
        return action;
    }
}