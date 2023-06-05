using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface ICategoryServices
    {
        List<Category> GetAll();

        Category GetById(int id);

        List<Category> FilterBy(CategoryQueryParameters parameters);

        Category Create(Category category);

        Category Update(int id, Category category);

        Category Delete(int id);
    }
}
