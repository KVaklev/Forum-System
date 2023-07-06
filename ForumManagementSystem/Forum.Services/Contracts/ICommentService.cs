using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface ICommentService
    {
        Comment GetByID(int id);

        PaginatedList<Comment> FilterBy(CommentQueryParameters parameters);

        Comment Create(Comment comment, User user);

        Comment Update(int id, Comment comment, User user);

        Comment Delete(int id, User user);

        bool IsUserUnauthorized(int id, User user);
    }
}
