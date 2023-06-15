using ForumManagementSystem.Models;

namespace DataAccess.Models
{
    public class PostTag
    {
        public int Id { get; set; }
        // Foreign key
        public int PostId { get; set; }
        public int TagId { get; set; }

        // Navigation property
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
