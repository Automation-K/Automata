using Automata.Extensions.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NodaTime;

namespace Automata.Timelines;

public static class SharedTimeline
{
    public static readonly Timeline Timeline;
    private static Task _timelineInfinity;
    static SharedTimeline()
    {
        Timeline = new Timeline("shared", NullLogger.Instance, SystemClock.Instance);
        _timelineInfinity = Timeline.Infinity();
    }
}