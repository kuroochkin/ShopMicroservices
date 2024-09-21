using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.Exceptions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models;

public class Product : Entity<ProductId>
{
    public string Name { get; private set; } = default!;

    public decimal Price { get; private set; } = default!;
    
    public static Product Create(ProductId id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentValidation.ThrowIfNegativeOrZero(price, nameof(price));
        
        var product = new Product
        {
            Id = id,
            Name = name,
            Price = price
        };

        return product;
    }
}