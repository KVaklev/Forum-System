using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using Business.Services.Helpers;
using DataAccess.Repositories.Contracts;

namespace ForumManagementSystem.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository repository;
        private readonly ITagService tagService;
        private readonly ILikePostRepository likePostRepository;
        public PostService(IPostRepository repository, ITagService tagService, ILikePostRepository likePostRepository)
        {
            this.repository = repository;
            this.tagService = tagService;
            this.likePostRepository = likePostRepository;
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
        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            return this.repository.FilterBy(filterParameters);
        }

        public Post Create(Post post, User user, List<string> tagsToAdd)
        {
            CheckIfBlocked(user);

            if (this.repository.TitleExists(post.Title))
            {
                throw new DuplicateEntityException($"Post with title '{post.Title}' already exists.");
            }

            Post createdPost = this.repository.Create(post, user);

            if (tagsToAdd == null)
            {
                return createdPost;
            }
            else
            {
                foreach (var name in tagsToAdd)
                {

                    Tag tag = this.tagService.Create(name, user);

                    this.repository.AddTagToPost(tag.Id, createdPost.Id);
                }
            }

            return createdPost;
        }

        public Post Update(int id, Post post, User loggedUser, List<string> tagsToAdd)
        {
            Post postToUpdate = this.repository.GetById(id);

            if (!IsAuthorized(postToUpdate.CreatedBy, loggedUser))
            {
                throw new UnauthenticatedOperationException(Constants.ModifyPostErrorMessage);
            }
         
            if (this.repository.TitleExists(post.Title))
            {
                throw new DuplicateEntityException($"Post with title '{post.Title}' already exists.");
            }

            Post updatedPost = this.repository.Update(id, post);

            updatedPost.PostTags.Clear();

            foreach (var name in tagsToAdd)
            {
                Tag tag = this.tagService.Create(name, loggedUser);

                this.repository.AddTagToPost(tag.Id, updatedPost.Id);
            }
            
            return updatedPost;
        }

        public void Delete(int id, User loggedUser)
        {
            Post postToDelete = repository.GetById(id);

            if (!IsAuthorized(postToDelete.CreatedBy, loggedUser))
            {
                throw new UnauthenticatedOperationException(Constants.ModifyPostErrorMessage);
            }

            this.repository.Delete(id);
        }

        public void CheckIfBlocked(User user)
        {
            if (user.IsBlocked)
            {
                throw new UnauthorizedAccessException(Constants.ModifyPostErrorMessageIfUserIsBlocked);
            }
        }

        public bool IsAuthorized(User user, User loggedUser)
        {
            bool isAuthorized = false;

            if (user.Equals(loggedUser) || loggedUser.IsAdmin)
            {
                isAuthorized = true;
            }
            return isAuthorized;
        }
    }
}
