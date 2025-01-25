using SFML.Window;

namespace MyEngine.MyInput;

public class InputManager
{
    public List<InputAction> KeyBinds;

    public InputManager()
        => KeyBinds = new List<InputAction>();

    public void UpdateKeyBinds()
    {
        foreach(InputAction each in KeyBinds)
            each.Update();
    }

    public void ResolveCallbacks()
    {
        foreach(InputAction each in KeyBinds)
            each.ResolveCallbacks();
    }
    
    public InputAction AddKeyBind(string name, Keyboard.Key key)
    {
        InputAction result = new InputAction(name, key);
        KeyBinds.Add(result);
        
        return result;
    }
    
    public InputAction GetKeyBind(string name)
        => KeyBinds.Single(x => x.Name == name);
}