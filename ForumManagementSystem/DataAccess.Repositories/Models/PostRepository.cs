using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly List<Post> posts;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;

        public PostRepository(IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;


            posts = new List<Post>()

            {
                new Post
                {
                    Id = 1,
                    User = this.userRepository.GetById(1),
                    Title = "BMW 5, 2007",
                    Category = this.categoryRepository.GetById(1),
                    Content = "Have you ever experienced issues when start the engine",
                    DateTime = DateTime.Now
                },

                new Post
                {
                    Id = 2,
                    User = this.userRepository.GetById(2),
                    Title = "Mercedes GLC, 2012",
                    Category = this.categoryRepository.GetById(2),
                    Content = "Have you experienced issues with suspension making strange noise",
                    DateTime = DateTime.Now
                },

            };
        }


        public List<Post> GetAll()
        {
            return this.posts;
        }

        public Post GetByUser(User user)
        {
            Post post = this.posts.FirstOrDefault(post => post.User.Id == user.Id);
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
            return post ?? throw new EntityNotFoundException($"Post with title = {title} doesn't exist.");
        }
        public Post GetByCategory(string categoryName)
        {
            Post post = this.posts.Where(posts => posts.Category.Name == categoryName).FirstOrDefault();
            return post ?? throw new EntityNotFoundException($"Post with category = {categoryName} doesn't exist.");
        }

        public Post Update(int id, Post post)
        {
            Post postToUpdate = this.GetById(id);

            postToUpdate.User = post.User;
            postToUpdate.Title = post.Title;
            postToUpdate.Category = post.Category;
            postToUpdate.Content = post.Content;
            postToUpdate.Comments = post.Comments;

            return postToUpdate;

        }

        public Post Create(Post post, User user)
        {
            post.Id = this.posts.Count + 1;
            post.User.Id = user.Id;
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

            //if (!string.IsNullOrEmpty(filterParameters.User.Username))
            //{
            //    result = result.FindAll(p => p.User.Username.Contains(filterParameters.User.Username));
            //}
            if (!string.IsNullOrEmpty(filterParameters.Title))
            {
                result = result.FindAll(p => p.Title.Contains(filterParameters.Title));
            }
            //if (!string.IsNullOrEmpty(filterParameters.Category.Name))
            //{
            //    result = result.FindAll(p => p.Category.Name.Contains(filterParameters.Category.Name));
            //}
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
                    result = result.OrderBy(p => p.User).ToList();
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
    }
}
