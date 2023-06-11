using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public interface IPostRepository
    {
        List<Post> GetAll();

        Post GetById(int id);

        Post GetByUser(User user);

        Post GetByTitle(string title);

        Post GetByCategory(string category);

        List<Post> FilterBy(PostQueryParameters parameters);

        Post Create(Post post, User user);

        Post Update(int id, Post post);

        Post Delete(int id);

        Category GetByCategoryId(int categoryId);

        User GetByUserId(int userId);

    }
}
