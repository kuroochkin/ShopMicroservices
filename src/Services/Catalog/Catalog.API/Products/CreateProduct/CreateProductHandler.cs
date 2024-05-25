using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductHandler
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IDocumentSession _session;
    
    public CreateProductHandler(IDocumentSession session) => _session = session;
    
    public async Task<CreateProductResult> Handle(
        CreateProductCommand command, 
        CancellationToken cancellationToken)
    {
        //Business
        var product = new Product
        {
            Name = command.Name,
            Categories = command.Categories,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
        
        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}