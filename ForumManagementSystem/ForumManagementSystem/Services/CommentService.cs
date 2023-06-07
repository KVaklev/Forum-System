using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{


    public class CommentService : ICommentService
    {
        private readonly ICommentRepository repository;

        public CommentService(ICommentRepository repository)
        {
            this.repository = repository;
        }

        public Comment Create(Comment comment)
        {
            return this.repository.Create(comment);
        }

        public Comment Delete(int id)
        {
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

        public Comment Update(int id, Comment comment)
        {
            return this.repository.Update(id, comment);
        }
    }
}
