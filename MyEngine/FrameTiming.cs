namespace MyEngine;

public class FrameTiming 
{
    private const int TargetFps = 60; 
    private const long TicksBetweenFrames = TimeSpan.TicksPerSecond / TargetFps; 
    public const float TargetDeltaSeconds = 1.0f / TargetFps; 
     
    public float DeltaSeconds; 
    private long _lastTimingTick; 
 
    public void UpdateLastTimingTick() 
        => _lastTimingTick = DateTime.Now.Ticks; 
     
    public void Timing() 
    { 
        long ticksToSleep = TicksBetweenFrames - (DateTime.Now.Ticks - _lastTimingTick); 
 
        if (ticksToSleep > 0) 
            Thread.Sleep((int)(ticksToSleep / TimeSpan.TicksPerMillisecond)); 
        
        DeltaSeconds = (float)(DateTime.Now.Ticks - _lastTimingTick) / TimeSpan.TicksPerSecond; 
        UpdateLastTimingTick(); 
    } 
}