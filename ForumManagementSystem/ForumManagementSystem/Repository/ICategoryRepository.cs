using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();

        Category GetById(int id);

        Category GetByName(string name);

        List<Category> FilterBy(CategoryQueryParameters parameters);

        Category Create(Category category);

        Category Update(int id,Category category);

        Category Delete(int id);

        


    }
}
