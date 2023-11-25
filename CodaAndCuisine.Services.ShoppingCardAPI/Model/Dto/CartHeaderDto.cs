namespace CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto
{
    public class CartHeaderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public int Discount { get; set; }
        public int CartToal { get; set; }
    }
}


