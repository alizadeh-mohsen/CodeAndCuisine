using CodeAndCuisine.Web.Models;

namespace CodeAndCuisine.Web.Services.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetShoppingCart(string userId);
        Task<ResponseDto> CardUpsert(ShoppingCartDto cartDto);
        Task<ResponseDto> RemoveItem(int itemId);
        Task<ResponseDto> ApplyCoupon(ShoppingCartDto cartDto);
        
    }
}
