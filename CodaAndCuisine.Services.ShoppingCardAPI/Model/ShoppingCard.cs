using System.ComponentModel.DataAnnotations;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Model
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public CartHeader CartHeader { get; set; }
        public IList<CartDetail> CartDetails{ get; set; }

    }
}
