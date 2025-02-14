using MyEngine.Nodes;
using SFML.Graphics;

namespace MyEngine;

// here instead of inheriting from Dictionary<string, SceneNode>, dictionary could be used directly with extension methods,
// but I decided that just making it a new class would be simpler, cleaner and makes it
public class SceneManager : Dictionary<string, SceneNode>
{
    public void Render()
    {
        foreach (SceneNode scene in Values)
        {
            if (scene.IsRendered)
                scene.RenderScene();
        }
    }
    
    public void Update(FrameTiming timing)
    {
        foreach (SceneNode scene in Values)
        {
            if(scene.IsProcessed) 
                scene.UpdateScene(timing);
        }
    }
    
    public void ProcessInput()
    {
        foreach (SceneNode scene in Values)
        {
            if (scene.IsProcessed)
                scene.ProcessInput();
        }
    }
}