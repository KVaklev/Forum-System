using ForumManagementSystem.Models;

namespace DataAccess.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public User CreatedBy { get; set; }

        //TODO
        // Foreign key
        public int PostId { get; set; }

        // Navigation property
        public Post Post { get; set; }

        // Collection navigation containing dependents
        List<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
