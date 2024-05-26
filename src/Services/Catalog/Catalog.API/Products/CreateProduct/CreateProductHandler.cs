﻿using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}

internal class CreateProductHandler
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IDocumentSession _session;
    private readonly IValidator<CreateProductCommand> _validator;

    public CreateProductHandler(IDocumentSession session) =>
        _session = session;

    public async Task<CreateProductResult> Handle(
        CreateProductCommand command, 
        CancellationToken cancellationToken)
    {    
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