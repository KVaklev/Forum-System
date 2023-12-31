﻿using AutoMapper;
using Business.Exceptions;
using Business.ViewModels.Models;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using System.Text;

namespace ForumManagementSystem.Controllers.MVC
{
	public class AuthController : Controller
	{
		private readonly IAuthManager authManager;
		private readonly IUserService userService;
		private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthController(IAuthManager authManager, IUserService userService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.authManager = authManager;
            this.userService = userService;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
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
				var posts = this.userService.GetAll();
				this.HttpContext.Session.SetString("LoggedUser", user.Username);
				this.HttpContext.Session.SetInt32("UserId", user.Id);
				this.HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
				this.HttpContext.Session.SetString("IsBlocked", user.IsBlocked.ToString());
				this.HttpContext.Session.SetString("FirstName", user.FirstName);
				this.HttpContext.Session.SetString("LastName", user.LastName);

				return RedirectToAction("Index", "Home");
			}
			catch (AuthenticationException ex)
			{
				HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View(loginViewModel);
			}
			catch (UnauthorizedOperationException ex)
			{
				HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View(loginViewModel);
			}
			catch (UnauthenticatedOperationException ex)
			{
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
			var codedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(registerViewModel.Password));

            registerViewModel.Password = codedPassword.ToString();

            User user = this.mapper.Map<User>(registerViewModel);

			this.userService.Create(user);

            if (registerViewModel.ImageFile != null)
            {
                string imageUploadedFolder = Path.Combine(webHostEnvironment.WebRootPath, "UploadedImages");
                string uniqueFileName = user.FirstName + " " + user.LastName + ".png";
                string filePath = Path.Combine(imageUploadedFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    registerViewModel.ImageFile.CopyTo(fileStream);
                }
                user.ProfilePhotoPath = "~/UploadedImages";
                user.ProfilePhotoFileName = uniqueFileName;
            }

            return this.RedirectToAction("Login", "Auth");
		}
	}
}
