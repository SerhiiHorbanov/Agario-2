using MyEngine.Timed;

namespace MyEngine.Nodes;

public sealed class EventSequenceNode : Node, IUpdatable
{
    public EventSequence Sequence;

    public EventSequenceNode(EventSequence sequence)
        => Sequence = sequence;

    public static EventSequenceNode CreateEventSequenceNode()
        => new EventSequenceNode(new());
    
    public static EventSequenceNode CreateEventSequenceNode(List<TimedEvent> timedEvents)
        => new EventSequenceNode(new(timedEvents));
    
    public void Update(in UpdateInfo info)
    {
        Sequence.Update();
    }
}