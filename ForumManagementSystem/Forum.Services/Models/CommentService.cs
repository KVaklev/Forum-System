using Business.Exceptions;
using Business.Services.Helpers;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository repository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IPostRepository postRepository;
        private readonly ILikeCommentRepository likeRepository;

        public CommentService(
            ICommentRepository repository, 
            ILikeCommentRepository likeRepository,
            ICategoryRepository categoryRepository,
            IPostRepository postRepository)
        {
            this.repository = repository;
            this.likeRepository = likeRepository;
            this.categoryRepository = categoryRepository;
            this.postRepository = postRepository;
        }

        public Comment Create(Comment comment, User user)
        {
            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException(Constants.ModifyCommentErrorMessage);
            }
            IncreaseComentCount(comment);
            return this.repository.Create(comment, user);
        }

        public Comment Delete(int id, User user)
        {
            if (!IsUserUnauthorized(id,user))
            {
                throw new UnauthorizedOperationException(Constants.ModifyCommentErrorMessage);
            }
            Comment comment = repository.GetByID(id);
            likeRepository.DeleteByComment(comment);
            DecreaseComentCount(comment);
            return this.repository.Delete(id);
        }

        public List<Comment> FilterBy(CommentQueryParameters parameters)
        {
            
            return this.repository.FilterBy(parameters);
        }

        public Comment GetByID(int id)
        {
            return this.repository.GetByID(id);
        }

        public Comment Update(int id, Comment comment, User user)
        {
            if (!IsUserUnauthorized(id,user))
            {
                throw new UnauthorizedOperationException(Constants.ModifyCommentErrorMessage);
            }
            return this.repository.Update(id, comment);
            
        }

        public bool IsUserUnauthorized(int id, User user)
        {
            bool isUserUnauthorized = true;
            if (user.IsBlocked)
            {
                isUserUnauthorized = false;
            }
            Comment CommentToUpdate = this.repository.GetByID(id);

            if (CommentToUpdate.UserId != user.Id && !user.IsAdmin)
            {
                isUserUnauthorized = false;
            }
            return isUserUnauthorized;
        }
        public int IncreaseComentCount(Comment comment)
        {
            Post post = this.postRepository.GetById(comment.PostId);
            Category category = this.categoryRepository.GetById(post.CategoryId);
            return category.CountComment++;
        }

        public int DecreaseComentCount(Comment comment)
        {
            Post post = this.postRepository.GetById(comment.PostId);
            Category category = this.categoryRepository.GetById(post.CategoryId);
            return category.CountComment--;
        }
    }
}
