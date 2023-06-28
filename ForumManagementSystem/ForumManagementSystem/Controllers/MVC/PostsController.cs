using AutoMapper;
using Business.Services.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.MVC
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IAuthManager authManager;
        private readonly IMapper mapper;
        private readonly ILikePostService likePostService;

        public PostsController(IPostService postService, IAuthManager authManager, IMapper mapper, ILikePostService likePostService)
        {
            this.postService = postService;
            this.authManager = authManager;
            this.mapper = mapper;
            this.likePostService = likePostService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Post> posts = this.postService.GetAll();

            return View(posts);
        }

        //[HttpGet]
        //public IActionResult Details(int id)
        //{
        //    try
        //    {
        //        Post post = this.postService.GetById();
        //        return View(post);
        //    }
        //    catch (EntityNotFoundException e)
        //    {
        //        this.Response.StatusCode = StatusCodes.Status404NotFound;
        //        this.ViewData["ErrorMessage"] = e.Message;

        //        return this.View("Error");
        //    }
        //}
    }
}
