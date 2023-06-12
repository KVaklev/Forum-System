using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly List<Post> posts;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly User user;

        public PostRepository(IUserRepository userRepository, ICategoryRepository categoryRepository, User user)
        {
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
            this.user = user;


            posts = new List<Post>()

            {
                new Post
                {
                    Id = 1,
                    CreatedBy = this.userRepository.GetById(1),
                    Title = "BMW 5, 2007",
                    Category = this.categoryRepository.GetById(1),
                    Content = "Have you ever experienced issues when start the engine",
                    DateTime = DateTime.Now,
                    UserId = this.userRepository.GetById(1).Id
                },

                new Post
                {
                    Id = 2,
                    CreatedBy = this.userRepository.GetById(2),
                    Title = "Mercedes GLC, 2012",
                    Category = this.categoryRepository.GetById(2),
                    Content = "Have you experienced issues with suspension making strange noise",
                    DateTime = DateTime.Now,
                    UserId = this.userRepository.GetById(2).Id
                },

            };
        }


        public List<Post> GetAll()
        {
            return this.posts;
        }

        public Post GetByUser(User user)
        {
            Post post = this.posts.FirstOrDefault(post => post.CreatedBy.Id == user.Id);
            return post ?? throw new EntityNotFoundException($"Post with username {user.Username} doesn't exist.");
        }

        public Post GetById(int id)

        {
            Post post = this.posts.Where(posts => posts.Id == id).FirstOrDefault();
            return post ?? throw new EntityNotFoundException($"Post with ID = {id} doesn't exist.");

        }

        public Post GetByTitle(string title)
        {
            Post post = this.posts.Where(posts => posts.Title == title).FirstOrDefault();

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
            Post post = this.posts.Where(posts => posts.Category.Name == categoryName).FirstOrDefault();
            return post ?? throw new EntityNotFoundException($"Post with category = {categoryName} doesn't exist.");
        }

        public Post Update(int id, Post post)
        {
            Post postToUpdate = this.GetById(id);

            postToUpdate.Title = post.Title;
          
            postToUpdate.Content = post.Content;
          //  postToUpdate.Comments = post.Comments; to fix
            postToUpdate.CategoryId = post.CategoryId;
            Category category = this.GetByCategoryId(post.CategoryId);
            postToUpdate.Category = category;
            postToUpdate.UserId = post.UserId;

            return postToUpdate;

        }

        public Post Create(Post post, User user)
        {
            post.Id = this.posts.Count + 1;
            post.UserId = user.Id;
            post.CreatedBy = user;
            Category category = this.GetByCategoryId(post.CategoryId);
            post.Category= category;
            this.posts.Add(post);
            return post;
        }

        public Post Delete(int id)
        {
            Post postToDelete = this.GetById(id);
            this.posts.Remove(postToDelete);
            return postToDelete;
        }

        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            List<Post> result = this.posts;

            if (filterParameters.User != null && !string.IsNullOrEmpty(filterParameters.User.Username))
            {
                result = result.FindAll(p => p.CreatedBy.Username.Contains(filterParameters.User.Username));
            }          

            if (!string.IsNullOrEmpty(filterParameters.Title))
            {
                result = result.FindAll(p => p.Title.Contains(filterParameters.Title));
            }

            if (filterParameters.Category != null && !string.IsNullOrEmpty(filterParameters.Category.Name))
            {
                result = result.FindAll(p => p.Category.Name.Contains(filterParameters.Category.Name));
            }

            if (filterParameters.FromDateTime.HasValue && filterParameters.ToDateTime.HasValue)
            {
                result = result.FindAll(p => p.DateTime >= filterParameters.FromDateTime && p.DateTime <= filterParameters.ToDateTime).ToList();
            }
            else
            {
                if ((filterParameters.FromDateTime.HasValue))
                {
                    result = result.FindAll(p => p.DateTime >= filterParameters.FromDateTime).ToList();
                }
                if (filterParameters.ToDateTime.HasValue)
                {
                    result = result.FindAll(p => p.DateTime <= filterParameters.ToDateTime).ToList();
                }
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

    }
}
