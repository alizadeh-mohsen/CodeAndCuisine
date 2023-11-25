using CodeAndCuisine.Services.ShoppingCartAPI.Model.Dto;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto
{
    public class CartDetailDto
    {
        public int Id { get; set; }

        public int CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }

        public int ProductId { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}


