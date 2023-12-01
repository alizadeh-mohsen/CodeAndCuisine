using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace CodeAndCuisine.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await LoadCartDtoBasedOnLoggedInuser());
        }

        private async Task<ShoppingCartDto> LoadCartDtoBasedOnLoggedInuser()
        {
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto responseDto = await _cartService.GetShoppingCart(userId);

            if (responseDto != null && responseDto.IsSuccess)
                return JsonConvert.DeserializeObject<ShoppingCartDto>(responseDto.Result.ToString());

            else
            {
                TempData["error"] = responseDto.Message;
                return new ShoppingCartDto();
            }
        }
    }
}