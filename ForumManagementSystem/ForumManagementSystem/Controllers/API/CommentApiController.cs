using AutoMapper;
using Business.Dto;
using Business.Exceptions;
using Business.Services.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using System.Net;

namespace ForumManagementSystem.Controllers.API
{
    [Route("api/comments")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        private ICommentService commentService;
        private readonly IMapper mapper;
        private readonly IAuthManager authManager;
        private readonly ILikeCommentService likeCommentService;

        public CommentApiController(
            ICommentService commentService,
            IMapper mapper,
            IAuthManager authManager,
            ILikeCommentService likeCommentService)
        {
            this.commentService = commentService;
            this.mapper = mapper;
            this.authManager = authManager;
            this.likeCommentService = likeCommentService;
        }

        [HttpGet("")]
        public IActionResult GetComment([FromQuery] CommentQueryParameters filterParameters, [FromHeader] string credentials)
        {
            try
            {
                User user = authManager.TryGetUser(credentials);
                List<Comment> result = commentService.FilterBy(filterParameters);
                List<RequireCommentDto> commentDto = result
                    .Select(comment => mapper.Map<RequireCommentDto>(comment)).ToList();

                return StatusCode(StatusCodes.Status200OK, commentDto);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }

        }

        [HttpGet("{id}")]
        public IActionResult GetCommentById(int id, [FromHeader] string credentials)
        {
            try
            {
                Comment comment = commentService.GetByID(id);
                User user = authManager.TryGetUser(credentials);
                RequireCommentDto commentDto = mapper.Map<RequireCommentDto>(comment);
                return StatusCode(StatusCodes.Status200OK, commentDto);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }



        [HttpPost("")]
        public IActionResult CreateComment([FromBody] CommentDto commentDto, [FromHeader] string credentials)
        {
            try
            {
                Comment comment = mapper.Map<Comment>(commentDto);
                User user = authManager.TryGetUser(credentials);
                Comment createdComment = commentService.Create(comment, user);

                return StatusCode(StatusCodes.Status201Created, createdComment);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, [FromBody] CommentDto commentDto, [FromHeader] string credentials)
        {
            try
            {
                Comment comment = mapper.Map<Comment>(commentDto);
                User user = authManager.TryGetUser(credentials);
                Comment updatedComment = commentService.Update(id, comment, user);

                return StatusCode(StatusCodes.Status200OK, updatedComment);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id, [FromHeader] string credentials)
        {
            try
            {
                User user = authManager.TryGetUser(credentials);
                var deletedComment = commentService.Delete(id, user);

                return StatusCode(StatusCodes.Status200OK, deletedComment);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }
        [HttpPut("{id}/like")]
        public IActionResult LikeDislikeComment(int id, [FromHeader] string credentials)
        {
            try
            {
                User userLoger = authManager.TryGetUser(credentials);
                Comment comment = commentService.GetByID(id);
                likeCommentService.Update(comment, userLoger);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }
    }
}
