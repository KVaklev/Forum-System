using DataAccess.Models;

namespace ForumManagementSystem.Models
{
    public class Post
    {

        public int Id { get; set; }

        public User User { get; set; } 

        public int UserId { get; set; }

        public string Title { get; set; }

        public Category Category { get; set; } 

        public int CategoryId { get; set; }

        public int Likes { get; set; }

        public string Content { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment> { };

        public DateTime DateTime { get; set; }

        List<Tag> Tags { get; set; } = new List<Tag>();

    }
}
