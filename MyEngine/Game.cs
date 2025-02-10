using MyEngine.ConfigSystem;
using MyEngine.MyInput;
using SFML.Graphics;
using MyEngine.Nodes;
using MyEngine.Nodes.Graphics;
using MyEngine.SoundSystem;

namespace MyEngine;

public abstract class Game
{
    protected Node Root;
    protected Node UIRoot;
    protected RenderWindow Window;

    protected InputSystem Input;
    protected Camera CurrentCamera; 
    protected Camera UICamera; 
    protected FrameTiming Time;
    
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
        UIRoot = Node.CreateNode();
        
        InitializeWindow();
        InitializeInput();
        InitializeCamera();
        InitializeTiming();
        
        GameSpecificInitialization();
    }

    private void InitializeTiming()
    {
        Time = new();
        Time.UpdateLastTimingTick();
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
        UICamera = Camera.CreateUICamera(Window);
        UIRoot.AdoptChild(UICamera);
    }

    private void InitializeWindow()
    {
        WindowConfigs configs = ConfigLoader.LoadFromFile<WindowConfigs>("Configs/Window.cfg");
        
        Window = new (new(configs.Size.X, configs.Size.Y), configs.Name);
        Window.Closed += (sender, args) => Window.Close();
    }

    protected abstract void GameSpecificInitialization();
    
    private bool ContinueGame()
        => Window.IsOpen;

    private void Render()
    {
        Window.Clear();
        
        CurrentCamera?.Render(Root);
        UICamera?.Render(UIRoot);
        
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
        Root.UpdateTree(Time);
        Input.ResolveCallbacks();
        SoundManager.UpdatePlayingSounds();
    }

    private void Timing()
    {
        Time.Timing();
    }
}