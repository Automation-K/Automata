namespace Automata.Yielding;

/// <summary>
/// Данный тип нужен для передачи данных из итератора внешнему получателю. Экземпляр объекта нужно передать аргументом в
/// метод итератора и ожидать завершения задачи, доступной для чтения внутри этого объекта. 
/// </summary>
public class YieldData<T>
{
    private T? _value;

    public T? Value
    {
        private get { return _value; }
        set
        {
            _value = value;
            Source.SetResult(_value);
        }
    }

    private TaskCompletionSource<T?> Source { get; }

    public Task<T?> Task => Source.Task;

    public YieldData()
    {
        Source = new TaskCompletionSource<T?>();
    }
}