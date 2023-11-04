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
            var token = _jwtTokenGenerator.GenerateToken(user);
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

        public async Task<UserDto> Register(RegisterRequestDto registerRequestDto)
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
                        Username = registerRequestDto.Username,
                        Role = registerRequestDto.Role

                    };
                    return userDto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<bool> AssignRole(string username, string roleName)
        {
            var user = _authDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            if (user == null)
                return false;

            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }
    }
}