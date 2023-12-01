using Microsoft.AspNetCore.Mvc;

namespace CodeAndCuisine.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
