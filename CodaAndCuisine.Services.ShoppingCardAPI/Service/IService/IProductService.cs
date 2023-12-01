using CodeAndCuisine.Services.ShoppingCartAPI.Model.Dto;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int id);
    }
}
