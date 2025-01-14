using SFML.Graphics;

namespace MyEngine.Nodes;

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

    public T? GetChildOfType<T>() where T : Node
    {
        foreach (Node child in Children)
        {
            if (child is T result)
                return result;
        }

        return null;
    }

    public T? GetSiblingOfType<T>() where T : Node
        => Parent.GetChildOfType<T>();    
    
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
    
    public Queue<Drawable> GetRenderQueue(Camera camera)
    {
        Queue<Drawable> drawables = new();
        AddThisAndChildrenToDrawableQueue(drawables);

        return drawables;
    }

    private void AddThisAndChildrenToDrawableQueue(Queue<Drawable> queue)
    {
        if (this is Drawable drawable)
            queue.Enqueue(drawable);
        
        foreach (Node child in Children)
            child.AddThisAndChildrenToDrawableQueue(queue);
    }
    
    public void ProcessInputTree()
    {
        ProcessInput();
        foreach (Node child in Children)
            child.ProcessInputTree();
    }
    
    protected virtual void ProcessInput()
    { }
    
    protected virtual void Update()
    { }
}