using ForumManagementSystem.Models;

namespace DataAccess.Models
{
    public class PostTag
    {
        // Foreign key
        public int PostId { get; set; }
        public int TagId { get; set; }

        // Navigation property
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
