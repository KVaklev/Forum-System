using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumManagementSystem.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITagRepository tagRepository;
        private readonly ApplicationContext context;

        public PostRepository(ApplicationContext context)
        {
           this.context = context;
        }

        public List<Post> GetAll()
        {
            return context.Posts.ToList();
        }

        public Post GetByUser(User user)
        {
            Post post = context.Posts.FirstOrDefault(post => post.CreatedBy.Id == user.Id);
            return post ?? throw new EntityNotFoundException($"Post with username {user.Username} doesn't exist.");
        }

        public Post GetById(int id)

        {
            Post post = context.Posts
                .Include(post =>post.CreatedBy)
                .Include(post=>post.PostTags)
                .ThenInclude(pt=>pt.Tag)
                .FirstOrDefault(p=>p.Id == id);
            return post ?? throw new EntityNotFoundException($"Post with ID = {id} doesn't exist.");

        }
        public Post GetByTitle(string title)
        {
            Post post = context.Posts.Where(posts => posts.Title == title).FirstOrDefault();

            if (post==null)
            {
                post= new Post { Title = title };
                return post;
            }
            else
            {
                throw new EntityNotFoundException($"Post with title = {title} already exist.");
            }
            
        }
        public Post GetByCategory(string categoryName)
        {
            Post post = context.Posts.Where(posts => posts.Category.Name == categoryName).FirstOrDefault();
            return post ?? throw new EntityNotFoundException($"Post with category = {categoryName} doesn't exist.");
        }

        public Post Update(int id, Post post)
        {
            Post postToUpdate = this.GetById(id);

            postToUpdate.Title = post.Title;
            postToUpdate.Content = post.Content;
            postToUpdate.PostTags = post.PostTags;
            postToUpdate.CategoryId = post.CategoryId;
            postToUpdate.UserId = post.UserId;
            context.SaveChanges();

            return postToUpdate;

        }

        public Post Create(Post post, User user)
        {
            post.CreatedBy = user;
            context.Posts.Add(post);
            context.SaveChanges();

            return post;
        }

        public Post Delete(int id)
        {
            Post postToDelete = this.GetById(id);
            context.Posts.Remove(postToDelete);
            context.SaveChanges();

            return postToDelete;
        }

        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            List<Post> result = context.Posts.ToList();

            if (filterParameters.Username != null && !string.IsNullOrEmpty(filterParameters.Username))
            {
                result = result.FindAll(post => post.CreatedBy.Username.Contains(filterParameters.Username));
            }          

            if (!string.IsNullOrEmpty(filterParameters.Title))
            {
                result = result.FindAll(post => post.Title.Contains(filterParameters.Title));
            }

            if (filterParameters.Category != null && !string.IsNullOrEmpty(filterParameters.Category))
            {
                result = result.FindAll(post => post.Category.Name.Contains(filterParameters.Category));
            }

            if (filterParameters.FromDateTime.HasValue)
            {
                result = result.FindAll(post => post.DateTime >= filterParameters.FromDateTime);
            }
            
            if (filterParameters.ToDateTime.HasValue)
            {
                result = result.FindAll(post => post.DateTime <= filterParameters.ToDateTime);
            }

            if (!string.IsNullOrEmpty(filterParameters.SortBy))
            {
                if (filterParameters.SortBy.Equals("user", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(p => p.CreatedBy).ToList();
                }
                else if (filterParameters.SortBy.Equals("title", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(p => p.Title).ToList();
                }
                else if (filterParameters.SortBy.Equals("category", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(p => p.Category).ToList();
                }
                else if (filterParameters.SortBy.Equals("fromDataTime", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(p => p.DateTime).ToList();
                }
                if (!string.IsNullOrEmpty(filterParameters.SortOrder) && filterParameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }
            return result;

        }

        public Category GetByCategoryId(int categoryId)
        {
            Category category=this.categoryRepository.GetById(categoryId);

            return category;
        }

        public User GetByUserId(int userId)
        {
           User user = this.userRepository.GetById(userId);
           
           return user;
        }

        public Tag GetByTag(string tagName)
        {
            Tag existingTag = this.tagRepository.GetByName(tagName);

            return existingTag;
        }

        public void AddTagToPost(int tagId, int postId)
        {
           PostTag postTag = new PostTag()
           {
               TagId = tagId,
               PostId = postId
           };
            Post post= this.GetById(postId);
            post.PostTags.Add(postTag);
            this.context.PostTags.Add(postTag);
            context.SaveChanges();
        }

        public bool TitleExists(string title)
        {
            return context.Posts.Any(p => p.Title == title);
        }
    }
}
