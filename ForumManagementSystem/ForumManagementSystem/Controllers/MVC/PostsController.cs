using AutoMapper;
using Business.Exceptions;
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
        private readonly ITagService tagService;


        public PostsController(IPostService postService, IAuthManager authManager, IMapper mapper, ILikePostService likePostService, ICategoryService categoryService, ITagService tagService)
        {
            this.postService = postService;
            this.authManager = authManager;
            this.mapper = mapper;
            this.likePostService = likePostService;
            this.categoryService = categoryService;
            this.tagService = tagService;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] PostQueryParameters queryParameters)
        {
            List<Post> posts = this.postService.FilterBy(queryParameters);


            //posts = posts.Where(p => p.CategoryId == id).ToList(); // Apply category filter

            //List<PostViewModel> postsViewModel = posts.Select(post => mapper.Map<PostViewModel>(posts)).ToList();

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


            var postViewModel = new PostViewModel();
            this.InitializeCategories(postViewModel);
            this.InitializeTags(postViewModel);

            return this.View(postViewModel);
        }

        [HttpPost]
        public IActionResult Create(PostViewModel postViewModel)
        {
            this.InitializeCategories(postViewModel);
            this.InitializeTags(postViewModel);
            try
            {

                if (this.ModelState.IsValid)
                {
                    var userName = this.HttpContext.Session.GetString("LoggedUser");
                    var user = authManager.TryGetUserByUsername(userName);
                    var post = mapper.Map<Post>(postViewModel);
                    var createdPost = postService.Create(post, user, postViewModel.Tags);
                    return RedirectToAction("Details", "Posts", new { id = createdPost.Id });
                }
            }
            catch (DuplicateEntityException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = ex.Message;
                return this.View(postViewModel);
            }

            catch (UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;
                return this.View("UnauthorizedError");
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
                this.InitializeTags(postViewModel);
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
            try
            {
                if (!this.ModelState.IsValid)
                {
                    this.InitializeCategories(postViewModel);
                    this.InitializeTags(postViewModel);
                    return View(postViewModel);

                }

                var userName = this.HttpContext.Session.GetString("LoggedUser");
                var user = authManager.TryGetUserByUsername(userName);
                var post = mapper.Map<Post>(postViewModel);
                var updatedPost = this.postService.Update(id, post, user, postViewModel.Tags);

                return this.RedirectToAction("Index", "Posts", new { id = updatedPost.Id });
            }
            catch (UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("UnauthorizedError");
            }
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
                var userName = HttpContext.Session.GetString("LoggedUser");
                var user = this.authManager.TryGetUserByUsername(userName);
                this.postService.Delete(id, user);

                return this.RedirectToAction("Index", "Posts");
            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
            catch(UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("UnauthorizedError");
            }
        }

        private void InitializeCategories(PostViewModel postViewModel)
        {
            var categories = categoryService.GetAll();
            postViewModel.Categories = new SelectList(categories, "Id", "Name");
        }

        private void InitializeTags(PostViewModel postViewModel)
        {
            var tags = tagService.GetAll();
            postViewModel.SelectTags = new SelectList(tags, "Id", "Name");
        }

    }
}
