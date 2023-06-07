using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly List<Comment> comments;
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;

        public CommentRepository(IUserRepository userRepository, IPostRepository postRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;

            this.comments = new List<Comment>()
            {
                new Comment()
                {
                    Id= 1,
                    User=this.userRepository.GetById(1),
                    Post = this.postRepository.GetById(1),
                    Content = "The best town!",
                    DateTime = DateTime.Now
                },
                new Comment()
                {
                    Id= 2,
                    User=this.userRepository.GetById(1),
                    Post = this.postRepository.GetById(1),
                    Content = "The worst town!",
                    DateTime = DateTime.Now
                }
            };
        }
        public Comment Create(Comment comment)
        {
            comment.Id = this.comments.Count + 1;
            this.comments.Add(comment);
            return comment;
        }

        public Comment Delete(int id)
        {
            Comment commentToDelete = this.GetByID(id);
            this.comments.Remove(commentToDelete);
            return commentToDelete;
        }
        //TODO
        public List<Comment> FilterBy(CommentQueryParameters parameters)
        {
            List<Comment> result = this.comments;

            if (!string.IsNullOrEmpty(parameters.User.Username))
            {
                result = result.FindAll(comment => comment.User.Username.Contains(parameters.User.Username));
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
        //TODO
        public Comment GetByUser(User user)
        {
            var comment = this.comments.FirstOrDefault(comment => comment.User.Equals(user));
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
