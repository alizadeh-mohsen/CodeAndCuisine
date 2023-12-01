using CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto;
using CodaAndCuisine.Services.ShoppingCartAPI.Service.IService;
using CodeAndCuisine.Services.ShoppingCartAPI.Model.Dto;
using Newtonsoft.Json;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {

                HttpClient client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync("");
                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                if (resp.IsSuccess)
                    return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(resp.Result.ToString());
                return new List<ProductDto>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            try
            {

                HttpClient client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync($"/api/Product/{id}");
                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                if (resp.IsSuccess)
                    return JsonConvert.DeserializeObject<ProductDto>(resp.Result.ToString());
                return new ProductDto();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
