using AutoMapper;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IAuthManager authManager;

        public HomeController(ICategoryService categoryService, IAuthManager authManager)
        {
            this.categoryService = categoryService;
            this.authManager = authManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Category> result = categoryService.GetAll();
            return View(result);
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
