using System.ComponentModel.DataAnnotations;

namespace CodeAndCuisine.Services.CouponAPI.Models.Dtos
{
    public class RegisterRequestDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
