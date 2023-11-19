using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CodeAndCuisine.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProductDto> Products = new();
            var response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                Products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                return View(Products);

            }

            TempData["error"] = response.Message;
            return RedirectToAction("Index", "Home");


        }

        #region Create


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var responseDto = await _productService.CreateProductAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }

                else
                    TempData["error"] = responseDto.Message;
            }

            return View(model);


        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {

            var ProductDto = await GetProduct(id);

            if (ProductDto != null)

                return View(ProductDto);


            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                
                var responseDto = await _productService.UpdateProductAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction("Index");
                }

                else
                    TempData["error"] = responseDto.Message;
            }


            return View(model);


        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {

            var ProductDto = await GetProduct(id);

            if (ProductDto != null)

                return View(ProductDto);


            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ProductDto ProductDto)
        {
            var responseDto = await _productService.DeleteProductProductAsync(ProductDto.ProductId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction("Index");
            }

            else
                TempData["error"] = responseDto.Message;

            return View();
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {

            var ProductDto = await GetProduct(id);

            if (ProductDto != null)

                return View(ProductDto);


            return NotFound();
        }

        #endregion


        private async Task<ProductDto> GetProduct(int ProductId)
        {
            ProductDto Product = null;

            var responseDto = await _productService.GetProductByIdAsync(ProductId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
            }
            return Product;
        }
    }
}