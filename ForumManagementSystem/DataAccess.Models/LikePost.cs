using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public  class LikePost
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public int? UserId { get; set; } 

        public User User { get; set; }

        public bool IsLikedPost { get; set; }
    }
}
