namespace ForumManagementSystem.Models
{
    public class Post
    {

        public int Id { get; set; }

        public User User { get; set; } // User user or string user? or not needed

        public int UserId { get; set; }

        public string Title { get; set; }

        public Category Category { get; set; } // Tag

        public int CategoryId { get; set; }

        public int Likes { get; set; }

        public string Content { get; set; }

        public List<Comment> Comments { get; set; }

        public DateTime DateTime { get; set; }

        //List<string> Tags { get; set; } ??

        //public int Likes { get; set; } - counter?
    }
}
