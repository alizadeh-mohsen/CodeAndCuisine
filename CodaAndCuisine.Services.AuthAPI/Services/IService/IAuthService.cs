using CodaAndCuisine.Services.AuthAPI.Data.Dtos;
using CodeAndCuisine.Services.CouponAPI.Models.Dtos;

namespace CodaAndCuisine.Services.AuthAPI.Services.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> Register(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<ResponseDto> AssignRole(string username, string roleName);

    }
}
