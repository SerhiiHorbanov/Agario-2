using System.Collections;
using MyEngine.Nodes.Graphics;
using MyEngine.Utils;
using SFML.Graphics;

namespace MyEngine.Nodes;

public class Node : IEnumerable<Node>
{
    private List<Node> _children;
    public Node Parent;

    private bool _toOrphan;
    
    private bool IsRoot
        => Parent == this;

    protected Node()
    {
        _toOrphan = false;
        _children = new();
        Parent = this;
    }

    public static Node CreateNode()
        => new Node();

    public Node CreateChildNode()
        => AdoptChild(CreateNode());
    
    public bool HasChild(Node child)
        => _children.Contains(child);

    public T? GetChildOfType<T>() where T : Node
    {
        foreach (Node child in _children)
        {
            if (child is T result)
                return result;
        }

        return null;
    }

    public List<T> GetChildrenOfType<T>() where T : Node
    {
        List<T> result = new();
        
        foreach (Node child in _children)
        {
            if (child is T node)
                result.Add(node);
        }

        return result;
    }
    
    public T? GetSiblingOfType<T>() where T : Node
        => Parent.GetChildOfType<T>();    
    
    public void DetachChild(Node child)
    {
        if (child == this)
            return;
        
        child.Parent = child;
        _children.Remove(child);
    }

    public void Orphan()
        => _toOrphan = true;
    
    public Node GetRootNode()
    {
        Node current = this;
        
        while (!current.IsRoot)
            current = current.Parent;

        return current;
    }
    
    public Node AdoptChild(Node child)
    {
        child.Parent.DetachChild(child);
        child.Parent = this;
        child._toOrphan = false;
        _children.Add(child);
        
        return child;
    }
    
    public void UpdateTree(FrameTiming timing)
    {
        if (!IsRoot)
        {
            GetRootNode().UpdateTree(timing);
            return;
        }

        UpdateInfo info = GetUpdateInfo(timing);
        
        Queue<Node> updateQueue = new();
        updateQueue.Enqueue(this);

        while (updateQueue.Any())
        {
            Node updating = updateQueue.Dequeue();

            if (updating._toOrphan)
            {
                updating.Parent.DetachChild(updating);
                continue;
            }
            
            updateQueue.Enqueue(updating._children);
            updating.Update(info);
        }
    }
    
    private UpdateInfo GetUpdateInfo(FrameTiming timing)
        => new(timing, this);
    
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
        
        foreach (Node child in _children)
            child.AddThisAndChildrenToDrawableQueue(queue);
    }
    
    public void ProcessInputTree()
    {
        ProcessInput();
        foreach (Node child in _children)
            child.ProcessInputTree();
    }
    
    protected virtual void ProcessInput()
    { }
    
    protected virtual void Update(in UpdateInfo info)
    { }

    public IEnumerator<Node> GetEnumerator()
        => _children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _children.GetEnumerator();
}