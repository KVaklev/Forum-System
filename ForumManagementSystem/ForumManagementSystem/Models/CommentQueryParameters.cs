namespace ForumManagementSystem.Models
{
    public class CommentQueryParameters
    {
        public string? UserName { get; set; }

        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }

    }
}
