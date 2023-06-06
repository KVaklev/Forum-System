namespace ForumManagementSystem.Models
{
    public class CategoryQueryParameters
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}
