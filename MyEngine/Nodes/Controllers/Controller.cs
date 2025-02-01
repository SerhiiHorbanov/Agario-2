namespace MyEngine.Nodes;

public abstract class Controller<T> : Node where T : Node
{
    private T _controlled;

    protected T Controlled
    {
        get => _controlled;
        set => SetControlled(value);
    }

    protected virtual void SetControlled(T newControlled)
        => _controlled = newControlled;
}