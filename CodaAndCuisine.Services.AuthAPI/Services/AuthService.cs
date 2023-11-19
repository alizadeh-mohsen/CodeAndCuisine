using CodaAndCuisine.Services.AuthAPI.Data;
using CodaAndCuisine.Services.AuthAPI.Data.Dtos;
using CodaAndCuisine.Services.AuthAPI.Models;
using CodaAndCuisine.Services.AuthAPI.Services.IService;
using CodeAndCuisine.Services.CouponAPI.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace CodaAndCuisine.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _authDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AuthDbContext authDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            , IJwtTokenGenerator jwtTokenGenerator)
        {
            _authDbContext = authDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _authDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == loginRequestDto.Username);

            if (user == null)
            {
                return new LoginResponseDto
                {
                    UserDto = null,
                    Token = string.Empty
                };
            }
            bool isvalid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isvalid)
            {
                return new LoginResponseDto { UserDto = null, Token = string.Empty };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);
            var result = new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
            };
            return new LoginResponseDto
            {
                UserDto = result,
                Token = token
            };
        }

        public async Task<ResponseDto> Register(RegisterRequestDto registerRequestDto)
        {
            try
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    Email = registerRequestDto.Email,
                    Name = registerRequestDto.Name,
                    UserName = registerRequestDto.Username,
                };

                var result = await _userManager.CreateAsync(applicationUser, registerRequestDto.Password);
                if (result.Succeeded)
                {

                    var assignResult = await AssignRole(registerRequestDto.Username, registerRequestDto.Role);

                    var userToReturn = _authDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == registerRequestDto.Username);
                    UserDto userDto = new UserDto
                    {
                        Email = registerRequestDto.Email,
                        Name = registerRequestDto.Name,
                        Username = registerRequestDto.Username

                    };
                    return new ResponseDto
                    {
                        Result = userDto,
                        IsSuccess = true
                    };
                }
                else
                {
                    var message = string.Empty;
                    foreach (var error in result.Errors)
                        message += error.Description + " ";
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = message.TrimEnd()

                    };
                }
            }
            catch (Exception ex)
            {

                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseDto> AssignRole(string username, string roleName)
        {
            try
            {
                var user = _authDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
                if (user == null)
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "User not found"
                    };

                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

                await _userManager.AddToRoleAsync(user, roleName);
                return new ResponseDto { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message

                };
            }
        }
    }
}