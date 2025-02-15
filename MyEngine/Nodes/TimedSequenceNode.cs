using MyEngine.Timed;

namespace MyEngine.Nodes;

public sealed class TimedSequenceNode<T> : Node, IUpdatable
{
    public TimedSequence<T> Sequence;

    private TimedSequenceNode(TimedSequence<T> sequence)
        => Sequence = sequence;

    public static TimedSequenceNode<T> CreateEventSequenceNode()
        => new (new());
    public static TimedSequenceNode<T> CreateEventSequenceNode(TimedSequence<T> sequence)
        => new(sequence);
    
    public void Update(in UpdateInfo info)
    {
        Sequence.Update();
    }
}