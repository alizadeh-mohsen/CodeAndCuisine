using System.Collections.ObjectModel;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto
{
    public class ShoppingCartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public List<CartDetailDto>? CartDetails { get; set; }
    }
}


