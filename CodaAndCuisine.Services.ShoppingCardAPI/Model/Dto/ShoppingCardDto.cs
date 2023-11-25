namespace CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto
{
    public class ShoppingCartDto
    {
        public int CartId { get; set; }
        public CartHeader CartHeader { get; set; }
        public IList<CartDetail> CartDetails { get; set; }
    }
}


