using Automata.Yielding;
using NodaTime;

namespace Automata.Timelines.Events;

public class ElapsedEvent : TimelineEvent
{
    private Duration Time { get; }

    private bool _already;
    private Duration _elapsedTime;
    
    public ElapsedEvent(Func<Task<YieldStatement>> timelineAction, Duration time) : base(timelineAction)
    {
        Time = time;
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
        if (_elapsedTime >= Time && !Recently())
        {
            _already = true;
            return await Invoke();
        }
        // Выключаем условие, чтобы оно больше не обновлялось.
        if (_already) Enabled = false;
        return null;
    }
    
    private bool Recently()
    {
        // Правда, когда время с последнего обновления больше времени ожидания
        return Timeline.Instant() - Timestamp.Stamp <= Time;
    }
}