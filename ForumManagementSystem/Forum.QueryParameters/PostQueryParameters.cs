namespace ForumManagementSystem.Models
{
    public class PostQueryParameters
    {
        public string? Username { get; set; }

        public string? Title { get; set; }

        public string? Category { get; set; }

        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }

        public int? UserId { get; set; }

        public string? Tag { get; set; }

        public int? CategoryId { get; set; }

        public int PageSize { get; set; } = 2;
        public int PageNumber { get; set; } = 1;
    }
}
