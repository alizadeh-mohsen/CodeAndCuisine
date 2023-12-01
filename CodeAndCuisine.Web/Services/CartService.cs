using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using CodeAndCuisine.Web.Utility;

namespace CodeAndCuisine.Web.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> ApplyCoupon(ShoppingCartDto shoppingCartDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.CartApiBase + "/ApplyCoupon",
                Data = shoppingCartDto
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> CardUpsert(ShoppingCartDto shoppingCartDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.CartApiBase + "/Upsert",
                Data = shoppingCartDto
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> GetShoppingCart(string id)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.CartApiBase + "/GetCart/" + id,
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> RemoveItem(int detailId)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.CartApiBase + "/Remove",
                Data = detailId

            };

            return await _baseService.SendAsync(requestDto);
        }

    }
}
