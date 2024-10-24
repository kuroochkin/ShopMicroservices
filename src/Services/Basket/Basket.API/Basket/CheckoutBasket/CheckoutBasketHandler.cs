using Basket.API.Data;
using Basket.API.Dtos;
using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using Mapster;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) 
    : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator 
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketCommandHandler
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CheckoutBasketCommandHandler(
        IBasketRepository basketRepository, 
        IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
        _basketRepository = basketRepository;
    }
    
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository
            .GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
        
        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        await _basketRepository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}