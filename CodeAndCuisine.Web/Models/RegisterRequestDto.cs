using System.ComponentModel.DataAnnotations;

namespace CodeAndCuisine.Web.Models
{
    public class RegisterRequestDto
    {
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
