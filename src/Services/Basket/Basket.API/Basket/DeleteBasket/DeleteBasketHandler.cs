using Basket.API.Data;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class DeleteBasketCommandHandler
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository)
        => _basketRepository = basketRepository;

    public async Task<DeleteBasketResult> Handle(
        DeleteBasketCommand command, 
        CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(command.UserName, cancellationToken);
        
        return new DeleteBasketResult(true);
    }
}