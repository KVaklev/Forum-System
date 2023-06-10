using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class CategoryDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MaxLength(25, ErrorMessage = "The {0} field must be less than {1} characters.")]
        [MinLength(5, ErrorMessage = "The {0} field must be at least {1} character.")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MaxLength(100, ErrorMessage = "The {0} field must be less than {1} characters.")]
        [MinLength(5, ErrorMessage = "The {0} field must be at least {1} character.")]
        public string Description { get; set; }

    }
}
