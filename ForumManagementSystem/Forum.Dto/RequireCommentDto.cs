using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dto
{
    public class RequireCommentDto
    {
        public string categoryName { get; set; }
        public string postTitle { get; set; }
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }


    }
}
