using NodaTime;
using NodaTime.Extensions;

namespace Automata.Timelines;

/// <summary>
/// Точка на временном промежутке, на часах.
/// </summary>
public readonly struct Timepoint
{
    private static readonly Period DefaultThreshold = Period.FromMinutes(30);
    
    /// <summary>
    /// Дополнительный промежуток времени для соответствия времени точки.
    /// </summary>
    public readonly Period Threshold;
    private readonly LocalTime _time;
    
    public Timepoint(LocalTime time)
    {
        _time = time;
        Threshold = DefaultThreshold;
    }

    public Timepoint(LocalTime time, Period threshold) : this(time)
    {
        Threshold = threshold;
    }

    /// <summary>
    /// Находится ли указанное время в пределах точки.
    /// </summary>
    public bool At(LocalTime timeToCheck)
    {
        if (_time <= timeToCheck && timeToCheck <= (_time + Threshold))
            return true;
        return false;
    }

    public bool At(IClock clock)
    {
        var localTime = clock.InBclSystemDefaultZone().GetCurrentLocalDateTime().TimeOfDay;
        return At(localTime);
    }
    
    /// <summary>
    /// Создает новую точку времени. Просто укороченная запись создания нового экземпляра.
    /// </summary>
    public static Timepoint New(int hour, int minute, Period period)
    {
        return new Timepoint(new LocalTime(hour, minute), period);
    }
}