using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Models
{
    public class CommentGetViewModel
    {
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
    }
}
