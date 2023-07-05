using AspNetCoreDemo.Models;
using Business.Exceptions;
using Business.Services.Helpers;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository repository;

        public CategoryService(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        public Category Create(Category category, User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedOperationException(Constants.ModifyCategoryErrorMessage);
            }

            try
            {
                var createCategory = this.repository.GetByName(category.Name);
            }
            catch (EntityNotFoundException)
            {
                var createCategory = this.repository.Create(category);
                return createCategory;  
            }
            throw new DuplicateEntityException(Constants.CategoryExistingErrorMessage);
        }

        public Category Delete(int id,User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedOperationException(Constants.ModifyCategoryErrorMessage);
            }
            return repository.Delete(id);
        }

        public PaginatedList<Category> FilterBy(CategoryQueryParameter parameters)
        {
            return this.repository.FilterBy(parameters);
        }

        public List<Category> GetAll()
        {
            return this.repository.GetAll();
        }

        public Category GetById(int id)
        {
            return this.repository.GetById(id);
        }

        public Category Update(int id, Category category, User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedOperationException(Constants.ModifyCategoryErrorMessage);
            }
            try
            {
                Category categoryToUpdate = this.repository.GetByName(category.Name);
                if (id!=categoryToUpdate.Id)
                {
                    throw new DuplicateEntityException(Constants.CategoryExistingErrorMessage);
                }
                return this.repository.Update(categoryToUpdate.Id, category);
            }
            catch (EntityNotFoundException)
            {
               return this.repository.Create(category);
            }
        }
    }
}
