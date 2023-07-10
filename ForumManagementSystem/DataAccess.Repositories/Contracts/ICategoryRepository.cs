using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();

        Category GetById(int id);

        Category GetByName(string name);

        PaginatedList<Category> FilterBy(CategoryQueryParameter parameters);

        Category Create(Category category);

        Category Update(int id, Category newCategory);

        Category Delete(int id);

        int IncreaseCategoryPostCount(Post post);

        int DecreaseCategoryPostCount(Post post);

    }
}
