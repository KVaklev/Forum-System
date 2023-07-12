using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;

namespace Business.QueryParameters
{
    public class UserSearchModel
    {
        public PaginatedList<User> Users { get; set; }

        public UserQueryParameters UserQueryParameters { get; set; }
    }
}
