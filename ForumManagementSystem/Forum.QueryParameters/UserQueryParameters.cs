namespace ForumManagementSystem.Models
{
    public class UserQueryParameters
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public bool? Admin { get; set; }
        public bool? Blocked { get; set; }

    }
}
