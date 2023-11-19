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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var responseDto = await _authService.Register(model);
            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var responseDto = new ResponseDto();
            LoginResponseDto loginResponseDto = await _authService.Login(model);
            if (loginResponseDto.UserDto == null)
            {

                responseDto.IsSuccess = false;
                responseDto.Message = "Username or password is incorrect";
                return BadRequest(responseDto);
            }
            responseDto.IsSuccess = true;
            responseDto.Result = loginResponseDto;
            return Ok(responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterRequestDto model)
        {
            var responseDto = await _authService.AssignRole(model.Username, model.Role.ToUpper());

            return Ok(responseDto);
        }
    }
}
