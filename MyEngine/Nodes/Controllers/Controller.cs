namespace MyEngine.Nodes.Controllers;

public abstract class Controller<T> : Node, IUpdatable where T : Node
{
    private WeakReference<T> _controlled;

    protected T Controlled
    {
        get
        {
            _controlled.TryGetTarget(out T result);
            return result;
        }
        set => SetControlled(value);
    }

    public virtual void Update(in UpdateInfo info)
        => EnsureControlledIsNotKilled();

    private void EnsureControlledIsNotKilled()
    {
        if (Controlled?.IsKilled ?? false)
            Controlled = null;
    }

    protected virtual void SetControlled(T newControlled)
        => _controlled = new WeakReference<T>(newControlled);
}