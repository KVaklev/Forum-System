using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly List<Post> posts;

        public PostRepository()
        {
            posts = new List<Post>()

            {
                new Post
                {
                    Id = 1,
                    User = "Ivan Ivanov",
                    Title = "BMW 5, 2007",
                    Category = "Engine",
                    Content = "Have you ever experienced issues when start the engine",
                    DateTime = DateTime.Now
                },

                new Post
                {
                    Id = 2,
                    User = "John Smith",
                    Title = "Mercedes GLC, 2012",
                    Category = "Suspension",
                    Content = "Have you experienced issues with suspension making strange noise",
                    DateTime = DateTime.Now
                },

            };
        }
        public Post Create(Post post)
        {
            post.Id = this.posts.Count + 1;
            this.posts.Add(post);
            return post;
        }

        public Post Delete(int id)
        {
            Post postToDelete = this.GetByID(id);
            this.posts.Remove(postToDelete);
            return postToDelete;
        }

        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            List<Post> result = this.posts;

            if (!string.IsNullOrEmpty(filterParameters.User))
            {
                result = result.FindAll(p => p.User.Contains(filterParameters.User));
            }
            if (!string.IsNullOrEmpty(filterParameters.Title))
            {
                result = result.FindAll(p => p.Title.Contains(filterParameters.Title));
            }
            if (!string.IsNullOrEmpty(filterParameters.Category))
            {
                result = result.FindAll(p => p.Category.Contains(filterParameters.Category));
            }
            if (filterParameters.FromDateTime.HasValue)
            {
                result = result.FindAll(p => p.DateTime <= filterParameters.FromDateTime).ToList();
            }
            if (filterParameters.ToDateTime.HasValue)
            {
                result = result.FindAll(p => p.DateTime <= filterParameters.ToDateTime).ToList();
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

        public List<Post> GetAll()
        {
            return posts;
        }

        public Post GetByCategory(string category)
        {
            var post = posts.FirstOrDefault(p => p.Category == category );
            return post ?? throw new EntityNotFoundException($"Post with category={category} doesn't exist.");
        }

       
        public Post GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Post GetByTitle(string title)
        {
            throw new NotImplementedException();
        }

              
        public Post GetByUser(string user)
        {
            throw new NotImplementedException();
        }

        public Post Update(int id, Post post)
        {
            throw new NotImplementedException();
        }
    }
}
