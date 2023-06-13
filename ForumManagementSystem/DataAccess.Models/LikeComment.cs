using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class LikeComment
    {
        public int Id { get; set; }

        public int CommentId { get; set; }

        public Comment Comment { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public bool IsLiked { get; set; }


     }
}
