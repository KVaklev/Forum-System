using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.QueryParameters
{
    public class PostSearchModel
    {
        public PaginatedList<Post> Posts { get; set; }

        public PostQueryParameters PostQueryParameters { get; set; }
    }
}
