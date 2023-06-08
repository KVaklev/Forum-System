namespace ForumManagementSystem.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string User { get; set; } // User user or string user? or not needed

        public string Title { get; set; }

        public Category Category { get; set; } // Tag

        public string Content { get; set; }

        public List<Comment> Comments { get; set; }

        public DateTime DateTime { get; set; }

        //List<string> Tags { get; set; } ??

        //public int UserId { get; set; } - do we need this - FK?

        //public int Likes { get; set; } - counter?
    }
}
