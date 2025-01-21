using SFML.Window;

namespace MyEngine.MyInput;

public class KeyBindManager
{
    public List<KeyBind> KeyBinds;

    public KeyBindManager()
        => KeyBinds = new List<KeyBind>();

    public void UpdateKeyBinds()
    {
        foreach(KeyBind each in KeyBinds)
            each.Update();
    }

    public void ResolveCallbacks()
    {
        foreach(KeyBind each in KeyBinds)
            each.ResolveCallbacks();
    }
    
    public KeyBind AddKeyBind(string name, Keyboard.Key key)
    {
        KeyBind result = new KeyBind(name, key);
        KeyBinds.Add(result);
        
        return result;
    }
    
    public KeyBind GetKeyBind(string name)
        => KeyBinds.Single(x => x.Name == name);
}