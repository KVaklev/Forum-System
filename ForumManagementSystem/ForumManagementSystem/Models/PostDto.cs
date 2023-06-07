using ForumManagementSystem.Services;
using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class PostDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MinLength(16, ErrorMessage = "The {0} field must be at least {1} character.")]
        [MaxLength(64, ErrorMessage = "The {0} field must be less than {1} characters.")]
        public string Title { get; set; }
               

        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MinLength(32, ErrorMessage = "The {0} field must be at least {1} character.")]
        [MaxLength(8192, ErrorMessage = "The {0} field must be less than {1} characters.")]
        public string Content { get; set; }
             

    }
}
