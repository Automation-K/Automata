using Automata.Timelines.Events;
using Automata.Yielding;
using NodaTime;

namespace Automata.Timelines;

public static class TimelineExtensions
{
    public static TimelineEvent OnDuration(this Timeline timeline, Func<Task<YieldStatement>> action, Duration duration,
        bool unite = true)
    {
        var durationEvent = new DurationEvent(action, duration);
        timeline.Add(durationEvent, unite);
        return durationEvent;
    }

    public static TimelineEvent OnElapsed(this Timeline timeline, Func<Task<YieldStatement>> action,Duration duration,
        bool unite = true)
    {
        var elapsedEvent = new ElapsedEvent(action, duration);
        timeline.Add(elapsedEvent, unite);
        return elapsedEvent;
    }

    public static TimelineEvent OnTime(this Timeline timeline, Func<Task<YieldStatement>> action,
        int hour, int minute, Period threshold, bool unite = true)
    {
        var timepointEvent = new TimepointEvent(action, Timepoint.New(hour, minute, threshold));
        timeline.Add(timepointEvent, unite);
        return timepointEvent;
    }
}