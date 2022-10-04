namespace Automata.Extensions.Common;

public static class NumberExtensions
{
    public static int ToInt(this double number)
    {
        if (number <= int.MinValue)
            return int.MinValue;
        
        if(number >= int.MaxValue)
            return int.MaxValue;

        return (int)number;
    }
}