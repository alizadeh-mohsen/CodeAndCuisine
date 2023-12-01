using CodeAndCuisine.Services.ShoppingCartAPI.Models.Dtos;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponByCode(string code);
    }
}
