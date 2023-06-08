using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }

        [MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(8192, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        // public int UserId { get; set; } - do we need this - FK?
        // public int PostId { get; set; } - do we need this - FK?
    }
}
