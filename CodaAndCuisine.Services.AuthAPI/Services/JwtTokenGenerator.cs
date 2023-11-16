using CodaAndCuisine.Services.AuthAPI.Models;
using CodaAndCuisine.Services.AuthAPI.Services.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodaAndCuisine.Services.AuthAPI.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(ApplicationUser applicationUser)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);


            var claimList = new List<Claim> {

                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email)
            };

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {

                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = handler.CreateToken(securityTokenDescriptor);
            return handler.WriteToken(token);

        }
    }

}