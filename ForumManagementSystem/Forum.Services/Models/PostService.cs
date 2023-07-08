using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using Business.Services.Helpers;
using DataAccess.Repositories.Contracts;
using AspNetCoreDemo.Models;

namespace ForumManagementSystem.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository repository;
        private readonly ITagService tagService;
        private readonly ILikePostRepository likePostRepository;
        private readonly ICategoryRepository categoryRepository;

        public PostService(
            IPostRepository repository,
            ITagService tagService,
            ILikePostRepository likePostRepository,
            ICategoryRepository categoryRepository)
        {
            this.repository = repository;
            this.tagService = tagService;
            this.likePostRepository = likePostRepository;
            this.categoryRepository = categoryRepository;
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
        public PaginatedList<Post> FilterBy(PostQueryParameters filterParameters)
        {
            return this.repository.FilterBy(filterParameters);
        }

        public Post Create(Post post, User user, string tags)
        {
            string action = "Create";

            var tagsToAdd = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(tags))
            {
                tagsToAdd = tags.Split().ToList();
            }
                       
            CheckIfBlocked(user,action);

            Post createdPost = this.repository.Create(post, user);
            
            IncreasePostCount(post);

            AddTags(createdPost, tagsToAdd);

            return createdPost;

        }

        public Post Update(int id, Post post, User loggedUser, string tags)
        {
            string action = "Update";
            var tagsToAdd = new List<string>();

            if(!string.IsNullOrWhiteSpace(tags))
            {
                tagsToAdd = tags.Split().ToList();
            }
            
            Post postToUpdate = this.repository.GetById(id);

            if (!IsAuthorized(postToUpdate.CreatedBy, loggedUser))
            {
                throw new UnauthorizedOperationException(Constants.ModifyPostErrorMessage);
            }

            CheckIfBlocked(loggedUser,action);

            Post updatedPost = this.repository.Update(id, post);

            updatedPost.PostTags.Clear();

            AddTags(updatedPost, tagsToAdd);

            return updatedPost;
        }

        public void Delete(int id, User loggedUser)
        {
            string action = "Delete";

            Post postToDelete = repository.GetById(id);

            if (!IsAuthorized(postToDelete.CreatedBy, loggedUser))
            {
                throw new UnauthorizedOperationException(Constants.ModifyPostErrorMessage);
            }

            CheckIfBlocked(loggedUser, action);

            DecreasePostCount(postToDelete);
            this.repository.Delete(id);
        }

        public void CheckIfBlocked(User user,string action)
        {
            if (user.IsBlocked && action=="Create")
            {
                throw new UnauthorizedOperationException(Constants.ModifyPostErrorMessageIfUserIsBlocked);
            }

            if (user.IsBlocked && action == "Update")
            {
                throw new UnauthorizedOperationException(Constants.ModifyPostErrorMessage);
            }

            if (user.IsBlocked && action == "Delete")
            {
                throw new UnauthorizedOperationException(Constants.ModifyPostErrorMessage);
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
        public int IncreasePostCount(Post post)
        {
            Category category = this.categoryRepository.GetById(post.CategoryId);
            category.CountPosts++;
            return category.CountPosts;
        }
        public int DecreasePostCount(Post post)
        {
            Category category = this.categoryRepository.GetById(post.CategoryId);
            category.CountPosts--;
            return category.CountPosts;
        }

        public Post AddTags(Post newlyCreatedPost, List<string> tagsToAdd)
        {
            if (tagsToAdd == null)
            {
                return newlyCreatedPost;
            }
            else
            {
                foreach (var name in tagsToAdd)
                {

                    Tag tag = this.tagService.Create(name);

                    this.repository.AddTagToPost(tag.Id, newlyCreatedPost.Id);
                }
            }

            return newlyCreatedPost;
        }

      
    }
}
