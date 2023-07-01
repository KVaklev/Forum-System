using AutoMapper;
using Business.Dto;
using Business.Services.Contracts;
using Business.Services.Models;
using Business.ViewModels.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;

namespace ForumManagementSystem.Controllers.MVC
{
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IPostService postService;
        private readonly IAuthManager authManager;
        private readonly IMapper mapper;
        private readonly ILikeCommentService likeCommentService;

        public CommentsController(
            ICommentService commentService,
            IPostService postService,
            IMapper mapper,
            IAuthManager authManager,
            ILikeCommentService likeCommentService)
        {
                this.commentService = commentService;
                this.postService = postService;
                this.mapper = mapper;
                this.authManager = authManager;
                this.likeCommentService = likeCommentService;
        }

        [HttpGet]
        public IActionResult Index([FromRoute] int id)
        {
            try
            {


                Post post = this.postService.GetById(id);
                CommentQueryParameters parameter = new CommentQueryParameters()
                {
                    postID = id
                };
                List<Comment> comments = this.commentService.FilterBy(parameter);
                List<CommentGetViewModel> commentsGetView = comments
                        .Select(comment => mapper.Map<CommentGetViewModel>(comment)).ToList();

                return View(commentsGetView);
            }
            catch (EntityNotFoundException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
        }
    }
}
