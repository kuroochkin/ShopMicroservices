using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Catalog.API.Products.GetProductById;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryHandler
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<GetProductByCategoryHandler> _logger;

    public GetProductByCategoryHandler(
        IDocumentSession session,
        ILogger<GetProductByCategoryHandler> logger)
    {
        _session = session;
        _logger = logger;
    }
    
    public async Task<GetProductByCategoryResult> Handle(
        GetProductByCategoryQuery query, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetProductByCategoryHandler.Handle called with {@Query}", query);

        var products = await _session.Query<Product>()
            .Where(a => a.Categories.Contains(query.Category))
            .ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}