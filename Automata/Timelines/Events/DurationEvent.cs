using Automata.Yielding;
using NodaTime;

namespace Automata.Timelines.Events;

public class DurationEvent : TimelineEvent
{
    private Duration Duration { get; }
    private Duration _elapsedTime;
    public DurationEvent(Func<Task<YieldStatement>> timelineAction, Duration duration) : base(timelineAction)
    {
        Duration = duration;
    }

    public override async Task Run()
    {
        using (await Timestamp.TokenLock())
        {
            Tick();
            var yield = await RunIfCondition();
            return;
        }
    }

    private void Tick()
    {
        _elapsedTime += Timeline.DeltaTime;
    }
    
    private async Task<YieldStatement?> RunIfCondition()
    {
        if (_elapsedTime >= Duration && !Recently())
            return await Invoke();
        return null;
    }
    
    private bool Recently()
    {
        // Правда, когда время с последнего обновления больше времени ожидания
        return Timeline.Instant() - Timestamp.Stamp <= Duration;
    }
}