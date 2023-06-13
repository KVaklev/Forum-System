using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using System.Net;

namespace ForumManagementSystem.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly List<Comment> comments;

        public CommentRepository()
        {
            

            this.comments = new List<Comment>()
            {
                new Comment()
                {
                    Id= 1,
                    UserId=1,
                    PostId = 1,
                    Content = "The best town!",
                    DateTime = DateTime.Now
                },
                new Comment()
                {
                    Id= 2,
                    UserId=2,
                    PostId = 2,
                    Content = "The worst town!",
                    DateTime = DateTime.Now
                }
            };
        }
        public Comment Create(Comment comment, User user)
        {
            comment.Id = this.comments.Count + 1;
            comment.UserId = user.Id;
            this.comments.Add(comment);
            return comment;
        }

        public Comment Delete(int id)
        {
            Comment commentToDelete = this.GetByID(id);
            this.comments.Remove(commentToDelete);
            return commentToDelete;
        }
        
        public List<Comment> FilterBy(CommentQueryParameters parameters)
        {
            List<Comment> result = this.comments;

            if (parameters.UserId.HasValue)
            {
                result = result.FindAll(comment => comment.UserId==parameters.UserId);
            }
            if (parameters.FromDateTime.HasValue)
            {
                result = result.FindAll(c => c.DateTime >= parameters.FromDateTime);
            }
            if (parameters.ToDateTime.HasValue)
            {
                result = result.FindAll(c => c.DateTime <= parameters.ToDateTime);
            }
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy.Equals("userId", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.UserId).ToList();
                }
                else if (parameters.SortBy.Equals("date", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.DateTime).ToList();
                }
                if (!string.IsNullOrEmpty(parameters.SortOrder)
                    && parameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }
            return result;
        }

        public List<Comment> GetAll()
        {
            return this.comments;
        }

        public Comment GetByID(int id)
        {
            var comment = this.comments.FirstOrDefault(comment => comment.Id == id);
            return comment ?? throw new EntityNotFoundException($"Comment with ID = {id} doesn't exist.");
        }
        
        public Comment GetByUser(User user)
        {
            var comment = this.comments.FirstOrDefault(comment => comment.UserId==user.Id);
            return comment ?? throw new EntityNotFoundException($"Comment with username {user.Username} doesn't exist.");
        }

        public Comment Update(int id, Comment comment)
        {
            Comment commentToUpdate = this.GetByID(id);
            commentToUpdate.Content = comment.Content;
            commentToUpdate.DateTime = DateTime.Now;

            return commentToUpdate;
        }
    }
}
