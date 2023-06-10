using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface IPostService
    {
        List<Post> GetAll();
        List<Post> FilterBy(PostQueryParameters filterParameters);
        Post GetById(int id);
        Post GetByUser(User user);
        Post Create(Post post, User user);
        Post Update(int id, Post post, User user);
        Post Delete(int id, User user);
    }
}
