using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace CodeAndCuisine.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
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
    }
}