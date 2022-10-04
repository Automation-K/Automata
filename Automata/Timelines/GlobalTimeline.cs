using Automata.Extensions.Common;
using Microsoft.Extensions.Logging;

namespace Automata.Timelines;

/// <summary>
/// Статический менеджер таймлайнов. Можно использовать для сохранения объекта таймлайна в памяти,
/// чтобы сборщик мусора его не удалил. При добавлении сразу запускает таймлайн.
/// </summary>
public static class GlobalTimeline
{
    public static List<Timeline> Timelines { get; private set; }

    static GlobalTimeline()
    {
        Timelines = new List<Timeline>();
    }

    /// <summary>
    /// Добавляет таймлайн и сразу запускает его.
    /// </summary>
    public static void Add(Timeline timeline, ILogger logger)
    {
        Timelines.Add(timeline);
        Task
            .Run(timeline.Infinity)
            .HandleExceptions(logger, $"GlobalTimeline {timeline.Name}");
    }
}