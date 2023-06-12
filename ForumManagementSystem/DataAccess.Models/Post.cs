using DataAccess.Models;

namespace ForumManagementSystem.Models
{
    public class Post
    {

        public int Id { get; set; }

        // Navigation property
        public User CreatedBy { get; set; }

        // Foreign key
        public int UserId { get; set; }

        public string Title { get; set; }

        // Navigation property
        public Category Category { get; set; }

        // Foreign key
        public int CategoryId { get; set; }

        public int Likes { get; set; }

        public string Content { get; set; }

        // Collection navigation containing dependents
        public List<Comment> Comments { get; set; } = new List<Comment> { };

        public DateTime DateTime { get; set; }

        // Collection navigation containing dependents
        List<Tag> Tags { get; set; } = new List<Tag>();

    }
}
