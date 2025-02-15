namespace MyEngine.Timed;

public record struct TimedEvent(float TicksSinceStart, Action Action);

public sealed class EventSequence
{
    private SortedList<long, Action> _events;

    public Action OnFinished;
    
    private bool _isPlaying;
    private long _now;
    private long _tickOfStart;
    private long _tickOfNextCalled;
    private int _indexOfNextCalled;
    
    public EventSequence(List<TimedEvent> events)
    {
        foreach (TimedEvent timedEvent in events)
            AddEvent(timedEvent);
        _isPlaying = false;
        OnFinished = Stop;
    }

    public void AddEvent(TimedEvent timedEvent)
    {
        long timeSinceStart = (long)(timedEvent.TicksSinceStart * TimeSpan.TicksPerSecond);
        _events.Add(timeSinceStart, timedEvent.Action);
    }

    public void Play()
    {
        _isPlaying = true;
        _tickOfStart = _now;
    }
    
    public void Stop()
        => _isPlaying = false;

    public void Restart()
    {
        SetTime(0);
        Play();
    }
    
    public void SetTime(float t)
    {
        long ticks = (long)(t * TimeSpan.TicksPerSecond);
        _tickOfStart = _now - ticks;

        int currentFrameIndex = GetFrameIndexByTick(ticks);
        
        _tickOfNextCalled = _events.Keys[currentFrameIndex];
        UpdateTickOfNextCalled();
        
        OnTimeSet();
    }
    
    public void Update()
    {
        if (!_isPlaying)
            return;
        
        _now = DateTime.Now.Ticks;
        
        while (ShouldProceed())
            Proceed();
    }

    private void OnTimeSet()
    {
        
    }

    private int GetFrameIndexByTick(long ticks)
    {
        int i = 0;
        while (i < _events.Count)
        {
            if (_events.Keys[i] > ticks)
            {
                i--;
                break;
            }

            i++;
        }

        return ClampIndex(i);
    }

    private int ClampIndex(int i)
    {
        if (i < 0)
            return 0;
        if (i >= _events.Count)
            return _events.Count - 1;
        return i;
    }

    private bool ShouldProceed() 
        => _tickOfNextCalled < _now;

    private void Proceed()
    {
        InvokeCurrent();

        _indexOfNextCalled++;

        if (_indexOfNextCalled >= _events.Count)
        {
            Stop();
            OnFinished?.Invoke();
            return;
        }
        
        UpdateTickOfNextCalled(); 
    }

    private void UpdateTickOfNextCalled()
    {
        _tickOfNextCalled = _tickOfStart + _events.Keys[_indexOfNextCalled];
    }

    private void InvokeCurrent() 
        => _events.Values[_indexOfNextCalled]();
}