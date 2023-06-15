using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext context;
        
        public CategoryRepository(ApplicationContext context)
        {
            this.context = context;        
        }
        public Category Create(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();

            return category;
        }

        public Category Delete(int id)
        {
            var category = GetById(id);
            context.Categories.Remove(category);
            context.SaveChanges();

            return category;
        }

        public List<Category> FilterBy(Models.CategoryQueryParameter parameters)
        {
            List<Category> result = context.Categories.ToList();

            if (!string.IsNullOrEmpty(parameters.Name))
            {
                result = result.FindAll(c => c.Name.Contains(parameters.Name));
            }
            if (!string.IsNullOrEmpty(parameters.Description))
            {
                result = result.FindAll(c => c.Name.Contains(parameters.Description));
            }
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy.Equals("name", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.Name).ToList();
                }
                else if (parameters.SortBy.Equals("description", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.Description).ToList();
                }
                if (!string.IsNullOrEmpty(parameters.SortOrder) 
                    && parameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }

            return result;
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            return category ?? throw new EntityNotFoundException($"Category with id={id} doesn't exist.");
        }
        public Category GetByName(string name)
        {
            var category = context.Categories.Where(c => c.Name == name).FirstOrDefault();
            return category ?? throw new EntityNotFoundException($"Category with name={name} doesn't exist.");
        }

        public Category Update(int id, Category category)
        {
            var categoryToUpdate = GetById(id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.DateTime = DateTime.Now;
            context.SaveChanges();

            return categoryToUpdate;
        }
    }
}
