using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumManagementSystem.Controllers
{
    [Route($"api/comments")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        private ICommentService commentService;
        private readonly CommentMapper commentMapper;

        public CommentApiController(ICommentService commentService, CommentMapper commentMapper)
        {
            this.commentService = commentService;
            this.commentMapper = commentMapper;
        }

        [HttpGet("")]
        public IActionResult GetComment([FromQuery] CommentQueryParameters filterParameters)
        {
            List<Comment> result = this.commentService.FilterBy(filterParameters);

            return this.StatusCode(StatusCodes.Status200OK, result);
        }
        [HttpGet("{id}")]
        public IActionResult GetCommentById(int id)
        {
            try
            {
                Comment comment = this.commentService.GetByID(id);

                return this.StatusCode(StatusCodes.Status200OK, comment);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }

        [HttpPost("")]
        public IActionResult CreateComment([FromBody] CommentDto commentDto)
        {
            try
            {
                Comment comment = this.commentMapper.Map(commentDto);
                Comment createdComment = this.commentService.Create(comment);

                return this.StatusCode(StatusCodes.Status201Created, createdComment);
            }
            catch (DuplicateEntityException ex)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, [FromBody] CommentDto commentDto)
        {
            try
            {
                Comment comment = this.commentMapper.Map(commentDto);
                Comment updatedComment = this.commentService.Update(id, comment);

                return this.StatusCode(StatusCodes.Status200OK, updatedComment);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            try
            {
                var deletedComment = this.commentService.Delete(id);

                return this.StatusCode(StatusCodes.Status200OK, deletedComment);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }
    }
}
