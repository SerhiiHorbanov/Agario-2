using SFML.Graphics;
using MyEngine.Nodes;

namespace MyEngine;

public abstract class Game
{
    protected Node Root;
    protected RenderWindow Window;

    protected Camera? CurrentCamera;
    
    public void Run()
    {
        Initialization();
        
        while (ContinueGame())
        {
            Render();
            Input();
            Update();
        }
    }

    private void Initialization()
    {
        Root = Node.CreateNode();
        
        Window = new RenderWindow(new(900, 900), "Window");
        Window.Closed += (sender, args) => Window.Close();
        
        CurrentCamera = Camera.CreateCamera(Window);
        Root.AdoptChild(CurrentCamera);
        
        GameSpecificInitialization();
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
        Root.ProcessInputTree();
    }

    private void Update() 
        => Root.UpdateTree();
}