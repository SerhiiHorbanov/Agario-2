using MyEngine.MyInput;
using SFML.Graphics;
using MyEngine.Nodes;
using SFML.Window;

namespace MyEngine;

public abstract class Game
{
    protected Node Root;
    protected RenderWindow Window;

    protected InputSystem Input;
    protected Camera CurrentCamera; 
    
    public void Run()
    {
        Initialization();
        
        while (ContinueGame())
        {
            Render();
            ProcessInput();
            Update();
            Timing();
        }
    }

    private void Initialization()
    {
        Root = Node.CreateNode();
        
        InitializeWindow();
        InitializeInput();
        InitializeCamera();
        
        FrameTiming.UpdateLastTimingTick();
        
        GameSpecificInitialization();
    }

    private void InitializeInput()
    {
        Input = InputSystem.CreateInputSystem();
        MouseWheel.AddListenerTo(Window);
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
        Window.Clear();
        CurrentCamera?.Render(Root);
        Window.Display();
    }

    private void ProcessInput()
    {
        Window.DispatchEvents();
        
        MouseInput.UpdateInput(Window);
        Input.Update();
        
        Root.ProcessInputTree();
    }

    private void Update()
    {
        Root.UpdateTree();
        Input.ResolveCallbacks();
    }

    private void Timing()
    {
        FrameTiming.Timing();
    }
}