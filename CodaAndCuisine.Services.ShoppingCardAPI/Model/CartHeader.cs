using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Model
{
    public class CartHeader
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        
        [NotMapped]
        public int Discount { get; set; }

        [NotMapped]
        public int CartToal { get; set; }

    }
}
