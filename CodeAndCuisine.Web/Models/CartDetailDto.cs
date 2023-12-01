namespace CodeAndCuisine.Web.Models
{
    public class CartDetailDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
    }
}


