using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class CommentDto
    {
        public int PostId { get; set; }

        public int UserId { get; set; }

        [MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(8192, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Content { get; set; }

    }
}
