using CodeAndCuisine.Services.ShoppingCartAPI.Model.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Model
{
    public class CartDetail
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto Product { get; set; }


    }
}
