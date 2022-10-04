using Automata.Yielding;

namespace Automata.Timelines.Events;

public class TimepointEvent : TimelineEvent
{
    public Timepoint Timepoint { get; }
    
    public TimepointEvent(Func<Task<YieldStatement>> timelineAction, Timepoint timepoint) : base(timelineAction)
    {
        Timepoint = timepoint;
    }

    public override async Task Run()
    {
        if(Recently() || !AtPoint())
            return;

        var yield = await Invoke();
        return;
    }

    private bool AtPoint()
    {
        return Timepoint.At(Timeline.LocalTime());
    }
    
    private bool Recently()
    {
        if (Timestamp.Stamp.ToUnixTimeTicks() == 0)
            return false;

        return Timeline.Instant() - Timestamp.Stamp <= Timepoint.Threshold.ToDuration();
    }
}