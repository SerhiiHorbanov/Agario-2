using SFML.Graphics;

namespace MyEngine;

public class Node
{
    protected List<Node> Children;
    protected Node Parent;
    
    public bool IsRoot
        => Parent == this;

    protected Node()
    {
        Children = new();
        Parent = this;
    }

    public static Node CreateNode()
        => new Node();

    public Node CreateChildNode()
        => AdoptChild(CreateNode());
    
    public bool HasChild(Node child)
        => Children.Contains(child);

    public T? GetDescendantOfType<T>() where T : Node
    {
        foreach (Node decendant in Children)
        {
            if (decendant is T result)
                return result;
        }

        return null;
    }
    
    public void DetachChild(Node child)
    {
        if (child == this)
            return;
        
        child.Parent = child;
        Children.Remove(child);
    }

    public void Orphan()
        => Parent.DetachChild(this);
    
    public Node AdoptChild(Node child)
    {
        child.Orphan();
        child.Parent = this;
        Children.Add(child);
        
        return child;
    }
    
    public void UpdateTree()
    {
        if (!IsRoot)
            return;
        
        Queue<Node> updateQueue = new();
        updateQueue.Enqueue(this);

        while (updateQueue.Any())
        {
            Node updating = updateQueue.Dequeue();

            updateQueue.Enqueue(updating.Children);
            updating.Update();
        }
    }
    
    public void RenderTree(RenderTarget target)
    {
        Render(target);
        foreach (Node child in Children)
            child.RenderTree(target);
    }
    
    public void ProcessInputTree()
    {
        ProcessInput();
        foreach (Node child in Children)
            child.ProcessInputTree();
    }
    
    protected virtual void Render(RenderTarget target)
    { }
    
    protected virtual void ProcessInput()
    { }
    
    protected virtual void Update()
    { }
}