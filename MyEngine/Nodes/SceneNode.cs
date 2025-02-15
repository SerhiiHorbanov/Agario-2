using MyEngine.Nodes.Graphics;
using MyEngine.Utils;
using SFML.Graphics;

namespace MyEngine.Nodes;

public sealed class SceneNode : Node
{
    private readonly List<Camera> _activeCameras;

    private readonly List<IUpdatable> _updatables;
    private readonly List<IProcessesInput> _inputProcessors;
    private readonly List<RenderedNode> _renderedNodes;

    public bool IsRendered;
    public bool IsProcessed;
    
    private SceneNode()
    {
        IsProcessed = true;
        IsRendered = true;
        
        _activeCameras = new();
        _updatables = new();
        _inputProcessors = new();
        _renderedNodes = new();
    }
    
    public static SceneNode CreateNewScene()
        => new();

    public static SceneNode CreateNewSceneWithCamera(RenderTarget target)
    {
        SceneNode scene = new();
        Camera camera = Camera.CreateCamera(target);
        
        scene.AdoptChild(camera);
        
        return scene;
    }
    
    public void RegisterNodeAndChildren(Node node)
    {
        EnsureCapacityForNode(node);
        
        if (node is Camera camera)
            _activeCameras.Add(camera);
        
        _updatables.TryAdd(node);
        _inputProcessors.TryAdd(node);
        _renderedNodes.TryAdd(node);
        node.IsInScene = true;
        
        foreach (Node child in node) 
            RegisterNodeAndChildren(child);
    }

    private void EnsureCapacityForNode(Node node)
    {
        int amount = node.CountDescendants() + 1;
        
        _activeCameras.EnsureCapacity(_activeCameras.Count + amount);
        _inputProcessors.EnsureCapacity(_inputProcessors.Count + amount);
        _renderedNodes.EnsureCapacity(_renderedNodes.Count + amount);
    }

    public void UnregisterNodeAndChildren(Node node)
    {
        if (node is Camera camera)
            _activeCameras.Remove(camera);
        
        _updatables.TrySwapRemove(node);
        _inputProcessors.TrySwapRemove(node);
        _renderedNodes.TrySwapRemove(node);
        node.IsInScene = false;

        foreach (Node child in node)
            UnregisterNodeAndChildren(child);
    }
    
    public void UpdateScene(FrameTiming timing)
    {
        UpdateInfo info = GetUpdateInfo(timing);

        foreach (IUpdatable each in _updatables)
            each.Update(info);

        KillNodesToKill();
    }

    public void RenderScene()
    {
        foreach (Camera eachCamera in _activeCameras)
            eachCamera.Render(this);
    }

    public void ProcessInput()
    {
        foreach (IProcessesInput each in _inputProcessors)
            each.ProcessInput();
    }
    
    private UpdateInfo GetUpdateInfo(FrameTiming timing)
        => new(timing, this);
}