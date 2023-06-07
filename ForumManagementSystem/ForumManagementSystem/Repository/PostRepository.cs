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


        public List<Post> GetAll()
        {
            return this.posts;
        }

        public Post GetByUser(string user)
        {
            Post post = this.posts.Where(posts => posts.User == user).FirstOrDefault();
            return post ?? throw new EntityNotFoundException($"Post with user = {user} doesn't exist.");
        }

        public Post GetByID(int id)
        {
            Post post = this.posts.Where(posts => posts.Id == id).FirstOrDefault();
            return post ?? throw new EntityNotFoundException($"Post with ID = {id} doesn't exist.");

        }

        public Post GetByTitle(string title)
        {
            Post post = this.posts.Where(posts => posts.Title == title).FirstOrDefault();
            return post ?? throw new EntityNotFoundException($"Post with title = {title} doesn't exist.");
        }
        public Post GetByCategory(string category)
        {
            Post post = this.posts.Where(posts => posts.Category == category).FirstOrDefault();
            return post ?? throw new EntityNotFoundException($"Post with category = {category} doesn't exist.");
        }

        public Post Update(int id, Post post)
        {
            Post postToUpdate = this.GetByID(id);
            postToUpdate.User = post.User;
            postToUpdate.Title = post.Title;
            postToUpdate.Category = post.Category;
            postToUpdate.Content = post.Content;
            postToUpdate.Comments = post.Comments;

            return postToUpdate;

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
