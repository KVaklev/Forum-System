
using AutoMapper;
using Business.Exceptions;
using Business.ViewModels.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using System.Net;

namespace ForumManagementSystem.Controllers.MVC
{
    public class CategoriesController:Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IAuthManager authManager;

        public CategoriesController(
            ICategoryService categoryService, 
            IMapper mapper, 
            IAuthManager authManager,
            IUserService userService)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.authManager = authManager;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categoryViewModel = new CategoryViewModel();

            return this.View(categoryViewModel);
        }
        [HttpPost]
        public IActionResult Create(CategoryViewModel categoryViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var category = this.mapper.Map<Category>(categoryViewModel);

                    var username = this.HttpContext.Session.GetString("LoggedUser");
                    var user = authManager.TryGetUserByUsername(username);
                    var createdCategory = this.categoryService.Create(category, user);
                    return this.RedirectToAction("Index", "Home", new { id = createdCategory.Id });
                }
            }
            catch (DuplicateEntityException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = ex.Message;
                return this.View(categoryViewModel);
            }
            catch (UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("UnauthorizedError");
            }

            return this.View(categoryViewModel);
        }
        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            try
            {
               var category = this.categoryService.GetById(id);
               var categoryViewModel = this.mapper.Map<CategoryViewModel>(category);

                return this.View(categoryViewModel);
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

                return this.View("UnauthorizedError");
            }
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CategoryViewModel categoryViewModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(categoryViewModel);
                }
                var username = this.HttpContext.Session.GetString("LoggedUser");
                var user = authManager.TryGetUserByUsername(username);
                var category = mapper.Map<Category>(categoryViewModel);
                var updatedCategory = this.categoryService.Update(id, category, user);

                return this.RedirectToAction("Index", "Home", new { id = updatedCategory.Id });
            }
            catch (DuplicateEntityException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = ex.Message;
                return this.View(categoryViewModel);
            }
            catch (UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("UnauthorizedError");
            }
        }

        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var category = this.categoryService.GetById(id);

                return this.View(category);
            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
            catch (UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("UnauthorizedError");
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed([FromRoute] int id)
        {
            try
            {
                var username = this.HttpContext.Session.GetString("LoggedUser");
                var user = authManager.TryGetUserByUsername(username);
                var category = this.categoryService.GetById(id);
                this.categoryService.Delete(id, user);

                return this.RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
            catch (UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("UnauthorizedError");
            }
        }
		
		

	}
}
