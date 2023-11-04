using CodaAndCuisine.Services.AuthAPI.Data.Dtos;
using CodaAndCuisine.Services.AuthAPI.Services.IService;
using CodeAndCuisine.Services.CouponAPI.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CodaAndCuisine.Services.AuthAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var userDto = await _authService.Register(model);
            if (userDto == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not created";
                return BadRequest(_responseDto);
            }

            _responseDto.IsSuccess = true;
            _responseDto.Result = userDto;
            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            LoginResponseDto loginResponseDto = await _authService.Login(model);
            if (loginResponseDto.UserDto == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password is incorrect";
                return BadRequest(_responseDto);
            }
            _responseDto.IsSuccess = true;
            _responseDto.Result = loginResponseDto;
            return Ok(_responseDto);
        }
    }
}
