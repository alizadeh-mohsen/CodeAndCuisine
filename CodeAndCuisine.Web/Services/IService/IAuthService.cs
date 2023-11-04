using CodeAndCuisine.Web.Models;

namespace CodeAndCuisine.Web.Services.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<ResponseDto> AssignRoleAsync(RegisterRequestDto registerRequestDto);
    }
}
