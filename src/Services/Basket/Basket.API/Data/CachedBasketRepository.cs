using System.Text.Json;
using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository : IBasketRepository
{
    private readonly IBasketRepository _basketRepository;
    private readonly IDistributedCache _cache;

    public CachedBasketRepository(
        IBasketRepository basketRepository,
        IDistributedCache cache)
    {
        _basketRepository = basketRepository;
        _cache = cache;
    }
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
    {
        var cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        
        var basket = await _basketRepository.GetBasket(userName, cancellationToken);

        await _cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
    {
        await _basketRepository.StoreBasket(basket, cancellationToken);

        await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(userName, cancellationToken);

        await _cache.RemoveAsync(userName, cancellationToken);

        return true;
    }
}