using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface IPostService
    {
        List<Post> GetAll();
        PaginatedList<Post> GetTopTenCommented(PostQueryParameters queryParameters);
        PaginatedList<Post> GetLastTenCommented(PostQueryParameters queryParameters);
        PaginatedList<Post> FilterBy(PostQueryParameters filterParameters);
        Post GetById(int id);
        Post GetByUser(User user);
        Post Create(Post post, User user, string tagNames);
        Post Update(int id, Post post, User loggedUser, string tagNames);
        void Delete(int id, User loggedUser);
        bool IsAuthorized(User user, User loggedUser);

    }
}
