using Automata.Extensions.Common;
using Automata.Timelines.Events;
using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.Extensions;

namespace Automata.Timelines;

/// <summary>
/// С помощью таймлайна можно совершать действия на временном промежутке.
/// </summary>
public class Timeline
{
    /// <summary>
    /// Частота обновления. Число миллисекунд перед следующей итерацией.
    /// </summary>
    public Duration Tick { get; set; } = Duration.FromMilliseconds(150);

    public string Name { get; }
    public ILogger Logger { get; }
    public IClock Clock { get; }
    
    /// <summary>
    /// События на таймлайне. Иначе говоря чекпоинты на временном промежутке.
    /// </summary>
    private List<TimelineEvent> Events { get; }
    
    /// <summary>
    /// Время прошедшее с начала прошлой итерации.
    /// </summary>
    public Duration DeltaTime { get; private set; }

    public Timeline(string name, ILogger logger, IClock clock)
    {
        Name = name;
        Logger = logger;
        Clock = clock;

        Events = new List<TimelineEvent>();
    }

    /// <summary>
    /// Запуск таймлайна. Создает бесконечную задачу, которая никогда не завершится.
    /// </summary>
    public async Task Infinity()
    {
        try
        {
            DeltaTime = Duration.Zero;
            while (true)
            {
                var startTime = Instant();

                await RunEvents(DeltaTime);

                await Task.Delay(Tick.TotalMilliseconds.ToInt());
                DeltaTime = Instant() - startTime;
            }
        }
        catch (Exception ex)
        {
            Logger.LogCritical("Ошибка в таймлайне [{Name}]: {Exception}",
                Name, ex.Full());
        }
    }

    /// <summary>
    /// Запуск всех условий. Запускает условия и ожидает завершения всех их.
    /// </summary>
    private async Task RunEvents(Duration deltaTime)
    {
        var tasks = new List<Task>(Events.Count);
        for (var i = 0; i < Events.Count; i++)
        {
            var condition = Events[i];
            if (!condition.Enabled)
                continue;

            var task = Task
                .Run(condition.Run)
                .HandleExceptions(Logger, "timeline event task");
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }

    public void Add(TimelineEvent te)
    {
        Add(te,true);
    }

    public void Add(TimelineEvent te, bool unite)
    {
        te.Timeline = this;
        Events.Add(te);
        if (unite)
        {
            if (Events.Count > 1)
                Events[0].Unite(te);
        }
    }

    /// <summary>
    /// Добавляет условия. При добавлении коллекции условий, они будут иметь общий временной маркер/stamp.
    /// </summary>
    public void Add(params TimelineEvent[] timeConditions)
    {
        var first = timeConditions[0];

        foreach (var condition in timeConditions)
        {
            condition.Timeline = this;
            first.Unite(condition);
            Events.Add(condition);
        }
    }

    /// <summary>
    /// Текущее время, используемое таймлайном. 
    /// </summary>
    public Instant Instant()
    {
        return Clock.GetSystemInstant();
    }

    /// <summary>
    /// Локальное время, используемое таймлайном.
    /// </summary>
    public LocalTime LocalTime()
    {
        return Clock.InBclSystemDefaultZone().GetCurrentTimeOfDay();
    }
}