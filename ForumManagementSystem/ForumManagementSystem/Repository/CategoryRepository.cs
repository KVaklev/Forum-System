using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        List<Category> categories;

        public CategoryRepository()
        {
            this.categories = new List<Category>()
            {
                new Category
                 {
                    Id = 1,
                    Name = "VAT",
                    Description = "ЗДДС, ЗКПО, ЗДДФЛ",
                    DateTime = DateTime.Now
                 },
                new Category
                {
                    Id = 2,
                    Name = "ТРЗ и осигуряване",
                    Description = "КТ, КСО, Наредби, майчинство, болнични, обезщетения",
                    DateTime = DateTime.Now
                }
            };
        
        }
        public Category Create(Category category)
        {
            category.Id = categories.Count + 1;
            categories.Add(category);
            return category;
        }

        public Category Delete(int id)
        {
            var category = GetById(id);
            categories.Remove(category);
            return category;
        }

        public List<Category> FilterBy(CategoryQueryParameter parameters)
        {
            List<Category> result = categories;

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
            return categories;
        }

        public Category GetById(int id)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            return category ?? throw new EntityNotFoundException($"Category with id={id} doesn't exist.");
        }
        public Category GetByName(string name)
        {
            var category = categories.Where(c => c.Name == name).FirstOrDefault();
            return category ?? throw new EntityNotFoundException($"Category with name={name} doesn't exist.");
        }

        public Category Update(int id, Category category)
        {
            var categoryToUpdate = GetById(id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.DateTime = DateTime.Now;
            return categoryToUpdate;
        }
    }
}
