using CodaAndCuisine.Services.AuthAPI.Models;

namespace CodaAndCuisine.Services.AuthAPI.Services.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);

    }
}
