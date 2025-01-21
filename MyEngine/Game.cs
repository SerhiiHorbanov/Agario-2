using MyEngine.MyInput;
using SFML.Graphics;
using MyEngine.Nodes;
using SFML.Window;

namespace MyEngine;

public abstract class Game
{
    protected Node Root;
    protected RenderWindow Window;

    protected KeyBindManager KeyBinds;
    protected Camera? CurrentCamera;
    
    public void Run()
    {
        Initialization();
        
        while (ContinueGame())
        {
            Render();
            Input();
            Update();
            Timing();
        }
    }

    private void Initialization()
    {
        Root = Node.CreateNode();
        KeyBinds = new KeyBindManager();
        
        InitializeWindow();
        InitializeCamera();
        
        FrameTiming.UpdateLastTimingTick();
        
        GameSpecificInitialization();
    }

    private void InitializeCamera()
    {
        CurrentCamera = Camera.CreateCamera(Window);
        Root.AdoptChild(CurrentCamera);
    }

    private void InitializeWindow()
    {
        Window = new RenderWindow(new(900, 900), "Window");
        Window.Closed += (sender, args) => Window.Close();
    }

    protected abstract void GameSpecificInitialization();
    
    private bool ContinueGame()
        => Window.IsOpen;

    private void Render()
    {
        CurrentCamera?.Render(Root);
        Window.Display();
    }

    private void Input()
    {
        Window.DispatchEvents();
        
        MouseInput.UpdateInput(Window);
        KeyBinds.UpdateKeyBinds();
        
        Root.ProcessInputTree();
    }

    private void Update()
    {
        Root.UpdateTree();
        KeyBinds.ResolveCallbacks();
    }

    private void Timing()
    {
        FrameTiming.Timing();
    }
}