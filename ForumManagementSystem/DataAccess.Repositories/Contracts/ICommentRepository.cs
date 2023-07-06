using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public interface ICommentRepository
    {
        List<Comment> GetAll();

        Comment GetByID(int id);

        Comment GetByUser(User user);

        PaginatedList<Comment> FilterBy(CommentQueryParameters parameters);

        Comment Create(Comment comment, User user);

        Comment Update(int id, Comment comment);

        Comment Delete(int id);
    }
}
