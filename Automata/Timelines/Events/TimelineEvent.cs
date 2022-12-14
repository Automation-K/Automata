using Automata.Yielding;
using NodaTime;

namespace Automata.Timelines.Events;

/// <summary>
/// Событие находящееся на таймлайне, срабатывающее в определенное время.
/// </summary>
public abstract class TimelineEvent
{
    public Timeline Timeline { get; set; }
    
    /// <summary>
    /// Активно ли событие таймлайна. Если нет, то оно не будет обрабатываться.
    /// </summary>
    public bool Enabled { get; set; }
    
    protected Func<Task<YieldStatement>> TimelineAction { get;}
    
    /// <summary>
    /// Маркер времени последнего вызова события.
    /// </summary>
    protected Timestamp Timestamp { get; private set; }
    
    protected TimelineEvent(Func<Task<YieldStatement>> timelineAction)
    {
        TimelineAction = timelineAction;
        Enabled = true;
        Timestamp = new Timestamp();
    }

    public abstract Task Run();

    /// <summary>
    /// Устанавливает ссылку на маркер текущего события.
    /// </summary>
    public void Unite(TimelineEvent timelineEvent)
    {
        timelineEvent.Timestamp = Timestamp;
    }

    /// <summary>
    /// Вызов действия текущего события.
    /// </summary>
    protected async Task<YieldStatement> Invoke()
    {
        Timestamp.Stamp = Timeline.Instant();
        return await TimelineAction.Invoke();
    }
}

/// <summary>
/// Маркер времени.
/// </summary>
public class Timestamp
{
    private readonly SemaphoreSlim _semaphoreSlim;
    
    public Instant Stamp { get; set; }

    public Timestamp()
    {
        _semaphoreSlim = new SemaphoreSlim(1);
    }

    public async Task<SemaphoreToken> TokenLock()
    {
        await _semaphoreSlim.WaitAsync();
        return new SemaphoreToken(_semaphoreSlim);
    }
}

public class SemaphoreToken : IDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim;

    public SemaphoreToken(SemaphoreSlim semaphoreSlim)
    {
        _semaphoreSlim = semaphoreSlim;
    }

    public void Dispose()
    {
        _semaphoreSlim.Release();
    }
}