using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class GetUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
