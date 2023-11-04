using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using CodeAndCuisine.Web.Utility;

namespace CodeAndCuisine.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.CouponApiBase,
                Data = couponDto
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> DeleteCouponCouponAsync(int id)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.DELETE,
                Url = StaticData.CouponApiBase + "/" + id

            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> GetAllCouponsAsync()
        {

            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.CouponApiBase
            };

            return await _baseService.SendAsync(requestDto);

        }

        public async Task<ResponseDto> GetCouponAsync(string code)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.CouponApiBase + "/GetByCode/" + code

            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {

            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.CouponApiBase + "/" + id,
                Data = id
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.PUT,
                Url = StaticData.CouponApiBase,
                Data = couponDto
            };

            return await _baseService.SendAsync(requestDto);
        }
    }
}
