using CodeAndCuisine.Web.Services.IService;
using CodeAndCuisine.Web.Utility;
using NuGet.Common;

namespace CodeAndCuisine.Web.Services
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProviderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(StaticData.TokenCookie);
        }

        public string GetToken()
        {
            string token = null;
            bool? hastoken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticData.TokenCookie, out token);

            return hastoken is true ? token : string.Empty;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticData.TokenCookie, token);
        }
    }
}
