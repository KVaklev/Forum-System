using Business.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{


    public class CommentService : ICommentService
    {

        private const string ModifyCommentErrorMessage = "Only an admin or unblock user can create or modify a comment.";
        private readonly ICommentRepository repository;

        public CommentService(ICommentRepository repository)
        {
            this.repository = repository;
        }

        public Comment Create(Comment comment, User user)
        {
            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException(ModifyCommentErrorMessage);
            }
            return this.repository.Create(comment, user);
        }

        public Comment Delete(int id, User user)
        {
            Comment CommentToUpdate = this.repository.GetByID(id);
            if (CommentToUpdate.UserId != user.Id && !user.IsAdmin && user.IsBlocked)
            {
                throw new UnauthorizedOperationException(ModifyCommentErrorMessage);
            }
            return this.repository.Delete(id);
        }

        public List<Comment> FilterBy(CommentQueryParameters parameters)
        {
            return this.repository.FilterBy(parameters);
        }

        public List<Comment> GetAll()
        {
            return this.repository.GetAll();
        }

        public Comment GetByID(int id)
        {
            return this.repository.GetByID(id);
        }

        public Comment GetByUser(User user)
        {
            return this.repository.GetByUser(user);
        }

        public Comment Update(int id, Comment comment, User user)
        {
            Comment CommentToUpdate = this.repository.GetByID(id);
            if (CommentToUpdate.UserId!=user.Id && !user.IsAdmin && user.IsBlocked)
            {
                throw new UnauthorizedOperationException(ModifyCommentErrorMessage);
            }
            return this.repository.Update(id, comment);
        }
    }
}
