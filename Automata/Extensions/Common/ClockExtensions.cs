using NodaTime;
using NodaTime.Extensions;

namespace Automata.Extensions.Common;

public static class ClockExtensions
{
    public static Instant GetSystemInstant(this IClock clock) => clock.InBclSystemDefaultZone().GetCurrentInstant();
}