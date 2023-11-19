using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using CodeAndCuisine.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CodeAndCuisine.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProviderService _tokenProviderService;

        public AuthController(IAuthService authService, ITokenProviderService tokenProviderService)
        {
            _authService = authService;
            _tokenProviderService = tokenProviderService;
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

                    _tokenProviderService.SetToken(loginResponseDto.Token);
                    await SignInUser(loginResponseDto);

                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    //ModelState.AddModelError("CustomError", responseDto.Message);
                    TempData["error"] = responseDto.Message;
                    return View(model);
                }
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProviderService.ClearToken();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            setRoles();
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
                {
                    TempData["error"] = responseDto.Message;
                    setRoles();
                }

            }

            return View(model);
        }

        private async Task SignInUser(LoginResponseDto loginResponseDto)
        {

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponseDto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
            jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));



            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);


        }

        private void setRoles()
        {

            var RoleList = new List<SelectListItem>(){

                new SelectListItem { Text = StaticData.Admin, Value = StaticData.Admin },
                new SelectListItem { Text = StaticData.Customer, Value = StaticData.Customer }
            };
            ViewBag.RoleList = RoleList;
        }
    }
}