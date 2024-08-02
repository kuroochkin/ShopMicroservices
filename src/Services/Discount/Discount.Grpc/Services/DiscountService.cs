using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly DiscountContext _dbContext;

    private readonly ILogger<DiscountService> _logger;
    
    public DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon == null)
            throw new RpcException(new Status(
                StatusCode.NotFound, 
                $"Купон для {request.ProductName} не найден!"));
        
        _logger.LogInformation($"Обнаружен купон для {request.ProductName} на {coupon.Amount} монет!");

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        
        if (coupon is null)
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                "Не удалось создать купон, неправильные параметры"));

        _dbContext.Coupons.Add(coupon);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Купон на {coupon.ProductName} успешно создан!");

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        
        if (coupon is null)
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                "Не удалось создать объект купона, неправильные параметры"));

        _dbContext.Coupons.Update(coupon);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Купон на {coupon.ProductName} успешно обновлен!");

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        
        if (coupon == null)
            throw new RpcException(new Status(
                StatusCode.NotFound, 
                $"Купон для {request.ProductName} не найден!"));

        _dbContext.Coupons.Remove(coupon);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Купон на {coupon.ProductName} успешно удален!");

        return new DeleteDiscountResponse { Success = true };
    }
}