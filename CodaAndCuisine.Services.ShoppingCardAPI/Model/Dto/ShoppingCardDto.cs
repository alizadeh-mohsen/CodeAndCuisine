namespace CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto
{
    public class ShoppingCartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IList<CartDetailDto>? CartDetails { get; set; }
    }
}


