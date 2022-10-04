using Microsoft.Extensions.Logging;

namespace Automata.Extensions.Common;

public static class TaskExtensions
{
    public static Task HandleExceptions(this Task task, ILogger logger, string name = "")
    {
        task.ContinueWith((t) =>
        {
            // ReSharper disable once ConvertTypeCheckPatternToNullCheck
            if (task.Exception is not AggregateException agg) return;
        
            foreach (var e in agg.InnerExceptions) 
                logger.LogError("Ошибка в Task, {Name}: {Exception}",
                    name ,e.Full());
        }, TaskContinuationOptions.NotOnRanToCompletion);
        return task;
    }
    
    public static void HandleExceptions(this Task task, Action<IReadOnlyCollection<Exception>> handler)
    {
        task.ContinueWith((t) =>
        {
            // ReSharper disable once ConvertTypeCheckPatternToNullCheck
            if (task.Exception is not AggregateException agg) return;
            handler?.Invoke(agg.InnerExceptions);
        },TaskContinuationOptions.NotOnRanToCompletion);
    }
}