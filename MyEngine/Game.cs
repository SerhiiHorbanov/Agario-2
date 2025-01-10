﻿using SFML.Graphics;

namespace MyEngine;

public abstract class Game
{
    protected Node Root;
    protected RenderWindow Window;
    
    public void Run()
    {
        GameSpecificInitialization();
        
        while (ContinueGame())
        {
            Render();
            Input();
            Update();
        }
    }

    private void Initialization()
    {
        Root = Node.CreateRootNode();
        Window = new RenderWindow(new(900, 900), "Window");
        
        GameSpecificInitialization();
    }
    
    protected abstract void GameSpecificInitialization();
    
    private bool ContinueGame()
        => Window.IsOpen;

    private void Render()
    {
        Root.RenderTree(Window);
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