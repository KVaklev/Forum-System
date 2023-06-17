using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class GetUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}
