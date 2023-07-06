namespace ForumManagementSystem.Models
{
    public class CommentQueryParameters
    {
        public int? UserId { get; set; }

        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }

        public int? postID { get; set; }

		public int PageSize { get; set; } = 2;

		public int PageNumber { get; set; } = 1;

	}
}
