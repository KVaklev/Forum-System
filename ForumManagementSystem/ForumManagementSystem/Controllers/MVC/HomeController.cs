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

        private readonly IPostService postService;

        public HomeController(ICategoryService categoryService, IUserService userService, IPostService postService)
        {
            this.categoryService = categoryService;
            this.userService = userService;
            this.postService = postService;
        }
        [HttpGet]
        public IActionResult Index([FromQuery] CategoryQueryParameter categoryQueryParameter)
        {
            PaginatedList<Category> result = categoryService.FilterBy(categoryQueryParameter);

            var usersCount = this.userService.GetAll().Count();
            var postsCount = this.postService.GetAll().Count();

            var viewModel = new Home
            {
                Categories = result,
                UsersCount = usersCount,
                PostsCount = postsCount
            };

            return View(viewModel);
         
        }
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TopTen(PostQueryParameters queryParameters)
        {
            PaginatedList<Post> result = this.postService.GetTopTenCommented(queryParameters);

            var usersCount = this.userService.GetAll().Count();
            var postsCount = this.postService.GetAll().Count();

            var viewModel = new Home
            {
                Posts = result,
                UsersCount = usersCount,
                PostsCount = postsCount
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult LatestTen(PostQueryParameters queryParameters)
        {
            PaginatedList<Post> result = this.postService.GetLastTenCommented(queryParameters);

            var usersCount = this.userService.GetAll().Count();
            var postsCount = this.postService.GetAll().Count();

            var viewModel = new Home
            {
                Posts = result,
                UsersCount = usersCount,
                PostsCount = postsCount
            };

            return View(viewModel);
        }

    }
}
