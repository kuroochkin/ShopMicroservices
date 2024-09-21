using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;

    public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
            throw new DomainException("Id заказчика не может быть пустым");

        return new CustomerId(value);
    }
}