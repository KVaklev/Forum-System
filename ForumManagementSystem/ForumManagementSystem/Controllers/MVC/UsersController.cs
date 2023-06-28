using AutoMapper;
using Business.ViewModels.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.MVC
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IAuthManager authManager;
        public UsersController(IUserService userService, IMapper mapper, IAuthManager authManager)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.authManager = authManager;
        }

		[HttpGet]
		public IActionResult Index()
		{
			List<User> users = this.userService.GetAll();

			return this.View(users);
		}
		[HttpGet]
        public IActionResult Details(int id)
        {
			try
			{
				User user = this.userService.GetById(id);

				return this.View(user);
			}
			catch (EntityNotFoundException e)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = e.Message;

				return this.View("Error");
			}
		}

		[HttpGet]
		public IActionResult Create()
		{
			var userViewModel = new UserViewModel();

			return this.View(userViewModel);
		}
	}
}
