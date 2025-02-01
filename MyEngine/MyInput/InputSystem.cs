using MyEngine.Utils;

namespace MyEngine.MyInput;

public class InputSystem
{
    public InputListener GlobalListener;
    private List<InputListener> _activeListeners;

    private InputSystem()
    {
        GlobalListener = new();
        _activeListeners = new();
    }

    public static InputSystem CreateInputSystem()
    {
        InputSystem result = new();
        
        result.AddListener(result.GlobalListener);

        return result;
    }

    public void AddListener(InputListener listener)
    {
        if (!_activeListeners.Contains(listener))
        {
            _activeListeners.Add(listener);
            listener.System = this;
        }
    }

    public void RemoveListener(InputListener listener)
        => _activeListeners.SwapRemove(listener);

    public void Update()
    {
        foreach (InputListener each in _activeListeners)
            each.UpdateInputActions();
    }

    public void ResolveCallbacks()
    {
        foreach (InputListener each in _activeListeners)
            each.ResolveCallbacks();
    }
}