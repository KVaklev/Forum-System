﻿using AspNetCoreDemo.Models;
using AutoMapper;
using Business.Exceptions;
using Business.QueryParameters;
using Business.Services.Contracts;
using Business.ViewModels.Models;
using DataAccess.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Helpers;
using System.Linq;


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
        public IActionResult Index([FromQuery] PostQueryParameters postQueryParameters)
        {
            if (!this.HttpContext.Session.Keys.Contains("LoggedUser"))
            {
                return RedirectToAction("Login", "Auth");
            }

            List<Post> posts = this.postService.FilterBy(postQueryParameters);

            var filterViewModel = new PostSearchModel
            {
                Posts = (AspNetCoreDemo.Models.PaginatedList<Post>)posts,
                PostQueryParameters = postQueryParameters
                

            };

            return View(filterViewModel);

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

            var userName = this.HttpContext.Session.GetString("LoggedUser");
            var user = authManager.TryGetUserByUsername(userName);

            if (user.IsBlocked)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = "You are blocked";
                return this.View("UnauthorizedError");
            }
            else
            {

                var postViewModel = new PostViewModel();
                this.InitializeCategories(postViewModel);
                this.InitializeTags(postViewModel);

                return this.View(postViewModel);
            }


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

                var tags = post.PostTags
                       .Where(p => p.PostId == id)
                       .Select(p => p.Tag.Name)
                       .ToList();

                var tagNames = string.Join(" ", tags);
                              

                var postViewModel = this.mapper.Map<PostViewModel>(post);


                this.InitializeCategories(postViewModel);
                this.InitializeTags(postViewModel);

                postViewModel.Tags = tagNames;
                
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
            catch (UnauthorizedOperationException ex)
            {
                this.HttpContext.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("UnauthorizedError");
            }
        }

        [HttpGet]
        public IActionResult Like([FromRoute] int id)
        {
            try
            {
                var username = this.HttpContext.Session.GetString("LoggedUser");
                var user = this.authManager.TryGetUserByUsername(username);
                Post post = postService.GetById(id);
                likePostService.Update(post, user);
                return RedirectToAction("Index", "Posts", new { post = post.Id });

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

        private void InitializeTags(PostViewModel postViewModel)
        {
            var tags = tagService.GetAll();
            postViewModel.SelectTags = new SelectList(tags, "Id", "Name");
        }

    }
}
