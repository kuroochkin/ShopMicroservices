using System;
using System.ComponentModel;
using Ordering.Domain.Enums;
using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string? CardName { get; } = default!;

    public string CardNumber { get; } = default!;

    public string Expiration { get; } = default!;

    public string CVV { get; } = default!;

    public int PaymentMethod { get; } = default!;

    protected Payment()
    {
    }

    private Payment(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrEmpty(cardName);
        ArgumentException.ThrowIfNullOrEmpty(cardNumber);
        ArgumentException.ThrowIfNullOrEmpty(cvv);
        ArgumentValidation.ThrowIfGreaterThan(cvv.Length, 3, nameof(cvv));

        return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
    }
}