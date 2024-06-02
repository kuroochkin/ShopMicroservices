using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string userName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBasketCommandHandler 
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    private readonly IBasketRepository _basketRepository;

    public StoreBasketCommandHandler(IBasketRepository basketRepository)
        => _basketRepository = basketRepository;

    public async Task<StoreBasketResult> Handle(
        StoreBasketCommand command, 
        CancellationToken cancellationToken)
    {
        await _basketRepository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
    }
}