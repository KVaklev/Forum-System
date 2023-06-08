using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumManagementSystem.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsApiController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly PostMapper postMapper; 

        public PostsApiController (IPostService postService, PostMapper postMapper)
        {
            this.postService = postService;
            this.postMapper = postMapper;
        }

        [HttpGet("")]
        public IActionResult GetPosts([FromQuery] PostQueryParameters filterParameters)
        {
            List<Post> result = this.postService.FilterBy(filterParameters);

            return this.StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            try
            {
                Post post = this.postService.GetById(id);

                return this.StatusCode(StatusCodes.Status200OK, post);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult CreatePost([FromBody] PostDto postDto)
        {
            try
            {
                Post post = this.postMapper.Map(postDto);
                Post createdPost = this.postService.Create(post);

                return this.StatusCode(StatusCodes.Status201Created, createdPost);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] PostDto postDto)
        {
            try
            {
                Post post = this.postMapper.Map(postDto);
                Post updatedPost = this.postService.Update(id, post);

                return this.StatusCode(StatusCodes.Status200OK, updatedPost);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var deletedPost = this.postService.Delete(id);

                return this.StatusCode(StatusCodes.Status200OK, deletedPost);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

    }
}