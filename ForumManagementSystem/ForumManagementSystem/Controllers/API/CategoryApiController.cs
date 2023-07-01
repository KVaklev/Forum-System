using AutoMapper;
using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers.API
{
    [ApiController]
    [Route("api/category")]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IAuthManager authManager;

        public CategoryApiController(ICategoryService categoryService, IMapper mapper, IAuthManager authManager)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.authManager = authManager;
        }

        [HttpGet("")]
        public IActionResult GetCategories([FromQuery] CategoryQueryParameter parameters)
        {
            try
            {
                List<Category> result = categoryService.FilterBy(parameters);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }

        [HttpGet("categories/{id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                var category = categoryService.GetById(id);
                return StatusCode(StatusCodes.Status200OK, category);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }

        [HttpPost("")]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto, [FromHeader] string credentials)
        {
            try
            {
                Category category = mapper.Map<Category>(categoryDto);
                User user = authManager.TryGetUser(credentials);
                var createdCategory = categoryService.Create(category, user);
                return StatusCode(StatusCodes.Status201Created, createdCategory);
            }
            catch (DuplicateEntityException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, ex.Message);
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
        [HttpPut("categories/{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDto categoryDto, [FromHeader] string credentials)
        {
            try
            {
                Category category = mapper.Map<Category>(categoryDto);
                User user = authManager.TryGetUser(credentials);
                Category updatedCategory = categoryService.Update(id, category, user);
                return StatusCode(StatusCodes.Status200OK, updatedCategory);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (DuplicateEntityException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, ex.Message);
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

        [HttpDelete("categories/{id}")]
        public IActionResult DeleteCategory(int id, [FromHeader] string credentials)
        {
            try
            {
                User user = authManager.TryGetUser(credentials);
                var deletedCategory = categoryService.Delete(id, user);

                return StatusCode(StatusCodes.Status200OK, deletedCategory);
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

    }
}
