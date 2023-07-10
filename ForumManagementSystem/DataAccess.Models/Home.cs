using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;

namespace DataAccess.Models
{
    public class Home
    {
        public int UsersCount { get; set; }
        public int PostsCount { get; set; }
        public PaginatedList<Category> Categories { get; set; }

        public PaginatedList<Post> Posts { get; set; }

        public int PageNumber { get; set; }


    }
}
