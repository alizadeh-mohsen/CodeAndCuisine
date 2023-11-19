using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using CodeAndCuisine.Web.Utility;

namespace CodeAndCuisine.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateProductAsync(ProductDto ProductDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.ProductApiBase,
                Data = ProductDto
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> DeleteProductProductAsync(int id)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.DELETE,
                Url = StaticData.ProductApiBase + "/" + id

            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> GetAllProductsAsync()
        {

            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.ProductApiBase
            };

            return await _baseService.SendAsync(requestDto);

        }

        public async Task<ResponseDto> GetProductAsync(string code)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.ProductApiBase + "/GetByCode/" + code

            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> GetProductByIdAsync(int id)
        {

            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.GET,
                Url = StaticData.ProductApiBase + "/" + id,
                Data = id
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto> UpdateProductAsync(ProductDto ProductDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.PUT,
                Url = StaticData.ProductApiBase + "/" + ProductDto.ProductId,
                Data = ProductDto
            };

            return await _baseService.SendAsync(requestDto);
        }
    }
}
