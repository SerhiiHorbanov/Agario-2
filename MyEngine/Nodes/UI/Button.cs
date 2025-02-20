using MyEngine.MyInput;
using MyEngine.MyInput.InputActions;
using MyEngine.Nodes.Graphics;
using MyEngine.ResourceLibraries;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MyEngine.Nodes.UI;

public class Button : UINode, IDisposable
{
    private readonly InputListener _inputListener;
    private SpriteNode _sprite;

    private const Mouse.Button PressButton = Mouse.Button.Left;

    public Action OnPressed;
    public Action OnReleased;
    
    private bool IsPressed { get; set; }

    private SpriteNode Sprite
    {
        get => _sprite;
        set => _sprite = AdoptChild(value);
    }
    
    private FloatRect PressableArea 
        => _sprite.Sprite.GetGlobalBounds();

    private Button(WindowBase window) : base(window)
    {
        _inputListener = new();
    }

    protected override void OnPositionSet()
    {
        Sprite.Position = (Vector2f)Position;
    }

    private void OnMouseClicked()
    {
        Vector2i mousePosition = Mouse.GetPosition(Window);
        
        if (PressableArea.Contains(mousePosition))
        {
            IsPressed = true;
            OnPressed?.Invoke();
        }
    }

    private void OnMouseReleased()
    {
        if (IsPressed)
            OnReleased?.Invoke();
        
        IsPressed = false;
    }
    
    public static Button CreateButton(WindowBase window, InputSystem inputSystem)
    {
        Button result = new(window);

        inputSystem.AddListener(result._inputListener);
        ClickBind bind = result._inputListener.AddAction(new ClickBind("button press", Mouse.Button.Left));
        bind.AddOnStartedCallback(result.OnMouseClicked);
        bind.AddOnEndedCallback(result.OnMouseReleased);
        
        result.Sprite = SpriteNode.CreateSprite(RenderLayer.UILayer);

        return result;
    }

    public static Button CreateButton(WindowBase window, InputSystem inputSystem, string textureName)
    {
        Button result = CreateButton(window, inputSystem);
        
        result.Sprite.Texture = TextureLibrary.GetTexture(textureName);
        
        return result;
    }

    public void Dispose()
    {
        ClickBind bind = _inputListener.GetAction<ClickBind>("button press");
        bind.ResetCallbacks();
        
        _inputListener.Dispose();
    }
}