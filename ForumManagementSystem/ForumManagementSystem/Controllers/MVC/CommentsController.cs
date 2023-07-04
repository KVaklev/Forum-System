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
                    var user = authManager.TryGetUser("ivanchoDraganchov:123");
                    var createdComment = this.commentService.Create(comment, user);
                    return this.RedirectToAction("Index", "Posts", new { id = createdComment.Id });
               }
            }
            catch (EntityNotFoundException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }

            return this.View(commentViewModel);
        }
        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            try
            {
                var comment = this.commentService.GetByID(id);
                var commentCreateViewModel = this.mapper.Map<CommentCreateViewModel>(comment);
                               
                return this.View(commentCreateViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CommentCreateViewModel commentCreateViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(commentCreateViewModel);
            }
            var loggedUser = authManager.TryGetUser("ivanchoDraganchov:123");
            var comment = mapper.Map<Comment>(commentCreateViewModel);
            var updatedComment = this.commentService.Update(id, comment, loggedUser);

            return this.RedirectToAction("Index", "Posts", new { id = updatedComment.Id });
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
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed([FromRoute] int id)
        {
            try
            {
                var user = authManager.TryGetUser("ivanchoDraganchov:123");
                this.commentService.Delete(id, user);

                return this.RedirectToAction("Index", "Posts");
            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;

                return this.View("Error");
            }
        }

        [HttpGet]
        public IActionResult Like([FromRoute] int id)
        {
			try
			{
				User userLoger = authManager.TryGetUser("ivanchoDraganchov:123");
				Comment comment = commentService.GetByID(id);
				likeCommentService.Update(comment, userLoger);
                CommentQueryParameters parameter = new CommentQueryParameters()
                {
                    postID = comment.PostId
                };
                List<Comment> comments = this.commentService.FilterBy(parameter);

                List<CommentGetViewModel> commentsGetView = comments
                        .Select(comment => mapper.Map<CommentGetViewModel>(comment)).ToList();

                return View(commentsGetView);
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
					var user = authManager.TryGetUser("ivanchoDraganchov:123");
					var createdComment = this.commentService.Create(commentReply, user);
					return this.RedirectToAction("Index", "Posts", new { id = createdComment.Id });
				}
			}
			catch (EntityNotFoundException ex)
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = ex.Message;

				return this.View("Error");
			}

			return this.View(commentViewModel);
		}

	}
}
