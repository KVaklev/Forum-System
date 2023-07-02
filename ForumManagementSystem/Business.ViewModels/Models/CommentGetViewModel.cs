using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Models
{
    public class CommentGetViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
    }
}
