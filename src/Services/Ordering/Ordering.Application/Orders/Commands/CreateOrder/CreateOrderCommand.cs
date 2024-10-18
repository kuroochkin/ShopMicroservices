using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order)
    : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Название заказа - обязательно!");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("Id заказчика - обязательное поле!");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems не должен быть пустым!");
    }
}
