using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class CreatePostDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MinLength(16, ErrorMessage = "The {0} field must be at least {1} character.")]
        [MaxLength(64, ErrorMessage = "The {0} field must be less than {1} characters.")]
        public string Title { get; set; }  

        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MinLength(32, ErrorMessage = "The {0} field must be at least {1} character.")]
        [MaxLength(8192, ErrorMessage = "The {0} field must be less than {1} characters.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must be in the range from {1} to {2}.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must be in the range from {1} to {2}.")]
        public int CategoryId { get; set; }

        public List<string> Tags { get; set; }
    }
}
