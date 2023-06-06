namespace ForumManagementSystem.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string User { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }


        //public User users { get; set; }

    }
}
