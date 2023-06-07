namespace ForumManagementSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string User { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }
    }
}
