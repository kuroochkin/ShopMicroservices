using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryHandler
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session;

    public GetProductByCategoryHandler(IDocumentSession session) 
        => _session = session;

    public async Task<GetProductByCategoryResult> Handle(
        GetProductByCategoryQuery query, 
        CancellationToken cancellationToken)
    {
        var products = await _session.Query<Product>()
            .Where(a => a.Categories.Contains(query.Category))
            .ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}