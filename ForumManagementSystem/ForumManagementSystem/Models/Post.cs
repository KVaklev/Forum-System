namespace ForumManagementSystem.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string User { get; set; }

        public string Title { get; set; }

        public string Category { get; set; } // Tag

        public string Content { get; set; }

        public List<Comment> Comments { get; set; }

        public DateTime DateTime { get; set; }
    }
}
