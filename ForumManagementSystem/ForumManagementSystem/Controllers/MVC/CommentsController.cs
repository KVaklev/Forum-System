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
                CommentQueryParameters parameter = new CommentQueryParameters()
                {
                    postID = id
                };
                PaginatedList<Comment> comments = this.commentService.FilterBy(parameter);
                PaginatedList<CommentGetViewModel> commentsGetView = new PaginatedList<CommentGetViewModel>(
                    comments.Select(comment => mapper.Map<CommentGetViewModel>(comment)).ToList(),
                    comments.TotalPages,
                    comments.PageNumber
);

                return View(commentsGetView);
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
            var commentViewModel = new CommentViewModel();

            return this.View(commentViewModel);
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
                    
                    return this.RedirectToAction("Index", "Comments", new { id = createdComment.PostId });
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

                return this.RedirectToAction("Index", "Comments", new { id = updatedComment.PostId });
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

                return this.RedirectToAction("Index", "Comments", new { id = comment.PostId });
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
                return this.RedirectToAction("Index", "Comments", new { id = comment.PostId }); 
			}
			catch (EntityNotFoundException ex)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
			}
		}

		[HttpGet]
		public IActionResult CreateReply()
		{
			var commentViewModel = new CommentViewModel();
			return this.View(commentViewModel);
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
                        Content =$"\"{comment.Content}\" - " + $" {commentViewModel.Content}",
                        PostId = comment.PostId,
                        CommentId = comment.Id
					};
					var commentReply = this.mapper.Map<Comment>(commentReplyCreateViewModel);

                    var username = this.HttpContext.Session.GetString("LoggedUser");
                    var user = authManager.TryGetUserByUsername(username);
                    var createdComment = this.commentService.Create(commentReply, user);
					return this.RedirectToAction("Index", "Comments", new { id = createdComment.PostId });
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
