using AutoMapper;
using Business.Services.Contracts;
using Business.ViewModels.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.MVC
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IAuthManager authManager;
        private readonly IMapper mapper;
        private readonly ILikePostService likePostService;
        private readonly ICategoryService categoryService;

        public PostsController(IPostService postService, IAuthManager authManager, IMapper mapper, ILikePostService likePostService, ICategoryService categoryService)
        {
            this.postService = postService;
            this.authManager = authManager;
            this.mapper = mapper;
            this.likePostService = likePostService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Post> posts = this.postService.GetAll();

            return View(posts);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                Post post = this.postService.GetById(id);
                return View(post);
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
            if (!this.HttpContext.Session.Keys.Contains("LoggedUser"))
            {
               return RedirectToAction("Login", "Auth");
            }

            //if (this.HttpContext.Session.GetString("LoggedUser") == null)
            //{
            //    return RedirectToAction("Login", "Auth");
            //}

            var postViewModel = new PostViewModel();
            this.InitializeCategories(postViewModel);
            return this.View(postViewModel);
        }

        [HttpPost]
        public IActionResult Create(PostViewModel postViewModel)
        {
            this.InitializeCategories(postViewModel);
            try
            {
				
				if (this.ModelState.IsValid)
                {
					var user = authManager.TryGetUser("ivanchoDraganchov:123");
					var tagNames = new List<string>();
                    var post = mapper.Map<Post>(postViewModel);
                    var createdPost = postService.Create(post, user, tagNames);
                    return RedirectToAction("Details", "Posts", new { id = createdPost.Id });
                }                
            }
            catch (DuplicateEntityException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = ex.Message;
                return this.View(postViewModel);
            }

            return this.View(postViewModel);
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            try
            {
                var post = postService.GetById(id);

                var postViewModel = this.mapper.Map<PostViewModel>(post);
                this.InitializeCategories(postViewModel);
                return this.View(postViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, PostViewModel postViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                this.InitializeCategories(postViewModel);
                return View(postViewModel);
            }
            var loggedUser = authManager.TryGetUser("ivanchoDraganchov:123");
            var post = mapper.Map<Post>(postViewModel);
            var tagsToEdit = new List<string>();
            var updatedPost = this.postService.Update(id, post, loggedUser, tagsToEdit);

            return this.RedirectToAction("Index", "Posts", new { id = updatedPost.Id });
        }

        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var post = this.postService.GetById(id);

                return this.View(post);
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
                this.postService.Delete(id, user);

                return this.RedirectToAction("Index", "Posts");
            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
        }

        private void InitializeCategories(PostViewModel postViewModel)
        {
            var categories = categoryService.GetAll();
            postViewModel.Categories = new SelectList(categories, "Id", "Name");
        }

    }
}
