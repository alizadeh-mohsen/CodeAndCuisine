using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace CodeAndCuisine.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> Products = new();
            var response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
                Products = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
            else
                TempData["error"] = response.Message;

            return View(Products);
        }

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            var product = new ProductDto();
            var response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
                product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
            else
                TempData["error"] = response.Message;
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> details(ProductDto productDto)
        {
            var cartDto = new ShoppingCartDto
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(c => c.Type == IdentityModel.JwtClaimTypes.Subject)?.FirstOrDefault().Value,
                },

            };
            var cartDetails = new CartDetailDto
            {
                ProductId = productDto.ProductId,
                Quantity = productDto.Count
            };

            List<CartDetailDto> cartDetailDtos = new List<CartDetailDto>
            {
                cartDetails
            };
            cartDto.CartDetails = cartDetailDtos;

            var response = await _cartService.CardUpsert(cartDto);


            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item added to shopping cart";

                return RedirectToAction("Index");
            }

            else
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Index", "Error", response.Message);
            }
        }
    }
}