using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public interface IPostRepository
    {
        List<Post> GetAll();

        Post GetByID(int id);

        Post GetByUser(string user);

        Post GetByTitle(string title);

        Post GetByCategory(string category);

        List<Post> FilterBy(PostQueryParameters parameters);

        Post Create(Post post);

        Post Update(int id, Post post);

        Post Delete(int id);

    }
}
