using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface IPostService
    {
        List<Post> GetAll();
        List<Post> FilterBy(PostQueryParameters filterParameters);
        Post GetById(int id);
        Post GetByUser(User user);
        Post Create(Post post, User user, List<string> tagNames);
        Post Update(int id, Post post, User loggedUser, List<string> tagNames);
        void Delete(int id, User loggedUser);
        bool IsAuthorized(User user, User loggedUser);

    }
}
