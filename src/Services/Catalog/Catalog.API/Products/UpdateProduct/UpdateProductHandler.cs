using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Id is required");
        
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductHandler
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(
        IDocumentSession session,
        ILogger<UpdateProductHandler> logger)
    {
        _session = session;
        _logger = logger;
    }
    
    public async Task<UpdateProductResult> Handle(
        UpdateProductCommand command, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("UpdateProductHandler.Handle called with {@Command}", command);
        
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);
        
        if (product is null)
            throw new ProductNotFoundException(command.Id);

        product.Name = command.Name;
        product.Categories = command.Categories;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        
        _session.Update(product);
        await _session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}