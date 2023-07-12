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
using System.Security.Cryptography;
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
        public IActionResult Index([FromQuery] CommentQueryParameters parameters)
        {
            try
            {
                if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
                {
                    return UnLoggedErrorView();
                }
                
                parameters.SortBy = "date";
                PaginatedList<Comment> comments = this.commentService.FilterBy(parameters);
                return View(comments);
            }
            catch (EntityNotFoundException ex)
            {
                return EntityErrorView(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Filter([FromQuery] CommentQueryParameters parameters)
        {
            try
            {
                if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
                {
                    return UnLoggedErrorView();
                }
                
                parameters.SortBy = "date";
                parameters.SortOrder = "desc";
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
            if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
            {
                return UnLoggedErrorView();
            }
            else if ((this.HttpContext.Session.GetString("IsBlocked")) == "True")
            {
                return BlockedErrorView();
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
                    var loggedUser = GetLoggedUser();
                    var comment = MapCreateComment(id, commentViewModel);
                    var createdComment = this.commentService.Create(comment, loggedUser);

                    return this.RedirectToAction("Index", "Comments", new { postID = createdComment.PostId });
                }
                else 
                {
                    return this.View(commentViewModel);
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
        
        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            try
            {
                if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
                {
                    return UnLoggedErrorView();
                }
                else
                {
                    var comment = this.commentService.GetByID(id);
                    var commentViewModel = this.mapper.Map<CommentViewModel>(comment);
                    return this.View(commentViewModel);
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
        public IActionResult Edit([FromRoute] int id, CommentViewModel commentViewModel)
        {
            try
            {

                if (!this.ModelState.IsValid)
                {
                    return View(commentViewModel);
                }
                
                var inputComment = MapEditComment(id, commentViewModel);
                var loggedUser = GetLoggedUser();
                var updatedComment = this.commentService.Update(id, inputComment, loggedUser);

                return this.RedirectToAction("Filter", "Comments", new { Username = updatedComment.CreatedBy.Username });
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

        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
                {
                    return UnLoggedErrorView();
                }
                else 
                { 
                    var comment = this.commentService.GetByID(id);
                    return this.View(comment);
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
                var comment = this.commentService.Delete(id, loggedUser);

                return this.RedirectToAction("Filter", "Comments", new { Username = comment.CreatedBy.Username });
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

        [HttpGet]
        public IActionResult Like([FromRoute] int id)
        {
            try
            {
                var username = this.HttpContext.Session.GetString("LoggedUser");
                var user = authManager.TryGetUserByUsername(username);
                Comment comment = commentService.GetByID(id);
                likeCommentService.Update(comment, user);
                return this.RedirectToAction("Index", "Comments", new { postId = comment.PostId});
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

        [HttpGet]
        public IActionResult CreateReply()
        {
            if ((this.HttpContext.Session.GetString("LoggedUser")) == null)
            {
                return UnLoggedErrorView();
            }
            else if((this.HttpContext.Session.GetString("IsBlocked")) == "True")
            {
                return BlockedErrorView();
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
                    var commentReply = MapReply(id, commentViewModel);
                    var loggedUser = GetLoggedUser();
                    var createdComment = this.commentService.Create(commentReply, loggedUser);
                    return this.RedirectToAction("Index", "Comments", new { postID = createdComment.PostId });
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

            return this.View(commentViewModel);
        }

        private Comment MapReply(int id, CommentViewModel commentViewModel)
        {
            var comment = commentService.GetByID(id);
            var commentReplyCreateViewModel = new CommentReplyCreateViewModel()
            {
                Content = $"\"{comment.CreatedBy.Username}: {comment.Content}\" - " + $" {commentViewModel.Content}",
                PostId = comment.PostId,
                CommentId = comment.Id
            };
            var commentReply = this.mapper.Map<Comment>(commentReplyCreateViewModel);
            return commentReply;
        }

        private Comment MapEditComment(int commentId, CommentViewModel commentViewModel)
        {
            var commentToUpdate = this.commentService.GetByID(commentId);
            var commentCreateViewModel = new CommentCreateViewModel()
            {
                PostId = commentToUpdate.PostId,
                Content = commentViewModel.Content
            };
            var inputComment = mapper.Map<Comment>(commentCreateViewModel);
            return inputComment;
        }
       
        private Comment MapCreateComment(int postId, CommentViewModel commentViewModel)
        {
            var commentCreateVireModel = new CommentCreateViewModel()
            {
                Content = commentViewModel.Content,
                PostId = postId
            };
            var comment = this.mapper.Map<Comment>(commentCreateVireModel);
            return comment;
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

        private IActionResult BlockedErrorView()
        {
              this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
              this.ViewData["ErrorMessage"] = "You are a \"BLOCKED USER\"!";
              return this.View("UnauthorizedError");
            
        }
        
        private IActionResult UnLoggedErrorView()
        {
            this.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            this.ViewData["ErrorMessage"] = "You are a \"UNLOGGED USER\"!";
            return this.View("UnauthorizedError");

        }
    }
}
