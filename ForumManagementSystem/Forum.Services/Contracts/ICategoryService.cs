using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface ICategoryService
    {
        List<Category> GetAll();

        Category GetById(int id);

        List<Category> FilterBy(CategoryQueryParameter parameters);

        Category Create(Category category, User user);

        Category Update(int id, Category category, User user);

        Category Delete(int id, User user);
    }
}

