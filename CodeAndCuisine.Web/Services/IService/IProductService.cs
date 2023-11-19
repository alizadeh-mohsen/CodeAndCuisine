using CodeAndCuisine.Web.Models;

namespace CodeAndCuisine.Web.Services.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProductsAsync();
        Task<ResponseDto> GetProductAsync(string code);
        Task<ResponseDto> GetProductByIdAsync(int id);
        Task<ResponseDto> CreateProductAsync(ProductDto ProductDto);
        Task<ResponseDto> UpdateProductAsync(ProductDto ProductDto);
        Task<ResponseDto> DeleteProductProductAsync(int id);
    }
}
