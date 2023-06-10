using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using Business.Exceptions;

namespace ForumManagementSystem.Services
{
    public class PostService : IPostService
    {
        private const string ModifyPostErrorMessage = "Only an admin can modify a post.";

        private readonly IPostRepository repository;
        public PostService(IPostRepository repository)
        {
            this.repository = repository;
        }
        public List<Post> GetAll()
        {
            return this.repository.GetAll();
        }
        public Post GetById(int id)
        {
            return this.repository.GetById(id);
        }
        public Post GetByUser(User user)
        {
            return this.repository.GetByUser(user);
        }

        public Post Create(Post post, User user)
        {
            bool duplicateExists = true;

            try
            {
                this.repository.GetByTitle(post.Title);
            }
            catch (EntityNotFoundException)

            {
                duplicateExists = false;
            }
            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Post {post.Title} already exists.");
            }

            Post createdPost = this.repository.Create(post, user);

            return createdPost;
        }
                
        public Post Update(int id, Post post, User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyPostErrorMessage);
            }

            bool duplicateExists = true;

            try
            {
                Post existingPost = this.repository.GetByTitle(post.Title);

                if (existingPost.Id == id)
                {
                    duplicateExists = false;
                }
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Post {post.Title} already exists.");
            }

            Post updatedPost = this.repository.Update(id, post);

            return updatedPost;
        }

        public Post Delete(int id, User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyPostErrorMessage);
            }
            return repository.Delete(id);
        }


        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            return this.repository.FilterBy(filterParameters);
        }
    }
}
