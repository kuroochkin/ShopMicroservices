﻿using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
    }
}

internal class DeleteProductCommandHandler
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
        IDocumentSession session,
        ILogger<DeleteProductCommandHandler> logger)
    {
        _session = session;
        _logger = logger;
    }
    
    public async Task<DeleteProductResult> Handle(
        DeleteProductCommand command, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteProductHandler.Handle called with {@Command}", command);
        
        _session.Delete<Product>(command.Id);
        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}