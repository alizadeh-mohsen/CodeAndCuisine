using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using CodeAndCuisine.Web.Utility;

namespace CodeAndCuisine.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.AuthApiBase + "/Login",
                Data = loginRequestDto
            };

            var result = await _baseService.SendAsync(requestDto, false);
            return result;
        }
        public async Task<ResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.AuthApiBase + "/Register",
                Data = registerRequestDto
            };

            return await _baseService.SendAsync(requestDto);
        }
        public async Task<ResponseDto> AssignRoleAsync(RegisterRequestDto registerRequestDto)
        {
            var requestDto = new RequestDto
            {
                ApiType = StaticData.ApiType.POST,
                Url = StaticData.AuthApiBase,
                Data = registerRequestDto
            };

            return await _baseService.SendAsync(requestDto, false);
        }

    }
}
