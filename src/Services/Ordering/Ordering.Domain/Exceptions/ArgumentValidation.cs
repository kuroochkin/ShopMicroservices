namespace Ordering.Domain.Exceptions;

public static class ArgumentValidation
{
    public static void ThrowIfNegativeOrZero(int value, string paramName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.");
    }

    public static void ThrowIfNegativeOrZero(decimal value, string paramName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.");
    }
    
    public static void ThrowIfGreaterThan(int value, int maxValue, string paramName)
    {
        if (value > maxValue)
            throw new ArgumentOutOfRangeException(paramName, $"Value {value} cannot be greater than {maxValue}.");
    }
}