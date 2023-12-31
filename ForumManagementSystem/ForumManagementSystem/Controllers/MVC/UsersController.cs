﻿using AutoMapper;
using Business.Exceptions;
using Business.QueryParameters;
using Business.ViewModels.Models;
using DataAccess.Models;
using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using System.Text;

namespace ForumManagementSystem.Controllers.MVC
{
	public class UsersController : Controller
	{
		private readonly IUserService userService;
		private readonly IMapper mapper;
		private readonly IAuthManager authManager;
		private readonly IWebHostEnvironment webHostEnvironment;
		public UsersController(IUserService userService, IMapper mapper, IAuthManager authManager, IWebHostEnvironment webHostEnvironment)
		{
			this.userService = userService;
			this.mapper = mapper;
			this.authManager = authManager;
			this.webHostEnvironment = webHostEnvironment;
		}

		[HttpGet]
		public IActionResult Index(UserQueryParameters userQueryParameters)
		{
			if (!this.HttpContext.Session.Keys.Contains("LoggedUser"))
			{
				return RedirectToAction("Login", "Auth");
			}
			List<User> users = this.userService.FilterBy(userQueryParameters);

			var newViewModel = new UserSearchModel
			{
				Users = (AspNetCoreDemo.Models.PaginatedList<User>)users,
				UserQueryParameters = userQueryParameters

            };

            return this.View(newViewModel);
		}
		[HttpGet]
		public IActionResult Details([FromRoute] int id)
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
		[HttpPost]
		public IActionResult Create(UserViewModel userViewModel)
		{
			try
			{
				if (this.ModelState.IsValid)
				{
					var user = this.mapper.Map<User>(userViewModel);
					var createdUser = this.userService.Create(user);
					return this.RedirectToAction("Details", "Users", new { id = createdUser.Id });
				}
			}
			catch (DuplicateEntityException ex)
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
				this.ViewData["ErrorMessage"] = ex.Message;
				return this.View(userViewModel);
			}

			return this.View(userViewModel);
		}
		[HttpGet]
		public IActionResult Edit([FromRoute] int id)
		{
			try
			{
				var user = userService.GetById(id);

				var userEditViewModel = this.mapper.Map<UserEditViewModel>(user);

				return this.View(userEditViewModel);
			}
			catch (EntityNotFoundException ex)
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View("Error");
			}
		}
		[HttpPost]
		public IActionResult Edit([FromRoute] int id, UserEditViewModel userEditViewModel)
		{
			if (!this.ModelState.IsValid)
			{
				return View(userEditViewModel);
			}
			try
			{
                var username = this.HttpContext.Session.GetString("LoggedUser");

                var loggedUser = authManager.TryGetUserByUsername(username);

				if (userEditViewModel.Password != null)
				{
					var codedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(userEditViewModel.Password));

					userEditViewModel.Password = codedPassword.ToString();
				}

                var user = mapper.Map<User>(userEditViewModel);


				var updatedUser = this.userService.Update(id, user, loggedUser);

                return this.RedirectToAction("Details", "Users", new { id = updatedUser.Id });
            }
            catch (EntityNotFoundException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
            catch (UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View(userEditViewModel);
            }
            catch (DuplicateEntityException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View(userEditViewModel);
            }

        }
		[HttpGet]
		public IActionResult Delete([FromRoute] int id)
		{
			try
			{
				var user = this.userService.GetById(id);

				return this.View(user);
			}
			catch (EntityNotFoundException ex)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View("Error");
			}
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed([FromRoute] int id)
		{
			try
			{
                var username = this.HttpContext.Session.GetString("LoggedUser");
                var user = authManager.TryGetUserByUsername(username);
                this.userService.Delete(id, user);

				return this.RedirectToAction("Index", "Users");
			}
			catch (EntityNotFoundException ex)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View("Error");
			}
        }

		[HttpGet]
		public IActionResult Profile([FromRoute] int id)
		{
			try
			{
				var user = userService.GetById(id);

				var userUpdateProfileViewModel = this.mapper.Map<UserUpdateProfileViewModel>(user);

				return this.View(userUpdateProfileViewModel);
			}
			catch (EntityNotFoundException ex)
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View("Error");
			}
		}
		[HttpPost, ActionName("Profile")]
		[HttpPost]
		public IActionResult ProfileConfirmed([FromRoute] int id, UserUpdateProfileViewModel userUpdateProfileViewModel)
		{

			if (!this.ModelState.IsValid)
			{
				return View(userUpdateProfileViewModel);
			}
			try
			{
				var userToUpdate = userService.GetById(id);

				if (userUpdateProfileViewModel.NewPassword != null)
				{
					var codedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(userUpdateProfileViewModel.NewPassword));

					userUpdateProfileViewModel.NewPassword = codedPassword.ToString();
				}

				var loggedUser = userToUpdate;

				var user = mapper.Map<User>(userUpdateProfileViewModel);

				userToUpdate = this.userService.Update(id, user, loggedUser);

				if (userUpdateProfileViewModel.ImageFile != null)
				{
					string imageUploadedFolder = Path.Combine(webHostEnvironment.WebRootPath, "UploadedImages");
					string uniqueFileName = userToUpdate.FirstName + " " + userToUpdate.LastName + ".png";
					string filePath = Path.Combine(imageUploadedFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						userUpdateProfileViewModel.ImageFile.CopyTo(fileStream);
					}
					userToUpdate.ProfilePhotoPath = "~/UploadedImages";
					userToUpdate.ProfilePhotoFileName = uniqueFileName;
				}

				return this.RedirectToAction("Details", "Users", new { id = userToUpdate.Id });
			}
			catch (EntityNotFoundException ex)
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View("Error");
			}
			catch (DuplicateEntityException ex)
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View(userUpdateProfileViewModel);
			}
			catch (UnauthorizedOperationException ex)
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View(userUpdateProfileViewModel);
			}
		}
	}
}
