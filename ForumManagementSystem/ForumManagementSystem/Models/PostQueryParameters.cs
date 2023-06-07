namespace ForumManagementSystem.Models
{
    public class PostQueryParameters
    {
        public string User { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}
