using Microsoft.AspNetCore.Mvc;

namespace ForumManagementSystem.Controllers.MVC
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
