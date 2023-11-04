using CodaAndCuisine.Services.AuthAPI.Data.Dtos;
using CodeAndCuisine.Services.CouponAPI.Models.Dtos;

namespace CodaAndCuisine.Services.AuthAPI.Services.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string username, string roleName);

    }
}
