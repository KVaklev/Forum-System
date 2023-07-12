
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
            try
            {
                if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
                {
                    return UnLoggedErrorView();
                }
                else if ((this.HttpContext.Session.GetString("IsAdmin")) != "True")
                {
                    return NotAdminErrorView();
                }
                else 
                {
                    var categoryViewModel = new CategoryViewModel();
                    return this.View(categoryViewModel);
                }
            }
            catch (UnauthenticatedOperationException ex)
            {
                return UnauthorizedErrorView(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel categoryViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var category = this.mapper.Map<Category>(categoryViewModel);
                    var loggedUser = GetLoggedUser();
                    var createdCategory = this.categoryService.Create(category, loggedUser);
                    return this.RedirectToAction("Index", "Home", new { id = createdCategory.Id });
                }
            }
            catch (DuplicateEntityException ex)
            {
                this.ViewData["ErrorMessage"] = ex.Message;
                return this.View(categoryViewModel);
            }
            catch (UnauthorizedOperationException ex)
            {
                return UnauthorizedErrorView(ex.Message);
            }

            return this.View(categoryViewModel);
        }
        
        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            try
            {
                if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
                {
                    return UnLoggedErrorView();
                }
                else if ((this.HttpContext.Session.GetString("IsAdmin")) != "True")
               {
                return NotAdminErrorView();
                }
               else
               { 
                    var category = this.categoryService.GetById(id);
                    var categoryViewModel = this.mapper.Map<CategoryViewModel>(category);
                    return this.View(categoryViewModel);
               }
            }
            catch (EntityNotFoundException ex)
            {
                return EntityErrorView(ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return UnauthorizedErrorView(ex.Message);
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
                var loggedUser = GetLoggedUser();
                var category = mapper.Map<Category>(categoryViewModel);
                var updatedCategory = this.categoryService.Update(id, category, loggedUser);

                return this.RedirectToAction("Index", "Home", new { id = updatedCategory.Id });
            }
            catch (DuplicateEntityException ex)
            {
                this.ViewData["ErrorMessage"] = ex.Message;
                return this.View(categoryViewModel);
            }
            catch (UnauthorizedOperationException ex)
            {
                return UnauthorizedErrorView(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
                {
                    return UnLoggedErrorView();
                }
                else if ((this.HttpContext.Session.GetString("IsAdmin")) != "True")
                {
                    return NotAdminErrorView();
                }
                else 
                { 
                    var category = this.categoryService.GetById(id);
                    return this.View(category);   
                }
            }
            catch (EntityNotFoundException ex)
            {
                return EntityErrorView(ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return UnauthorizedErrorView(ex.Message);
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed([FromRoute] int id)
        {
            try
            {
                var loggedUser = GetLoggedUser();
                var category = this.categoryService.GetById(id);
                this.categoryService.Delete(id, loggedUser);

                return this.RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException ex)
            {
                return EntityErrorView(ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return UnauthorizedErrorView(ex.Message);
            }
        }

        private User GetLoggedUser()
        {
            var username = this.HttpContext.Session.GetString("LoggedUser");
            var user = authManager.TryGetUserByUsername(username);
            return user;
        }

        private IActionResult EntityErrorView(string message)
        {
            this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            this.ViewData["ErrorMessage"] = message;

            return this.View("Error");
        }

        private IActionResult UnauthorizedErrorView(string message)
        {
            this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            this.ViewData["ErrorMessage"] = message;

            return this.View("UnauthorizedError");
        }

        private IActionResult UnLoggedErrorView()
        {
            this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            this.ViewData["ErrorMessage"] = "You are a \"UNLOGGED USER\"!";
            return this.View("UnauthorizedError");

        }

        private IActionResult NotAdminErrorView()
        {
            this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            this.ViewData["ErrorMessage"] = "You are not \"ADMIN USER\"!";
            return this.View("UnauthorizedError");

        }

    }
}
