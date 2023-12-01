using CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto;
using CodaAndCuisine.Services.ShoppingCartAPI.Service.IService;
using CodeAndCuisine.Services.ShoppingCartAPI.Models.Dtos;
using Newtonsoft.Json;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCouponByCode(string code)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("Coupon");
                var response = await client.GetAsync($"/api/Coupon/GetByCode/{code}");
                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                if (resp.IsSuccess)
                    return JsonConvert.DeserializeObject<CouponDto>(resp.Result.ToString());
                return new CouponDto();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}