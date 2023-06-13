namespace ForumManagementSystem.Models
{
    public class PostQueryParameters
    {
        public string? Username { get; set; }

        public string? Title { get; set; }

        public string? Category { get; set; }

        public string? FromDateTime { get; set; }

        public string? ToDateTime { get; set; }

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }

        public int? UserId { get; set; }

        public int? CategoryId { get; set; }
    }
}
