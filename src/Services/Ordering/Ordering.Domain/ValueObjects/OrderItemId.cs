using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects;

public class OrderItemId
{
    public Guid Value { get; }
    
    private OrderItemId(Guid value) => Value = value;

    public static OrderItemId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
            throw new DomainException("Id элемента заказа не может быть пустым!");

        return new OrderItemId(value);
    }
}