using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly CategoryMapper categoryMapper;
        private readonly AuthManager authManager;

        public CategoryApiController
            (ICategoryService categoryService, 
             CategoryMapper categoryMapper, 
             AuthManager authManager)
        {
            this.categoryService = categoryService;
            this.categoryMapper = categoryMapper;
            this.authManager = authManager;
        }

        [HttpGet("")]
        public IActionResult GetCategories([FromQuery] CategoryQueryParameter parameters)
        {
            List<Category> result = this.categoryService.FilterBy(parameters);

            return this.StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("categories/{id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                var category = this.categoryService.GetById(id);

                return this.StatusCode(StatusCodes.Status200OK, category);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }

        [HttpPost("")]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryDto, [FromHeader] string credentials)
        {
            try
            {
                Category category = this.categoryMapper.Map(categoryDto);
                User user = this.authManager.TryGetUser(credentials);
                var createdCategory = this.categoryService.Create(category, user);
                return this.StatusCode(StatusCodes.Status201Created, createdCategory);
            }
            catch (DuplicateEntityException ex)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }
        [HttpPut("categories/{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDTO categoryDto, [FromHeader] string credentials)
        {
            try
            {
                Category category = this.categoryMapper.Map(categoryDto);
                User user = this.authManager.TryGetUser(credentials);
                Category updatedCategory = this.categoryService.Update(id, category,user);
                return this.StatusCode(StatusCodes.Status200OK, updatedCategory);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (DuplicateEntityException ex)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }

        [HttpDelete("categories/{id}")]
        public IActionResult DeleteCategory(int id, [FromHeader] string credentials)
        {
            try
            {
                User user = this.authManager.TryGetUser(credentials);
                var deletedCategory = this.categoryService.Delete(id, user);

                return this.StatusCode(StatusCodes.Status200OK, deletedCategory);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }

    }
}
