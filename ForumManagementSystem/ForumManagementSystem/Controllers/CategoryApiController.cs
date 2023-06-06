using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumManagementSystem.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {

        private readonly ICategoryServices categoryService;
        public CategoryApiController(ICategoryServices categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult GetCategories([FromQuery] CategoryQueryParameters parameters)
        {
            List<Category> result = this.categoryService.FilterBy(parameters);

            return this.StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("{id}")]
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
        public IActionResult CreateCategory([FromBody] Category category)
        {
            try
            {
                
                var createdCategory= this.categoryService.Create(category);
                return this.StatusCode(StatusCodes.Status201Created, createdCategory);
            }
            catch (DuplicateEntityException ex)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            try
            {
                Category updatedCategory = this.categoryService.Update(id, category);
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
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var deletedCategory = this.categoryService.Delete(id);

                return this.StatusCode(StatusCodes.Status200OK, deletedCategory);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }

    }
}
