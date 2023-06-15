using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class Category
    {
        public int Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MaxLength(25, ErrorMessage = "The {0} field must be less than {1} characters.")]
        [MinLength(5, ErrorMessage = "The {0} field must be at least {1} character.")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MaxLength(1000, ErrorMessage = "The {0} field must be less than {1} characters.")]
        [MinLength(5, ErrorMessage = "The {0} field must be at least {1} character.")]
        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        // Collection navigation containing dependents
        public List<Post> Posts { get; set; }

        //TODO - Counts in Category
        public int CountPosts { get; set; }
        
        public int CountComment { get; set; }

    }
}
