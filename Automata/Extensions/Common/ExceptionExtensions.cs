namespace Automata.Extensions.Common;

public static class ExceptionExtensions
{
    public static string Full(this Exception ex)
    {
        return ex.Message + ex.StackTrace;
    }
}