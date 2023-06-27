using AutoMapper;
using Business.Dto;
using Business.Exceptions;
using Business.Services.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.API
{
    [ApiController]
    [Route("api/posts")]
    public class PostsApiController : ControllerBase
    {

        private readonly IPostService postService;
        private readonly IMapper mapper;
        private readonly IAuthManager authManager;
        private readonly ILikePostService likePostService;

        public PostsApiController(IPostService postService, IMapper mapper, IAuthManager authManager, ILikePostService likePostService)

        {
            this.postService = postService;
            this.mapper = mapper;
            this.authManager = authManager;
            this.likePostService = likePostService;
        }

        [HttpGet("")]
        public IActionResult GetPosts([FromQuery] PostQueryParameters filterParameters, [FromHeader] string credentials)
        {

            try
            {
                User user = authManager.TryGetUser(credentials);

                List<Post> result = postService.FilterBy(filterParameters);

                List<GetPostDto> getPostDto = result.Select(post => mapper.Map<GetPostDto>(post)).ToList();

                return StatusCode(StatusCodes.Status200OK, getPostDto);
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
        public IActionResult GetPostById(int id, [FromHeader] string credentials)
        {
            try
            {
                Post post = postService.GetById(id);

                User user = authManager.TryGetUser(credentials);

                GetPostDto getPostDto = mapper.Map<GetPostDto>(post);

                return StatusCode(StatusCodes.Status200OK, getPostDto);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult CreatePost([FromBody] CreatePostDto createPostDto, [FromHeader] string credentials)
        {
            try
            {
                User user = authManager.TryGetUser(credentials);

                Post post = mapper.Map<Post>(createPostDto);

                Post createdPost = postService.Create(post, user, createPostDto.Tags);

                return StatusCode(StatusCodes.Status201Created, createdPost);
            }
            catch (DuplicateEntityException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }

            catch (UnauthenticatedOperationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] CreatePostDto createPostDto, [FromHeader] string credentials)
        {
            try
            {
                User loggedUser = authManager.TryGetUser(credentials);

                Post post = mapper.Map<Post>(createPostDto);

                Post updatedPost = postService.Update(id, post, loggedUser, createPostDto.Tags);

                return StatusCode(StatusCodes.Status200OK, updatedPost);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (UnauthenticatedOperationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id, [FromHeader] string credentials)
        {
            try
            {
                User user = authManager.TryGetUser(credentials);

                postService.Delete(id, user);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (UnauthenticatedOperationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

        [HttpPut("{id}/like")]

        public IActionResult LikeDislikePost(int id, [FromHeader] string credentials)
        {
            try
            {
                User loggedUser = authManager.TryGetUser(credentials);
                Post post = postService.GetById(id);
                likePostService.Update(post, loggedUser);

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