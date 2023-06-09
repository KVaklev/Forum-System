using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private const string ModifyCategoryErrorMessage = "Only an admin can modify a category.";
        private readonly ICategoryRepository repository;

        public CategoryService(ICategoryRepository repository)
        {
            this.repository = repository;
        }
        public Category Create(Category category, User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyCategoryErrorMessage);
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
            throw new DuplicateEntityException($"Category {category.Name} already exists.");
        }

        public Category Delete(int id,User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyCategoryErrorMessage);
            }
            return repository.Delete(id);
        }

        public List<Category> FilterBy(CategoryQueryParameter parameters)
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
                throw new UnauthorizedOperationException(ModifyCategoryErrorMessage);
            }

            bool duplicateExists = true;
            try
            {
                Category existingCategory = this.repository.GetByName(category.Name);

                if (existingCategory.Id == id)
                {
                    duplicateExists = false;
                }
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Category {category.Name} already exists.");
            }

            Category updatedCategory = this.repository.Update(id, category);

            return updatedCategory;
        }
    }
}
