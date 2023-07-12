using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.QueryParameters
{
    public class CommentSearchModel
    {
        public PaginatedList<Comment> Comments { get; set; }

        public CommentQueryParameters CommentQueryParameters { get; set; }
    }
}
