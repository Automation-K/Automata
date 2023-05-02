namespace Automata.Extensions.Collections;

public static class LinkedListExtensions
{
    /// <summary>
    /// Возвращает первый найденный объект или ничего. Удобно для использования со структурами, т.к. структуры не
    /// могут быть null.
    /// </summary>
    public static LinkedListNode<T>? FirstNode<T>(this LinkedList<T> source, Func<T, bool> predicate)
    {
        if (source == null)
            throw new ArgumentNullException(nameof (source));
        if (predicate == null)
            throw new ArgumentNullException(nameof (predicate));

        var node = source.First;
        if(node == null)
            return null;

        while (node != null)
        {
            if (predicate(node.Value))
                return node;

            node = node.Next;
        }
            
        return null;
    }
    
    /// <summary>
    /// Присоединяет другой список в конец текущего
    /// </summary>
    public static LinkedList<T> Append<T>(this LinkedList<T> source, LinkedList<T> another)
    {
        foreach (var value in another)
        {
            source.AddLast(value);
        }

        return source;
    }
}