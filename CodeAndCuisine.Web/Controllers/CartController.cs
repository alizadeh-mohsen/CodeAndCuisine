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


        [HttpPost]
        public async Task<IActionResult> Remove(int detaiId)
        {
            ResponseDto responseDto = await _cartService.RemoveItem(detaiId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";

            }
            return RedirectToAction("Index");


        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(ShoppingCartDto shoppingCartDto)
        {
            if (shoppingCartDto.CartHeader.CouponCode == null)
                return View();

            ResponseDto responseDto = await _cartService.ApplyCoupon(shoppingCartDto);
            if (responseDto != null && responseDto.IsSuccess)
                TempData["success"] = "Coupon accepted";

            return RedirectToAction("Index");


        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(ShoppingCartDto shoppingCartDto)
        {
            shoppingCartDto.CartHeader.CouponCode = string.Empty;
            ResponseDto responseDto = await _cartService.ApplyCoupon(shoppingCartDto);
            if (responseDto != null && responseDto.IsSuccess)

                TempData["success"] = "Coupon accepted";
            return RedirectToAction("Index");


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