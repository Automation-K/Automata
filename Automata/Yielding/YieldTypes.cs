namespace Automata.Yielding;

/// <summary>
/// Выражение возвращаемое при вызове итератора.
/// </summary>
public abstract record YieldStatement;

public abstract record YieldSuccess : YieldStatement;
/// <summary>
/// Выражение обозначает, что текущая итерация прошла успешно.
/// </summary>
public record Ok : YieldSuccess;
/// <summary>
/// Выражение обозначает, что нужно перейти на следущий этап, подождав перед этим некоторое время.
/// </summary>
/// <param name="DelayMs">Количество миллисекунд перед итерацией.</param>
public record Next(int DelayMs) : YieldSuccess;
/// <summary>
/// Выражение обозначает, что нужно использовать другой итератор.
/// </summary>
public record Branch(Func<IAsyncEnumerable<YieldStatement>> Function) : YieldSuccess;

public abstract record YieldError : YieldStatement;
/// <summary>
/// Выражение обозначет, что итерация не прошла успешно.
/// </summary>
public record Fail : YieldError
{
    public object Data { get; }

    public Fail(object? data = null)
    {
        Data = data ?? new NullReferenceException();
    }
}
/// <summary>
/// Выражение обозначает, что итерация завершилась с ошибкой.
/// </summary>
public record Abort(Exception Exception) : YieldError;