using AutoMapper;
using Business.ViewModels.Models;
using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.MVC
{
	public class UsersController : Controller
	{
		private readonly IUserService userService;
		private readonly IMapper mapper;
		private readonly IAuthManager authManager;
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly ApplicationContext aplicationContext;

		public UsersController(IUserService userService, IMapper mapper, IAuthManager authManager, IWebHostEnvironment webHostEnvironment, ApplicationContext aplicationContext)
		{
			this.userService = userService;
			this.mapper = mapper;
			this.authManager = authManager;
			this.webHostEnvironment = webHostEnvironment;
			this.aplicationContext = aplicationContext;
		}

		[HttpGet]
		public IActionResult Index(UserQueryParameters userQueryParameters)
		{
			if (!this.HttpContext.Session.Keys.Contains("LoggedUser"))
			{
				return RedirectToAction("Login", "Auth");
			}
			List<User> users = this.userService.FilterBy(userQueryParameters);

			return this.View(users);
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
			var loggedUser = this.authManager.TryGetUser("ivanchoDraganchov:123");

			var user = mapper.Map<User>(userEditViewModel);

			var updatedUser = this.userService.Update(id, user, loggedUser);

			return this.RedirectToAction("Details", "Users", new { id = updatedUser.Id });
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
				var user = this.authManager.TryGetUser("ivanchoDraganchov:123");
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
			var userToUpdate=userService.GetById(id);

			var loggedUser = this.authManager.TryGetUser(userToUpdate.Username,userUpdateProfileViewModel.Password);

			var user = mapper.Map<User>(userUpdateProfileViewModel);

			userToUpdate = this.userService.Update(id, user, loggedUser);

            if (userUpdateProfileViewModel.ImageFile != null)
            {
                string imageUploadedFolder = Path.Combine(webHostEnvironment.WebRootPath, "UploadedImages");
                string uniqueFileName = Guid.NewGuid().ToString() + "_"  + userUpdateProfileViewModel.ImageFile.FileName;
                string filePath = Path.Combine(imageUploadedFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    userUpdateProfileViewModel.ImageFile.CopyTo(fileStream);
                }
                userToUpdate.ProfilePhotoPath = "~/UploadedImages";
                userToUpdate.ProfilePhotoFileName = uniqueFileName;

                aplicationContext.Add(userUpdateProfileViewModel);
                aplicationContext.SaveChanges();


                //return this.RedirectToAction("Index");
            }

            return this.RedirectToAction("Details", "Users", new { id = userToUpdate.Id });
		}
	}
}
