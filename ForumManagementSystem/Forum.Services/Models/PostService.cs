using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using System.Reflection.Metadata.Ecma335;

namespace ForumManagementSystem.Services
{
    public class PostService : IPostService
    {
        private const string ModifyPostErrorMessage = "Only an admin or post creator can modify a post.";
        private const string ModifyPostErrorMessageIfUserIsBlocked = "Blocked user cannot create a post.";

        private readonly IPostRepository repository;
        private readonly ITagService tagService;
        public PostService(IPostRepository repository, ITagService tagService)
        {
            this.repository = repository;
            this.tagService = tagService;
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

        public Post Create(Post post, User user, List<string> tagsToAdd) 
        {

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

            if (postToUpdate.UserId != id && !loggedUser.IsAdmin)
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
            Post post = repository.GetById(id);

            if (!loggedUser.Equals(post.CreatedBy) || !loggedUser.IsAdmin || loggedUser.IsBlocked)
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
