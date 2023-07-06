using AutoMapper;
using Business.Exceptions;
using Business.ViewModels.Models;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.MVC
{
	public class AuthController : Controller
	{
		private readonly IAuthManager authManager;
		private readonly IUserService userService;
		private readonly IMapper mapper;

        public AuthController(IAuthManager authManager, IUserService userService, IMapper mapper)
        {
            this.authManager = authManager;
			this.userService = userService;
			this.mapper = mapper;
        }

		[HttpGet]
        public IActionResult Login()
		{
			var loginViewModel = new LoginViewModel();

			return this.View(loginViewModel);
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel loginViewModel)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(loginViewModel);
			}

			try
			{
				var user = this.authManager.TryGetUser(loginViewModel.Username, loginViewModel.Password);
				var users = this.userService.GetAll();
				var posts=this.userService.GetAll();
				this.HttpContext.Session.SetString("LoggedUser", user.Username);
				this.HttpContext.Session.SetInt32("UserId", user.Id);
				this.HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
				
				return RedirectToAction("Index", "Home");
			}
			catch (AuthenticationException ex)
			{
				HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden; 
				this.ViewData["ErrorMessage"]=ex.Message;

				return this.View(loginViewModel);
			}
			catch (UnauthorizedOperationException ex)
			{
				HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View(loginViewModel);
			}
		}

		[HttpGet]
		public IActionResult Logout()
		{
			this.HttpContext.Session.Remove("LoggedUser");

			return RedirectToAction(actionName: "Index", controllerName: "Home");

		}

		[HttpGet]
		public IActionResult Register()
		{
			var registerViewModel = new RegisterViewModel();

			return this.View(registerViewModel);
		}

		[HttpPost]
		public IActionResult Register(RegisterViewModel registerViewModel)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(registerViewModel);
			}

			if (this.userService.UsernameExists(registerViewModel.Username))
			{
				this.ModelState.AddModelError("Username", "Username already exists.");

				return this.View(registerViewModel);
			}

			if (registerViewModel.Password != registerViewModel.ConfirmPassword)
			{
				this.ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");

				return this.View(registerViewModel);
			}

			User user = this.mapper.Map<User>(registerViewModel);
			this.userService.Create(user);

			return this.RedirectToAction("Login", "Auth");
		}
	}
}
