namespace ForumManagementSystem.Models
{
    public class CategoryQueryParameter
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }

        public int PageSize { get; set; } = 2;
        public int PageNumber { get; set; } = 1;
    }
}
