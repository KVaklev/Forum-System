using AspNetCoreDemo.Models;
using AutoMapper;
using Business.Dto;
using Business.Exceptions;
using Business.Services.Contracts;
using Business.Services.Models;
using Business.ViewModels.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using System.Data;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace ForumManagementSystem.Controllers.MVC
{
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly IAuthManager authManager;
        private readonly IMapper mapper;
        private readonly ILikeCommentService likeCommentService;

        public CommentsController(
            ICommentService commentService,
            IPostService postService,
            IUserService userService,
            IMapper mapper,
            IAuthManager authManager,
            ILikeCommentService likeCommentService)
        {
                this.commentService = commentService;
                this.postService = postService;
                this.userService = userService;
                this.mapper = mapper;
                this.authManager = authManager;
                this.likeCommentService = likeCommentService;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] CommentQueryParameters parameter)
        {
            try
            {
                PaginatedList<Comment> comments = this.commentService.FilterBy(parameter);
                return View(comments);
            }
            catch (EntityNotFoundException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
        }

        [HttpGet]
        public IActionResult Filter([FromQuery] CommentQueryParameters parameters)
        {
            try
            {
                PaginatedList<Comment> comments = this.commentService.FilterBy(parameters);
                if (parameters.Username == null)
                {
                    return View("Get", comments);
                }
                else 
                {
                     return View(comments);
                }
            }
            catch (EntityNotFoundException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            if ((this.HttpContext.Session.GetString("IsBlocked")) == "True" 
                || (this.HttpContext.Session.GetString("LoggedUser")) == null)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = "You are not \"LOGGET USER\" or you are \"BLOCKED\"!";
                return this.View("UnauthorizedError");
            }
            else
            {
                var commentViewModel = new CommentViewModel();
                return this.View(commentViewModel);
            }
        }

        [HttpPost]
        public IActionResult Create([FromRoute] int id, CommentViewModel commentViewModel)
        {
            try
            {
               if (this.ModelState.IsValid)
               {
                    var commentCreateVireModel = new CommentCreateViewModel()
                    {
                        Content = commentViewModel.Content,
                        PostId = id
                    };
                    var comment = this.mapper.Map<Comment>(commentCreateVireModel);
                    var username = this.HttpContext.Session.GetString("LoggedUser");
                    var user = authManager.TryGetUserByUsername(username);
                    var createdComment = this.commentService.Create(comment, user);
                    
                    return this.RedirectToAction("Index", "Comments", new { postID = createdComment.PostId });
               }
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

            return this.View(commentViewModel);
        }
        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            try
            {
                var comment = this.commentService.GetByID(id);
                var commentViewModel = this.mapper.Map<CommentViewModel>(comment);
                               
                return this.View(commentViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CommentViewModel commentViewModel)
        {
            try
            {

                if (!this.ModelState.IsValid)
                {
                    return View(commentViewModel);
                }

                var commentToUpdate = this.commentService.GetByID(id);
                var commentCreateViewModel = new CommentCreateViewModel()
                {
                    PostId = commentToUpdate.PostId,
                    Content = commentViewModel.Content
                };
                var inputComment = mapper.Map<Comment>(commentCreateViewModel);
                var username = this.HttpContext.Session.GetString("LoggedUser");
                var user = authManager.TryGetUserByUsername(username);
                var updatedComment = this.commentService.Update(id, inputComment, user);

                return this.RedirectToAction("Index", "Comments", new { postId = updatedComment.PostId });
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

        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var comment = this.commentService.GetByID(id);
                return this.View(comment);
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
                var comment = this.commentService.Delete(id, user);

                return this.RedirectToAction("Index", "Comments", new { postId = comment.PostId });
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

        [HttpGet]
        public IActionResult Like([FromRoute] int id)
        {
			try
			{
                var username = this.HttpContext.Session.GetString("LoggedUser");
                var user = authManager.TryGetUserByUsername(username);
                Comment comment = commentService.GetByID(id);
				likeCommentService.Update(comment, user);
                return this.RedirectToAction("Index", "Comments", new { postId = comment.PostId }); 
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

		[HttpGet]
		public IActionResult CreateReply()
		{
            if ((this.HttpContext.Session.GetString("IsBlocked")) == "True"
                || (this.HttpContext.Session.GetString("LoggedUser")) == null)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = "You are not \"LOGGED USER\" or you are \"BLOCKED\"!";
                return this.View("UnauthorizedError");
            }
            else
            {
                var commentViewModel = new CommentViewModel();
                return this.View(commentViewModel);
            }
		}

		[HttpPost]
		public IActionResult CreateReply([FromRoute] int id, CommentViewModel commentViewModel)
		{
			try
			{
				if (this.ModelState.IsValid)
				{
                    var comment = commentService.GetByID(id);
                    var commentReplyCreateViewModel = new CommentReplyCreateViewModel()
                    {
                        Content =$"\"{comment.CreatedBy.Username}: {comment.Content}\" - " + $" {commentViewModel.Content}",
                        PostId = comment.PostId,
                        CommentId = comment.Id
					};
					var commentReply = this.mapper.Map<Comment>(commentReplyCreateViewModel);

                    var username = this.HttpContext.Session.GetString("LoggedUser");
                    var user = authManager.TryGetUserByUsername(username);
                    var createdComment = this.commentService.Create(commentReply, user);
					return this.RedirectToAction("Filter", "Comments", new { username=user.Username});
				}
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

			return this.View(commentViewModel);
		}

	}
}
