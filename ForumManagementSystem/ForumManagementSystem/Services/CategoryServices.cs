using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository repository;
        public CategoryServices(ICategoryRepository repository)
        {
            this.repository = repository;
        }
        public Category Create(Category category)
        {
            try
            {
                var createCategory = this.repository.GetByName(category.Name);
            }
            catch (EntityNotFoundException ex)
            {
                var createCategory = this.repository.Create(category);
                return createCategory;
               
            }
            throw new DuplicateEntityException($"Category {category.Name} already exists.");
        }

        public Category Delete(int id)
        {
            return repository.Delete(id);
        }

        public List<Category> FilterBy(CategoryQueryParameters parameters)
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

        public Category Update(int id, Category category)
        {
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
