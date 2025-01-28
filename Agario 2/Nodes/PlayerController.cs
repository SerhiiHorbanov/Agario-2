using MyEngine.MyInput;
using MyEngine.Nodes;
using SFML.Window;

namespace Agario_2.Nodes;

public class PlayerController : InputBasedController<Player>, IProcessesInput
{
    private PlayerController() : base(new InputListener())
    { }
    
    protected override void SetControlled(Player newControlled)
    {
        base.SetControlled(newControlled);

        Input.GetAction<KeyBind>("dash").ResetOnDownCallbacks();
        Input.GetAction<KeyBind>("body swap").ResetOnDownCallbacks();

        if (newControlled == null)
            return;
        
        Input.GetAction<KeyBind>("dash").AddOnDownCallback(Controlled.Dash);
        Input.GetAction<KeyBind>("body swap").AddOnDownCallback(Controlled.SwapBodies);
    }

    public static PlayerController CreatePlayerController(InputSystem inputSystem, Player player = null)
    {
        PlayerController result = new();

        result.InitializeKeyBinds();
        inputSystem.AddListener(result.Input);
        result.Controlled = player;
        
        return result;
    }

    private void InitializeKeyBinds()
    {
        Input.AddAction(new KeyBind("dash", Keyboard.Key.Space));
        Input.AddAction(new KeyBind("body swap", Keyboard.Key.F));
    }
    
    public void ProcessInput()
        => Controlled.WishedDelta = MouseInput.MousePositionFromWindowCenter;
}