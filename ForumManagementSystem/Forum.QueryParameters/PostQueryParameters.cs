namespace ForumManagementSystem.Models
{
    public class PostQueryParameters
    {
        public User? User { get; set; }

        public string? Title { get; set; }

        public Category? Category { get; set; }

        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }
    }
}
