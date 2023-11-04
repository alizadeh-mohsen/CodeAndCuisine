using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using CodeAndCuisine.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CodeAndCuisine.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequestDto model)
        {
            if (ModelState.IsValid)
            {
                var responseDto = await _authService.LoginAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                    TempData["success"] = "Logged in successfully";
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("CustomError", responseDto.Message);
                    return View(model);
                }
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var RoleList = new List<SelectListItem>(){

                new SelectListItem { Text = StaticData.Admin, Value = StaticData.Admin },
                new SelectListItem { Text = StaticData.Customer, Value = StaticData.Customer }
            };
            ViewBag.RoleList = RoleList;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterRequestDto model)
        {
            if (ModelState.IsValid)
            {
                var responseDto = await _authService.RegisterAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Registered successfully";
                    return RedirectToAction("Index", "Home");
                }

                else
                    TempData["error"] = responseDto.Message;
            }

            return View(model);
        }
    }
}