using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerHandler
    : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    private readonly IApplicationDbContext _dbContext;

    public GetOrdersByCustomerHandler(IApplicationDbContext dbContext) 
        => _dbContext = dbContext;
    
    public async Task<GetOrdersByCustomerResult> Handle(
        GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        // get orders by customer using dbContext
        // return result

        var orders = await _dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);

        return new GetOrdersByCustomerResult(orders.ToOrderDtoList());        
    }
}