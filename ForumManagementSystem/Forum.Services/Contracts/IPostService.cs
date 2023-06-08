using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface IPostService
    {
        List<Post> GetAll();
        List<Post> FilterBy(PostQueryParameters filterParameters);
        Post GetById(int id);
        Post Create(Post post);
        Post Update(int id, Post post);
        Post Delete(int id);
    }
}
