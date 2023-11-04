namespace CodaAndCuisine.Services.AuthAPI.Data.Dtos
{
    public class LoginResponseDto
    {
        public UserDto UserDto{ get; set; }
        public string Token { get; set; }
    }
}
