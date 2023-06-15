using AutoMapper;
using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsApiController : ControllerBase
    {

        private readonly IPostService postService;
        private readonly IMapper mapper;
        private readonly AuthManager authManager;

        public PostsApiController(IPostService postService, IMapper mapper, AuthManager authManager)

        {
            this.postService = postService;
            this.mapper = mapper;
            this.authManager = authManager;
        }

        [HttpGet("")] // TO CREATE GETPOSTDTO AND ADD IT HERE
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
        public IActionResult CreatePost([FromBody] CreatePostDto createPostDto, [FromHeader] string credentials) 
        {
            try
            {
                User user = this.authManager.TryGetUser(credentials);

                Post post = this.mapper.Map<Post>(createPostDto);

                Post createdPost = this.postService.Create(post, user, createPostDto.Tags);

                return this.StatusCode(StatusCodes.Status201Created, createdPost);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }

            catch (UnauthenticatedOperationException e)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

       
        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] CreatePostDto createPostDto, [FromHeader] string credentials) 
        {
            try
            {
                User loggedUser = this.authManager.TryGetUser(credentials);

                Post post = this.mapper.Map<Post>(createPostDto);

                Post updatedPost = this.postService.Update(id, post, loggedUser, postDto.Tags);

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
            catch (UnauthenticatedOperationException e)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id, [FromHeader] string credentials)
        {
            try
            {
                User user = this.authManager.TryGetUser(credentials);

                this.postService.Delete(id, user);

                return this.StatusCode(StatusCodes.Status200OK);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (UnauthenticatedOperationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }


    }
}