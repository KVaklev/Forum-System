using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class GetUserDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(30, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please provide a valid email.")]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}
