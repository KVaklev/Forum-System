using ForumManagementSystem.Models;

namespace DataAccess.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //TODO
        // Foreign key
        public int PostId { get; set; }

        // Navigation property
        public Post Post { get; set; }

        // Collection navigation containing dependents
        public List<Post> Posts { get; set; }
    }
}
