using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must be in the range from {1} to {2}.")]  //TODO  - MaxValue - user.Count
       
        // Foreign key
        public int UserId { get; set; }
        
        // Navigation property
        public User CreatedBy { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must be in the range from {1} to {2}.")] //TODO  - MaxValue - post.Count

        // Foreign key
        public int PostId { get; set; }

        // Navigation property
        public Post Post { get; set; }

        [MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(8192, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public int LikesCount { get; set; }

        public List<LikeComment> Likes { get; set; } = new List<LikeComment>();
    }
}
