using AspNetCoreDemo.Models;
using AutoMapper;
using Business.ViewModels.Models;
using DataAccess.Models;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly IAuthManager authManager;

        public HomeController(ICategoryService categoryService, IAuthManager authManager, IUserService userService)
        {
            this.categoryService = categoryService;
            this.authManager = authManager;
            this.userService = userService;
        }
        [HttpGet]
        public IActionResult Index([FromQuery] CategoryQueryParameter categoryQueryParameter)
        {
            PaginatedList<Category> result = categoryService.FilterBy(categoryQueryParameter);

            var usersCount = this.userService.GetAll().Count();
            var postsCount = this.userService.GetAll().Count();

            var viewModel = new Home
            {
                Categories = result,
                UsersCount = usersCount,
                PostsCount = postsCount
            };

            return View(viewModel);
         
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
