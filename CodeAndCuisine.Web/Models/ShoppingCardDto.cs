namespace CodeAndCuisine.Web.Models
{
    public class ShoppingCartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public List<CartDetailDto>? CartDetails { get; set; }
    }
}


