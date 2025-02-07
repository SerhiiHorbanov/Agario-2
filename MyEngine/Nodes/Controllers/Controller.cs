namespace MyEngine.Nodes.Controllers;

public abstract class Controller<T> : Node where T : Node
{
    private T _controlled;

    protected T Controlled
    {
        get => _controlled;
        set => SetControlled(value);
    }

    protected override void Update(in UpdateInfo info)
        => EnsureControlledIsNotKilled();

    private void EnsureControlledIsNotKilled()
    {
        if (Controlled?.IsKilled ?? false)
            Controlled = null;
    }

    protected virtual void SetControlled(T newControlled)
        => _controlled = newControlled;
}