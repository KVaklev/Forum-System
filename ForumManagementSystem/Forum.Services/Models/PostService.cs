using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using Business.Exceptions;

namespace ForumManagementSystem.Services
{
    public class PostService : IPostService
    {
        private const string ModifyPostErrorMessage = "Only an admin can modify a post.";
        private const string ModifyPostErrorMessageIfUserIsBlocked = "Blocked user cannot create a post.";
            
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

            bool duplicateExists = false;  // predpolagame che nqma takuv post
=======
            if (user.IsBlocked)
            {
                throw new UnauthorizedAccessException(ModifyPostErrorMessageIfUserIsBlocked);
            }
            bool duplicateExists = false;


            try
            {
                this.repository.GetByTitle(post.Title);
            }
            catch (EntityNotFoundException)

            {
                duplicateExists = true;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Post {post.Title} already exists.");
            }

            Post createdPost = this.repository.Create(post, user);

            return createdPost;
        }
                
        public Post Update(int id, Post post, User loggedUser)
        {
            Post postToUpdate = this.repository.GetById(id);

            if (!postToUpdate.CreatedBy.Equals(loggedUser) && !loggedUser.IsAdmin)
            {
                throw new UnauthenticatedOperationException(ModifyPostErrorMessage);
            }

            bool duplicateExists = false;

            try
            {
                this.repository.GetByTitle(post.Title);
            }
            catch (EntityNotFoundException)

            {
                duplicateExists = true;
            }
            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Post {post.Title} already exists.");
            }

            Post updatedPost = this.repository.Update(id, post);

            return updatedPost;
        }

        public void Delete(int id, User loggedUser)
        {
            Post post = repository.GetById(id);

            if (!loggedUser.Equals(post.CreatedBy) || !loggedUser.IsAdmin || loggedUser.IsBlocked )
            {
                throw new UnauthorizedOperationException(ModifyPostErrorMessage);
            }

            this.repository.Delete(id);
        }


        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            return this.repository.FilterBy(filterParameters);
        }
    }
}
